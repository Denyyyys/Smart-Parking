# Smart Parking

This student project showcases our implementation of a Smart Parking system designed to enhance the driver experience during parking. The documentation is available in Polish in the file `dokumentacja.pdf`.

## Back end

The back end is developed as an API using ASP.NET Core technology. It offers the following features:

- User Authentication: Utilizing JSON Web Tokens for registering and logging in users and parking administrators.
- Driver Management: Allows administrators to add new drivers to the parking system.
- Parking Management: Administrators can create, retrieve, modify, and delete parking entries.
- Parking Space State Management: The state of a parking space can be set to free, occupied, or flashed (when a driver is looking for this space).

To explore all endpoints, start the server and navigate to the documentation at https://localhost:7026/swagger/index.html.

## Front end

The front end is built with React and is optimized for mobile screens, given the assumption that drivers will use the application in their cars near the parking area.

## Hardware

The `ParkingController` folder contains scripts for a Raspberry Pi, which controls the sensors and LEDs of our prototype.

### Team members

- Denys Fokashchuk: back-end and front-end developer
- Filip Szczygielski: front-end developer
- Piotr Kluba: embedded systems engineer

### Note

This project is cloned from our team repository, which was hosted by the Faculty of Electronics and Information Technology (Elka) at the Warsaw University of Technology (Politechnika Warszawska), which is not accessible for everyone except students and teachers of that university.
