# TODO-LIST

About: This is a web application; user can add new todo items to a list, user can mark todo items as complete and user can view current and historic todo items. 
Front-end is build using Angular 11 with bootstrap and back-end service is build using Asp.Net Core 3.1. It is using Sqlite3 database, entity framework, JwtToken authentication, xUnit, Moq and Serilog.

# TodoListService:
Pre-requisite to debug code: Asp.Net Core 3.1, Microsoft Visual Studio Community 2019.

Desktop Setup:
1.	Download the code from Github and upzip it at your desktop.
2.	Open the solution in VS 2019 / or Open VS command prompt and navigate to the solution directory and run command “dotnet restore”
3.	Build the code using VS2019 / or command “dotnet msbuid”
4.	Run the application using VS 2019 without IIS/IIS Express / or go to bin/debug folder and run the .exe file.
5.	It will run the application by default at http://localhost:5000

Note: 
1.	Sqlite3 database path is configured in the appsettings.json file in main service application. If there is any issue while connection to Sqlite3 db, please change the path in appsettings.json. You can find the database file inside App_Data directory of the project.
2.	To run the test cases from the test project, please change the Sqlite3 database file path in code, the db file path is hardcoded in file TodoListControllerTests.cs and AuthenticationControllerTests.cs. There are test data available in test db at App_Data directory of test project.
3.	Log file relative's path is configured in appsettings.json file. If log is not getting generated in your project root directory then you can hardcode the absolute path in appsettings.json file.


# Todo-list-angular
Pre-requisite to debug code: Visual Studio Code or Visual Studio Community 2019, NodeJS and Angular-CLI.

Desktop Setup
1.	Download the code from Github and upzip it at your desktop.
2.	Open directory “todo-list-angular” in VS Code / or Open command prompt and navigate to angular application’s main folder “todo-list-angular”.
3.	To install dependencies run command “npm install” using VS Code terminal / or run “npm install” from command prompt.
4.	To run the application run command “ng serve” using VS Code terminal / or run “ng server using command prompt”.
5.	Launch the chrome browser and browse http://localhost:4200

Note: 1. Angular app will run default at port 4200. If it is not able to connect to TodoListService then please check and confirm that TodoListService is up and running. TodoListService runs default on port 5000. If it is running on different port then you can correct the TodoListService’s url in angular’s environment file and re-launch the angular app.
2. Right now username and password is harcoded in encironment file. we can create a login page later and take the username and password from login page.


# Database Sqlite3:
Path - main project: App_Data/TodoListDB.db
Path – test project : App_Data/TodoListDB.Test.db

Tables:
1.	TODO: 
CREATE TABLE "Todo" (
	"id"	INTEGER NOT NULL,
	"userId"	TEXT NOT NULL,
	"description"	TEXT NOT NULL,
	"isCompleted"	INTEGER NOT NULL DEFAULT 0,
	"isActive"	INTEGER NOT NULL DEFAULT 1,
	PRIMARY KEY("id" AUTOINCREMENT)
)

2.	USER: 
CREATE TABLE "User" (
	"userId"	TEXT NOT NULL,
	"password"	TEXT NOT NULL,
	"firstName"	TEXT,
	"lastName"	TEXT,
	"emailId"	TEXT NOT NULL,
	"isActive"	INTEGER NOT NULL DEFAULT 1,
	PRIMARY KEY("userId")
)

