# ECommerceWebMicroservices
Description: 

Microservice 1: User Authentication Microservice
Domain: User management and authentication
Endpoints: Register, login, update profile, change password

Microservice 2: Product Catalog Microservice
Domain: E-commerce product catalog
Endpoints: Retrieve product information, add to cart, manage inventory

Both microservices are containerized using Docker. API gateway is using an Ocelot framework, this will  help to route requests to the appropriate microservices. For asynchronous communication, once a user is registered it will have a notification that the user is successfully registered. This notification is using RabbitMQ.
