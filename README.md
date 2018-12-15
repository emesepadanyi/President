# President

## Requirements
This is a card game web application. To be able to run this program, you need Visual Studio version 15.7 or higher, and Visual Studio Code.


## Server side application
After forking or downloading the repository, open the server application from `\President\President.API\President.API.sln`.

First the configurations should be set up correcly. To do that, right click on the President.API project, and than Properties. Underneath the Debug menupoint, you should uncheck the "Launch browser" checkbox. On the same settings page, the "App URL" should be `https://localhost:5001;http://localhost:5000` in order to work properly.

When Visual Studio has downloaded all of the dependencies, you should be able to start the application by selecting President.API from the dropdown menu.

## Database

In Visual Studio, open the `Server Explorer` view. Right click on the `Data Connections` submenu, and click `Create New SQL Server Database`. On the panel chose `Microsoft SQL Panel`, and click continue. The name of the server should be `localhost` and the database can be anything, for example `PresidentGame`. Copy the database connection string by right clicking on the new database, and than Properties. Than go to `appSettings.json` and change the `DefaultConnection` value to the new string.

After that, open the Package Manager Console by clicking on `Tools -> NuGet Package Manager -> Package Manager Console`. In the new view set the default project to President.DAL, and run `Update-Database`. That creates the empty tables that are necessary to run the application.

## Client side application
To be able to run the client side application, open the project containing folder in Visual Studio Code by clicking on `File -> Open folder -> President/President.Client`. After that open a new terminal by clicking on `Terminal -> New terminal`, and type `npm install` into the opened terminal. This will take a few minutes. After the dependencies are installed, you should be able to start the application by typing `ng serve` into the terminal.

Since the application uses local storage, only one instance can be run in one browser, otherwise they would write over eachothers' data. So, in order to imitate 4 users, you can start the application in four different browsers, or an easier solution is to start the client side application twice, on two different ports: 
* from one terminal: `ng serve --port 4200`,
* from an other terminal: `ng serve --port 4201`,

and open the application on those two separate links, and repeat the same from a different or an incognito browser. This way the login data would not overwrite eachother, and the application will work smoothly.