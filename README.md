# .NET 8 MVC Clean Architecture Project 📚

## Project Overview

This project is an MVC application built using .NET 8, following the Clean Architecture principles. The application focuses on managing Villas, including CRUD operations, data validation, and UI enhancements.
![1](https://github.com/user-attachments/assets/bda3d423-1bbb-4fa9-931f-26d93c21ea79)
![2](https://github.com/user-attachments/assets/0b2481ae-8c21-4df9-bca1-6b876db84d69)
![3](https://github.com/user-attachments/assets/66593108-6aa0-42f4-b126-0f84b3d5b2dc)
![4](https://github.com/user-attachments/assets/007cf542-4661-4a15-8a74-22d13777c989)
![5](https://github.com/user-attachments/assets/13b20097-0e1b-42a5-b6ed-cdf161299472)
![6](https://github.com/user-attachments/assets/90b21d2e-d91e-4d1b-ae66-ce7db983b580)
![7](https://github.com/user-attachments/assets/7c4c8592-1fb4-4afa-8871-bcb2eb2a9405)



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
