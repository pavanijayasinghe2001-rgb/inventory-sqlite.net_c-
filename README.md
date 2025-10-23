# Inventory Management System

This is a desktop application for managing inventory, customers, and employees, built with C# and Windows Forms (.NET 8).

## Prerequisites

Before you begin, ensure you have the following installed on your system:

*   **[.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)**
*   **[Visual Studio 2022](https://visualstudio.microsoft.com/vs/)** with the **.NET desktop development** workload enabled.

## How to Install and Run

1.  **Clone the repository** (or download the source code).
    ```sh
    git clone <your-repository-url>
    ```
2.  **Navigate to the project directory**.
    ```sh
    cd InventoryManagementSystem
    ```
3.  **Open the project in Visual Studio**.
    *   You can do this by double-clicking the `InventoryManagementSystem.csproj` file.

4.  **Restore Dependencies**.
    *   Visual Studio should automatically restore the required NuGet packages when you open the project. If not, you can do it manually by right-clicking the solution in the Solution Explorer and selecting "Restore NuGet Packages".

5.  **Run the application**.
    *   Press `F5` or click the "Start" button in Visual Studio to build and run the application.

### Alternative: Running from the Command Line

You can also run the application directly from the .NET CLI:

```sh
# Restore dependencies
dotnet restore

# Run the application
dotnet run
```

## Database

The application uses a **SQLite** database to store all its data.

*   The database file is named `inventory.db` and is located in the root directory of the project.
*   No separate database server installation is required. The necessary libraries are included in the project's dependencies.
