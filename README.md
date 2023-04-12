# Tennis-Manager
This repository contains both an Angular client and an ASP.NET Web API backend which were utilized in the creation of a tennis manager application. The application enables users to book available slots on a tennis court.

## Technologies
The following technologies were utilized in this project:
- Angular
- ASP.NET Web API
- .NET 6
- Entity Framework Core (Code first approach)
- Open API Swagger

## Client
The client application is an Angular app that enables users to view and book available tennis court slots. Additionally, users can also edit and delete previously booked slots as necessary.

### Getting Started
To run the client application locally, follow these steps:

1. Clone the repository
2. Navigate to the `client` directory
3. Run `npm install` to install dependencies
4. Run `npm start` to start the client application

The client application will run on `http://localhost:4200`

### Generate client
To generate an API client in the client project, you will need to have Swagger installed by running `npm install -g swagger`. Once this is completed, you can proceed to run `npm run generate:client`. This will create all models and services for the frontend application using the Swagger API documentation.

## Server

The server is an ASP.NET Web API that has been built on .NET 6. It provides endpoints that allow for the creation and management of user accounts, booking and canceling of tennis court slots, and the retrieval of available slots.

### Security

The server implements several security features aimed at safeguarding user data:

- Passwords are stored in hashed form using the `System.Security.Cryptography` library to prevent unauthorized access.
- Refresh tokens are implemented to allow users to obtain new access tokens without needing to re-enter their username and password. Refresh tokens are securely stored on the server and encrypted using the Data Protection API.
- JWT access tokens are used to authenticate API requests. These tokens are signed with a secret key and have an expiration time to prevent unauthorized access to the API.

### Getting started

To run the server locally, follow these steps:

1. Clone this repository.
2. Navigate to the server directory.
3. Open the solution file TennisManager.sln in Visual Studio or another .NET IDE.

Next, you will need to add the `appsettings.json` and `appsettings.Development.json` files. Copy the following content into the `appsettings.Development.json` file and adjust it to meet your needs.
```
{
  "ConnectionStrings": {
    "TennisDb": "server=(LocalDB)\\mssqllocaldb;attachdbfilename=|DataDirectory|\\TennisDB.mdf;database=TennisDB;integrated security=True"
  },
  "Settings": {
    "JwtSecretKey": "Replace this one",
    "JwtValid": "5", 
    "RefreshTokenValid": "60"
  }
}
```
If you need to make changes to the database, add them and execute the following command in the `Package Manager Console` to ensure that your changes are reflected:

`Add-Migration test -StartupProject Tennis.Api -Context TennisContext -Project Tennis.Database`

### API Documentation
The server API is documented using Swagger. To view the API documentation, run the server and navigate to `http://localhost:5000/swagger/index.html` or `https://localhost:5001/swagger/index.html for secure connections.


## Legacy Solution
This repository also contains a legacy solution that served as the origin of the idea for this project. The legacy solution was a school project created in the past.

## Suggestions
If you have any suggestions for how to improve this repository, please feel free to open an issue or submit a pull request. I welcome feedback and contributions from the community.

## License
This repository is licensed under the MIT License. See the `LICENSE` file for more information.
