from enum import Enum
import datetime


class DriverStatus(Enum):
    ENTERING = (0,)
    PARKED = (1,)
    LEAVING = 2


class Driver:
    def __init__(self):
        self.license_plate_ = None
        self.parked_place_ = -1
        self.status_ = DriverStatus.ENTERING
        self.time_entered_ = 0
        self.time_parked = 0

    def parked(self, place: int,license_plate: str) -> bool:
        self.time_entered_ = datetime.datetime.now()
        self.license_plate_ = license_plate
        self.status_ = DriverStatus.PARKED
        self.parked_place_ = place

    def leftPlace(self) -> bool:
        self.status_ = DriverStatus.LEAVING
        self.parked_place_ = -1

    def getLicensePlate(self):
        return self.license_plate_

    def getStatus(self) -> DriverStatus:
        return self.status_
