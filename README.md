# EFurni - Example of Scalable Monolith

# Summary
Example of furniture themed e-commerce backend / frontend written with Asp Net Core  and Blazor Web Assembly

# Motivation
Creating an example of three tier monolith within e-commerce concept that implements essential features of it's business such as order, checkout, basket, etc 
project's goals are being example of following topics

* Scalable monolith
* Proper implementation of Clean Architecture
* Application of S.O.L.I.D principles
* 3 Tier UI, Backend, Database approach
* N Layer sevice based approach
* Basics of distributed caching
* RESTFull Api Design


### Current Tech stack
* ASP Net Core
* Blazor Web Assembly
* Razor Pages
* Entity Framework Core
* Docker
* Kubernetes
* Redis
* Postgresql
* Azure Cloud

# Guide-level explanation
The Current structure of backend have following layers Service, Data access, Presentation layer 

* **Presentation** layer, that will capture all requests from users via rest over http then when user made request to endpoint it sanitizes and validates user's input before the call service layer
* **Service** layer, is where our business logic lives also the facade for our domain entities
* **Data Access** layer, is where the all the persistence operations are handling  

# Issues
* Dirty entities created by EF Core such as all the business entities scaffolded by EF Core and it causes to business entity's ruled by 
EF Core's conventions which is preventable with manuel mapping but I think this kills the concept of EF Core
  
* Authentication method is fairly simple and monolithic it uses token based centralized authentication so there is no third party support in it

* Doesn't met GDPR compliance 

* Lack of unit and integration test

# How to boot up

## Linux

Just run the ./bootup.sh

## Windows

### Prerequisites
* Redis
* Postgresql (preferably version 11 or higher)
* PostGIS
* .NET Core 5

### Post installation
After installing dependencies there will not much work to do just edit connection strings and users as your preferance and ready to go
further. 

