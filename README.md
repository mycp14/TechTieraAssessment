# TechTieraAssessment

I created different project for the API for the getting transaction list and asp.net core mvc razor for the uploading file (csv and xml).

I also added the sample file both csv and xml located in WebApp sample file folder.

Change SQL Credentials:
1. Open appsettings.json.
2. Look for the "connectionString" then change you're sql credentials.

Step-by-Step EF Core Code first migration
1. In the Visual Studio header, under tools look the nuget package manager then click the "Package Manager Console".
2. On the Package Manager Console.
	a. Kindly see the "Default project" and you must select the "WebApp.Data".
	b. Type the following script for generating and updating db context.
		1. add-migration updatedDB
		2. update-database
	c. Finally, kindly check your sql and look if there's an created "ExerciseDB"
	d. Done
