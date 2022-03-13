# sfeats

[Take Home Engineering Challenge](https://github.com/timfpark/take-home-engineering-challenge) for Wesley Evans

The solution presented here solves the food truck dilemma by presenting a [web API](https://sfeats.azurewebsites.net/swagger/index.html) capable of finding the nearest food trucks to a given location in the San Francisco area.

## Explanation of the parameters:

| Parameter | Valid Values | Description |
| --------------- | --------------- | --------------- |
| DataProvider | **csv** or **datasf** |  "csv" uses the source data originally provided by the [Take Home Engineering Challenge](https://github.com/timfpark/take-home-engineering-challenge) for the problem. "datasf" uses the source API provided by the city of San Francisco. |
| TravelMode | **Driving**, **Walking**, **Transit**, or **Truck** | The travel method used to calculate the distance and duration |
| Sort | **Distance** or **Duration** | Returns results sorted by either travel distance or time traveled |
| Units | **Miles** or **Kilometers** | Unit to use for reporting travel distance |
| Count | *integer* | Number of results to resturn |
| latitude | *double* | The latitude of the starting location |
| longitude | *double* | The longitude of the starting location |
| address | *string* | Street address in the San Francisco area. If latitude/longitude are set it will use them as the origin, otherwise it will use the street address. |

## Design considerations
- Solution uses dependency injection to support services. In the case of data provider, it allows use of different providers as requested by the user.
- Unit tests to validate data from service providers
- Route data provided by [BingMapsRESTToolkit](https://github.com/microsoft/BingMapsRESTToolkit). Originally, I implemented the solution using their DistanceMatrixRequest, however, once I added unit tests and compared the values to what was returned by typing directly into Bing Maps, I found the values differed significantly. I then switched to using RouteRequests instead. This led to performance issues as I had to make a single call for every address. However, I was ultimately able to break apart the request by batching the requests as different waypoints on the same route, and calling them all asynchronously.
- In order to not reveal my BingMaps API key, I chose to use Azure KeyVault.

## Limitations
As this is a coding exercise that is intended to be a 3 hour exercise, there were several features that I ran out of time to properly implement. This is a list of some of those items:
- I wanted to deploy this project using a pipeline, however, the Swashbuckle build was having issues with the .NET 6 Minimal API so I was unable to resolve this in time so it uses a manual publish from Visual Studio.
- I wanted to get the DistanceMatrixRequest working to reduce the number of calls to Bing
- I wanted to move the Azure KeyVault code into a separate service.
- I wanted to move some of the code from the FacilitiesController to a separate service 
- I wanted to strengthen error handling and messaging
- I wanted to move some more of the strings to the appsettings.json file
