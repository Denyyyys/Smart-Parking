from enum import Enum
import requests
from Driver import Driver, DriverStatus
import json
import RPi.GPIO as GPIO
import time

class PlaceStatus(Enum):
    TAKEN = (0,)
    FREE = (1,)
    BLINK = 2


class Parking:
    def __init__(self, num_places: int, host: str, token: str) -> None:
        self.auth_token_ = token
        self.num_places_ = num_places
        self.places_container: dict = dict()
        self.sensors_:dict = dict()
        self.leds_:dict[dict] = dict()
        self.host_ = host
        for i in range(num_places):
            self.places_container[i] = PlaceStatus.FREE
        self.drivers_: list[Driver] = []
        self.entrance_button_ = 0
        self.leave_button_ = 0
        self.entrance_led_ = 0
        self.leave_led_ = 0

    def createDbInstance(self):
        url = f"https://{self.host_}/api/Parking"
        headers = {
            "Authorization": f"Bearer {self.auth_token_}",
            "Content-Type": "application/json",
        }
        payload = json.dumps(
            {
                "Name": "Example Parking",
                "NumberParkingSpaces": self.num_places_,
                "City": "Example City",
                "Street": "Example Street",
                "PostalCode": "12345",
            }
        )

        retv = requests.post(url, payload, headers=headers, verify=False)
        print(f"Recieved response: {retv.text}")

    def configureSensors(self, place_sensor_map:dict) -> None:
        for i in range(self.num_places_):
            if i in place_sensor_map.keys():
                self.sensors_[i] = place_sensor_map[i]
            else:
                self.sensors_[i] = -1

    def configureLEDs(self, place_led_map:dict[dict]):
        for i in range(self.num_places_):
            if i in place_led_map.keys():
                self.leds_[i] = place_led_map[i]
            else:
                self.leds_[i] = -1

    def configureButtonsNotifiers(self, enter:tuple[int, int], leave:tuple[int, int]):
        self.entrance_button_ = enter[0]
        self.leave_button_ = leave[0]
        self.entrance_led_ = enter[1]
        self.leave_led_ = leave[1]
    

    def setPlaceStatus(self, place: int, status: PlaceStatus) -> None:
        self.places_container[place] = status

    def getPlaceStatus(self, place: int) -> PlaceStatus:
        return self.places_container[place]

    def updateLighting(self) -> None:
        for i in range(self.num_places_):
            if self.places_container[i] == PlaceStatus.TAKEN:
                if "red" in self.leds_[i].keys():
                    GPIO.output(self.leds_[i]["red"], False)
                if "green" in self.leds_[i].keys():
                    GPIO.output(self.leds_[i]["green"], True)
                if "yellow" in self.leds_[i].keys():
                    GPIO.output(self.leds_[i]["yellow"], True)
            elif self.places_container[i] == PlaceStatus.FREE:
                if "red" in self.leds_[i].keys():
                    GPIO.output(self.leds_[i]["red"], True)
                if "green" in self.leds_[i].keys():
                    GPIO.output(self.leds_[i]["green"], False)
                if "yellow" in self.leds_[i].keys():
                    GPIO.output(self.leds_[i]["yellow"], True)
            elif self.places_container[i] == PlaceStatus.BLINK:
                if "red" in self.leds_[i].keys():
                    GPIO.output(self.leds_[i]["red"], True)
                if "green" in self.leds_[i].keys():
                    GPIO.output(self.leds_[i]["green"], True)
                if "yellow" in self.leds_[i].keys():
                    GPIO.output(self.leds_[i]["yellow"], False)

    def addDriverToDb(self, license_plate: str, place: int) -> bool:
        url = f"https://{self.host_}/api/Parking/driver/{license_plate}"
        headers = {
            "Authorization": f"Bearer {self.auth_token_}",
            "Content-Type": "application/json",
        }
        payload = json.dumps({"MaxSecondsParking": 1200, "ParkingPlace": place+1})
        retv = requests.post(url, data=payload, headers=headers, verify=False)
        print(f"Recieved response: {retv}: {retv.text}")
        if retv.status_code == 404:
            return False
        else: return True

    def removeDriverFromDb(self, license_plate: str):
        url = f"https://{self.host_}/api/Parking/driver/{license_plate}"
        headers = {
            "Authorization": f"Bearer {self.auth_token_}",
            "Content-Type": "application/json",
        }
        retv = requests.delete(url, headers=headers, verify=False)
        print(f"Recieved response: {retv}")

    def parkDriver(self, license_plate: str, place: int) -> bool:
        for driver in self.drivers_:
            if driver.getLicensePlate() == license_plate:
                driver.parked(place)
                self.setPlaceStatus(place, PlaceStatus.TAKEN)
                self.updateLighting()
                self.addDriverToDb()
                return True

        return False

    def addDriver(self) -> None:
        self.drivers_.append(Driver())

    def deleteDriver(self, license_plate: str) -> bool:
        for driver in self.drivers_:
            if driver.getLicensePlate() == license_plate:
                self.drivers_.remove(driver)
                self.removeDriverFromDb(driver.getLicensePlate())
                return True
        return False

    def checkFlashingPlaces(self) -> bool:
        url = f"https://{self.host_}/api/Parking/flashingplaces"
        headers = {"Authorization": f"Bearer {self.auth_token_}"}
        retv = requests.get(url, headers=headers, verify=False)
        print(f"Recieved response: {retv.text}")  # TODO interpret response
        flashing_ids = retv.json
        sth_changed = False
        for id in flashing_ids:
            if self.places_container[id-1] != PlaceStatus.BLINK:
                self.places_container[id-1] = PlaceStatus.BLINK
                sth_changed = True
        if sth_changed:
            self.updateLighting()

        return True

    def anyDriverEntering(self):
        for driver in self.drivers_:
            if driver.getStatus() == DriverStatus.ENTERING:
                return True
        return False

    def checkPlaces(self):
        sth_changed = False
        for i in range(self.num_places_):
            # checking the appropriate sensor
            if self.sensors_[i] == -1:
                continue
            sensed = not bool(GPIO.input(self.sensors_[i]))
            if sensed and self.places_container[i] != PlaceStatus.TAKEN:
                sth_changed = True
                while (True):
                    if not self.anyDriverEntering():
                        return
                    license_plate = input("Enter license plate number: ")
                    if self.addDriverToDb(license_plate, i):
                        for driver in self.drivers_:
                            if driver.getStatus() == DriverStatus.ENTERING:
                                driver.parked(i, license_plate)
                        break
                self.places_container[i] = PlaceStatus.TAKEN
            elif (not sensed) and self.places_container[i] != PlaceStatus.FREE:
                sth_changed = True
                self.places_container[i] = PlaceStatus.FREE
                for driver in self.drivers_:
                    if driver.parked_place_ == i:
                        driver.leftPlace()
                        self.deleteDriver(driver.getLicensePlate())


        if sth_changed:
            self.updateLighting()

    def checkDriverEntering(self):
        if GPIO.input(self.entrance_button_) == GPIO.HIGH:
            GPIO.output(self.entrance_led_, False)
            self.addDriver()
            time.sleep(5)
            GPIO.output(self.entrance_led_, True)

    
    def checkDriverLeaving(self):
        if GPIO.input(self.leave_button_) == GPIO.HIGH:
            GPIO.output(self.leave_led_, False)
            time.sleep(5)
            GPIO.output(self.leave_led_, True)
            