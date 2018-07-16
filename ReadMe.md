# Introduction
Using the familiar development environments such as Visual Studio and C# for the development of application based on the.NET Core 2.1 framework makes the implementation of tools and web applications even easier. Leveraging the Windows 10 IoT Core platform such applications can be deployed to small devices such as the Raspberry PI 2 or PI 3.

## Modbus
A typical scenario for a Raspberry PI based application is home automation. The communication to various devices is key to the development of IoT applications. Many devices provide a Modbus interface either using TCP/IP or serial interfaces using Modbus RTU (see [Modbus](https://en.wikipedia.org/wiki/Modbus) at Wikipedia).

## Goal
Using various libraries from the community and the.NET Core 2.1 framework the implementation of applications to read and write Modbus slaves either via a command line application or via a web based interface is the main provides simple tools for the testing and development of monitoring and control applications.
Using Modbus TCP or Modbus RTU, the application should provide the following features:

* Reading and writing single or multiple coils.
* Reading single or multiple discrete inputs.
* Reading and writing single or multiple holding registers.
* Reading single or multiple input registers.

Since the Modbus specification typically only supports boolean and 16-bit integer date several extension have been used to provide access to a variety of additional datatypes such as floats, doubles, and even strings.

## Console Application
Using the Console Application template as a start, application settings, logging, and command line processing are added using several libraries:

* Windows 10 IoT
* ASP.NET Core 2.1
* CommandLine.Core Framework
* Serilog Logging Framework
* Modbus TCP (NModbus Library)

The application settings allows to pre-set common communcation or logging settings.

## ASP.NET Web Application
The standard ASP.NET web application template using razor pages and an Sqlite database for the individual user authentication has been used. Several pages are added to display selected data from the various components. An additional page is used to embed the Swagger Web API. The Swagger pages and selected other pages require authentication to be accessed. The ASP.NET Core Web application also uses HTTPS as the only protocol to access the pages.

### REST API and Swagger
The various data from the Modbus devices are made available via set of REST based web API's. Swagger integration provided by the Swashbuckle project adds Swagger to the NModbusTCP project providing the Swagger-UI to provide a rich discovery, documentation and playground experience to the REST API consumers.

### Controller Implementation
The following basic Modbus MVC controllers have been implemented:

* CoilController (reading and writing a single coil)
* CoilsController (reading and writing multiple coils)
* DiscreteInputController (reading a single discrete input)
* DiscreteInputsController (reading multiple discrete input)
* HoldingRegisterController(reading and writing a single holding register)
* HoldingRegistersController (reading and writing multiple holding registers)
* InputRegisterController (reading a single input register)
* InputRegistersController (reading multiple input registers)

and the Modbus extension controller for additional datatypes:

* ROSingleController (reading input registers)
* ROArrayController (reading input registers)
* RWSingleController (reading holding registers)
* RWArrayController (reading holding registers)

### Deployment
The applications can be deployed on the various platforms supported by the ASP.NET Core 2.1 framework. It also runs on a Raspberry PI 3 or Raspberry PI 2 B using Windows 10 IoT by simply publishing to a directory on the Raspberry PI.

## Summary
The use of the .NET Core 2.1 framework allows the implementation of various command line tools and simple Web applications. The REST based Web API based access to Modbus devices not only provides a HTTP gateway to Modbus TCP but also allows secure access via HTTPS (Note: Modbus has no security layer in Modbus TCP or Modbus RTU).