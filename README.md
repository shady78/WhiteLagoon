# .NET 8 MVC Clean Architecture Project 📚

## Project Overview

This project is an MVC application built using .NET 8, following the Clean Architecture principles. White Lagoon Website is filled with advanced concepts where customers can view the villa rooms in resort and making bookings with their credit cards. Admin can then view the bookings, check in/checkout the customer, and view the summary on their dashboard while managing all the villa via CMS that we will build for admin users.

![1](https://github.com/user-attachments/assets/bda3d423-1bbb-4fa9-931f-26d93c21ea79)
![2](https://github.com/user-attachments/assets/0b2481ae-8c21-4df9-bca1-6b876db84d69)
![3](https://github.com/user-attachments/assets/66593108-6aa0-42f4-b126-0f84b3d5b2dc)
![4](https://github.com/user-attachments/assets/007cf542-4661-4a15-8a74-22d13777c989)
![5](https://github.com/user-attachments/assets/13b20097-0e1b-42a5-b6ed-cdf161299472)
![6](https://github.com/user-attachments/assets/90b21d2e-d91e-4d1b-ae66-ce7db983b580)
![7](https://github.com/user-attachments/assets/7c4c8592-1fb4-4afa-8871-bcb2eb2a9405)



## Key Topics Covered

1. **Identity Security in ASP.NET Core using MVC**
   
2. **Building Applications using ASP.NET Core with MVC**
   
3. **Repository Pattern**
   
4. **Clean Architecture**
   
5. **Integrating Identity Framework and Extending User Fields**
   
6. **Integrating Entity Framework with Code-First Migrations**

8. **Authentication and Authorization in ASP.NET Core**

9. **Accepting Payments using Stripe**

10. **Admin Dashboard**

11. **Charts in .NET Core (ApexChart)**

12. **Building Dynamic PDF, PPT, and Word Documents in .NET Core (Syncfusion)**

13. **Data Seeding and Deployment to MyWindowsHosting**

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
