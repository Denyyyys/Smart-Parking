from Parking import Parking, PlaceStatus
import time
import requests
import RPi.GPIO as GPIO
import ssl

# constants

HOSTNAME = "192.168.0.15:7026"
NUM_PLACES = 10


def registerAdmin():
    url = f"https://{HOSTNAME}/api/account/register"
    payload = '{\r\n    "RegistrationType": "parkingAdmin",\r\n    "Email": "parkingAdmin1@mail.com",\r\n    "Password": "qwerty123",\r\n    "FirstName": "John",\r\n    "LastName": "Doe"\r\n}   '
    headers = {"Content-type": "application/json", "Accept": "application/json"}
    retv = requests.post(url, data=payload, verify=False, headers=headers)
    print(f"Recieved token: {retv.content}")
    return retv.json()["token"]


# init
token = registerAdmin()
myParking: Parking = Parking(NUM_PLACES, HOSTNAME, token)
GPIO.setmode(GPIO.BCM) 
GPIO.setup([21, 20, 16, 5, 6], GPIO.IN) 
GPIO.setup([26, 19, 13, 22, 27, 17, 18, 23, 24, 12, 9, 10], GPIO.OUT)
GPIO.output([26, 19, 13, 22, 27, 17, 18, 23, 24, 12, 9, 10], True)
myParking.configureSensors({0: 16 , 1: 20, 2: 21})
myParking.configureLEDs({0:{"red": 19,
                             "green": 26,
                             "yellow": 13}, 
                             1: {"red": 17,
                             "green": 22,
                             "yellow": 27}, 
                             2: {"red": 18,
                             "green": 24,
                             "yellow": 23},
                             
                             3: {"green": 12}, 
                             4: {"green": 12}, 
                             5: {"green": 12}, 
                             6: {"green": 12}, 
                             7: {"green": 12},
                               8: {"green": 12},
                               9: {"green": 12}})
myParking.configureButtonsNotifiers((5, 10), (6,9))
myParking.updateLighting()

myParking.createDbInstance()

time.sleep(5)

# main loop


while True:
    myParking.checkDriverEntering()
    myParking.checkFlashingPlaces()
    myParking.checkPlaces()
    myParking.checkDriverLeaving()
    time.sleep(1)