# PlantCare Api NetCore

This Backend was implemented under the latest version of Net Core technology (8.0), using Entity Framework for data storage and management.

Due to time constraints, authentication was not implemented for this test. This development includes CRUD operations in which the data of a plant is managed, since for the example it is not being stored in any database, temporary storage was used, which is persistent while this application is running.

To start the application, the following steps must be taken:

1. Download the content of this repository.
2. Check the Net Core version on your local computer, which must be ".NET 8.0". If you do not have this version, update it before opening the solution.
3. Open the PlantCareBackEnd.sln solution, which is located inside the "PlantCareBackEnd" folder.
4. If you are asked to install or update dependency packages, do so from the Nugget package manager.
5. Compile and run.

When you run the application, it will run in your browser at the following address "https://localhost:7066/", by default the application will show the page at the address "https://localhost:7066/swagger/index.html", on this page you can view, document and test the implemented APIs without the need for a client.

Keep the application running during testing with the frontend application.
