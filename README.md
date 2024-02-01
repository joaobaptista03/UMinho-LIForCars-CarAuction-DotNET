# LIForCars Auction Platform

LIForCars is a comprehensive auction platform designed to facilitate online car auctions. It allows users to list cars for auction, place bids on vehicles, and manage auctions and bids with ease. Built with ASP.NET Core and Entity Framework, the platform offers a robust and scalable solution for online car auction businesses.

<img src="https://i.ibb.co/gmZqTW9/image.png" alt="LIForCars">

## Features

- **User Authentication and Authorization:** Secure login and registration functionality, with support for both regular users and administrators.
- **Car Auction Management:** Users can create, view and delete car auctions. Auction details include make, model, year, and other car characteristics.
- **Bidding System:** Registered users can place bids on active car auctions. The system supports bid validation and real-time bid updates.
- **Administrator Controls:** Administrators have additional capabilities, such as approving auctions and accessing detailed auction statistics.
- **Responsive UI:** A user-friendly interface that is accessible on both desktop and mobile devices.

## Technical Stack

- **ASP.NET Core Razor Pages:** For server-side page generation and handling web requests.
- **Entity Framework Core:** ORM for database operations, using SQL Server as the backend.
- **Serilog:** For comprehensive logging throughout the application.
- **Bootstrap and Custom CSS:** For styling and responsive design.

## Setup and Installation

1. **Prerequisites:**
    - .NET 6 SDK
    - SQL Server

2. **Clone the repository:**
    ```bash
    git clone https://github.com/joaobaptista03/UMinho-LIForCars-CarAuction-DotNET.git
    ```

3. **Configure the database connection string:**
    - Update `appsettings.json` with your SQL Server connection string.

4. **Apply Database Migrations:**
    - Navigate to the project directory and execute:
      ```bash
      dotnet ef database update
      ```

5. **Run the application:**
    - Open the solution in Visual Studio and press `F5`, or run the following command in the terminal:
      ```bash
      dotnet run
      ```

6. **Access the application:**
    - Navigate to `https://localhost:5001` in your web browser.
