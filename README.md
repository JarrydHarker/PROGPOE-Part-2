# Agri Energy Connect Prototype
## Description
Agri Energy Connect Prototype is a website prototype designed to showcase the functionality of the Agri Energy Connect platform. It includes an account system with two user roles: employees and farmers. These roles are stored in a database and have different permissions and capabilities within the platform.
The website also features an accompanying API that interacts with a Google Cloud Storage to load images hosted separately. Users can browse products in the store, filter them by category, date, and name, and perform CRUD operations on products. Employees have additional capabilities to perform CRUD operations on farmers, while farmers have their own dashboards where they can manage their products.

## Features
Account system with two user roles: employees and farmers
CRUD operations on products and farmers
Image loading from Google Cloud Storage via API
Filtering products by category, date, and name
Employee and farmer dashboards with role-specific functionalities

## Technologies Used
C# for backend development
ASP.NET Core for web development
Entity Framework Core for database management
Google Cloud Storage for image hosting
Azure App Service for website and database hosting

## Getting Started
To get a copy of the project up and running on your local machine, follow these steps:

Clone the repository from https://github.com/JarrydHarker/PROGPOE-Part-2
Set up the database according to the provided database schema.
Configure the API to connect to Google Cloud Storage and adjust file paths accordingly.
Build and run the project locally for development and testing purposes.
Usage
Once the project is set up locally, you can access the website by navigating to the provided URL. Use the account system to log in as an employee or farmer and explore the platform's functionalities.

## Deployment
To deploy the website to production, follow these steps:

Set up a hosting environment on Azure App Service.
Deploy the project files to the hosting environment.
Configure any necessary environment variables, such as API keys and database connection strings.
Test the deployed website to ensure all functionalities are working as expected.
