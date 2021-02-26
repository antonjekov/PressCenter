# PressCenter https://noticiasenoriginal.azurewebsites.net/

News aggregator for the press releases of the Spanish government sites written in ASP.NET Core.

## Project Structure 
The project contains two applications that work independently and use the same database.
- First application only collects, normalize, and save data to database. 
- Second application is .NET Core WEB that only represent information. https://noticiasenoriginal.azurewebsites.net/

## Used technologies and libraries:
-	.NET
-	Azure functions
-	MS SQL Database (Azure SQL Database)
-	Entity Framework Core
-	StyleCop
-	Automapper
-	Bootstrap 4
-	Font-awesome 5
### For data scrapping
-	Angle Sharp (scrap static pages)
-	Puppeteer Sharp (scrap the pages that use scripts to dynamically generate content)
