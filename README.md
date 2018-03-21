# Pup Pals
Keeping track of your pet's social network

PupPals is a web application that allows users to track information about the houses they encounter while walking their pet. PupPals was built using ASP.NET MVC and connects to a SQLite database. This application utilizes the Google Maps Javascript API, Google Maps Geocoding API and Google Places API Web Service to create a visual representation of the information a user tracks and allows the user to search the map.

To run the application on your computer:
1. Register for a Google Maps API key. Make sure the following APIs are enabled:
    * Google Maps JavaScript API
    * Google Maps Geocoding API
    * Google Places API Web Service
    
2. Clone the repository
3. Create an appsettings.json with the following information
    ```
    {
        "ConnectionStrings": {
          "DefaultConnection": "DataSource={PATH TO DATABASE FILE}"
        },
        "Logging": {
          "IncludeScopes": false,
          "LogLevel": {
            "Default": "Warning"
          }
        },
        "ApplicationConfiguration": {
          "GoogleAPIKey": "{YOUR GOOGLE MAPS API KEY}"
        }
      }
    ```
 4. `dotnet restore` to install required packages
 5. Run `dotnet ef database update` to create your database. <br>
 NOTE: This application is configured for a SQLite database file.
    
