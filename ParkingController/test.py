import RPi.GPIO as GPIO
import time

GPIO.setmode(GPIO.BCM) 
GPIO.setup([5, 6], GPIO.IN)

while True:
    if GPIO.input(5) == GPIO.LOW:
        print("enter low")
    else:
        print("enter high")
    if GPIO.input(6) == GPIO.LOW:
        print("leave low")
    else:
        print("leave high")
    
    time.sleep(2)