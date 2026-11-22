BookStoresBlazor

BookStoresBlazor is an online bookstore web application developed for the SEN5001 – Mobile and Web Technologies assessment.

The system provides customers with an easy way to browse books, 
search and filter the catalogue, manage their shopping cart, 
place orders, and track order status. Store Staff can process customer orders and manage inventory,
while Administrators can manage users, books, authors, publishers, and sales.

Technologies Used


•	C#

•	.NET 8

•	ASP.NET Core Blazor Server

•	ASP.NET Core Web API

•	Entity Framework Core

•	SQLite

•	Bootstrap 5

•	Swagger

•	BCrypt.Net-Next

•	Visual Studio


Main Features



•	Customer registration and login

•	Book browsing, searching and filtering

•	Shopping cart

•	Wishlist

•	Checkout and order placement

•	Order tracking

•	Inventory and stock management

•	User and role management

•	Book, Author and Publisher CRUD operations

•	Sales reporting

•	REST API with Swagger

•	Responsive mobile-friendly interface


User Roles


Customer


•	Browse and search books

•	Add books to cart and wishlist

•	Checkout and place orders

•	Track orders

Store Staff

•	Process customer orders

•	Update order status

•	Manage inventory and stock


Administrator

•	Manage users and roles

•	Manage books, authors and publishers

•	View sales information

Folder Structure

BookStoresBlazor/

│
├── Controllers/     # REST API Controllers

├── Data/            # Database and Models

├── Migrations/      # Entity Framework Core migrations

├── Pages/           # Blazor pages

├── Services/        # Business logic and services

├── Shared/          # Shared UI components

├── wwwroot/         # CSS, images and static files

├── App.razor

├── Program.cs

└── BookStoresBlazor.csproj

Database

The application uses SQLite as the development database and Entity Framework Core for database access and management.

How to Run


1.	Open the project in Visual Studio.
2.	Restore the required NuGet packages.
3.	Build the solution.
4.	Run the application using F5.
5.	Access the Swagger API using /swagger.

   
Future Improvements

•	Native or hybrid mobile application

•	Real online payment gateway

•	Automated unit and integration testing

•	Book cover images

•	Push notifications

•	Offline mobile support

•	Cloud database deployment

Project Purpose

This project was developed as an academic project for the SEN5001 – Mobile and Web Technologies module and demonstrates web application development, database integration, REST API development, role-based access control, validation, testing, and responsive design.

