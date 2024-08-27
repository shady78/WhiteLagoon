# .NET 8 MVC Clean Architecture Project 📚

## Project Overview

This project is an MVC application built using .NET 8, following the Clean Architecture principles. The application focuses on managing Villas, including CRUD operations, data validation, and UI enhancements.

## Key Topics Covered

1. **Clean Architecture**:
   - Structured the project with Clean Architecture to separate concerns and make the application more maintainable.

2. **Dependency Injection**:
   - Implemented DI for better management of dependencies, especially within the controller and repository layers.

3. **Entity Framework Core**:
   - Used EF Core for database operations, including creating models, DbContext, and handling migrations.
   - Seeded initial data into the database to get started quickly.

4. **MVC Architecture**:
   - Followed the MVC pattern to separate the application logic, UI, and data.
   - Implemented routing to manage how requests are handled within the application.

5. **CRUD Operations**:
   - Developed complete CRUD operations for managing Villas, including validation on both server and client sides.
   - Added UI elements for editing and deleting records.

6. **Repository Pattern**:
   - Used the Repository pattern to abstract data access logic, making it easier to test and manage.
   - Implemented a UnitOfWork pattern to handle multiple repositories efficiently.

7. **Image Handling**:
   - Integrated functionality for uploading, displaying, and managing images related to Villas.

8. **UI Enhancements**:
   - Applied Bootstrap for responsive and visually appealing UI.
   - Added notifications and validation messages for better user experience.

## How to Run the Project

To run the project locally, follow these steps:

1. **Clone the repository**:
   ```bash
   git clone https://github.com/yourusername/your-repository.git
- Navigate to the project directory:
    ```bash
    cd your-repository
- Restore the NuGet packages:
    ```bash
    dotnet restore
  

- Apply Entity Framework Migrations:

  Ensure the database connection string is correctly set in the appsettings.json file.
  Run the following command to apply migrations and create the database:
```bash
    dotnet ef database update
    dotnet run
