# 📌 Microservices Architecture with Gateway, Orchestrator, and Saga Pattern

<h4 align="left">📖 Overview
This project demonstrates example of implementing: </h4> 

<h3 align="left">🌟 Key Features </h3>  

&emsp;&emsp;🛡️ &nbsp;**Gateway API** with both HTTP & AMPQ communication  
&emsp;&emsp;📬 **Event-driven** microservices using RabbitMQ & Rebus  
&emsp;&emsp;🔄 **Saga Pattern** for distributed transactions  
&emsp;&emsp;⚡ **CQRS with MediatR** for clear separation of concerns (UserService)  
&emsp;&emsp;🧼 **Clean Architecture** principles (UserService)   

<h3 align="left">🛠️ Technologies Used</h3>    

&emsp;&emsp;**.NET 8** – Core framework for building microservices   
&emsp;&emsp;**Rebus** – Lightweight service bus for messaging and saga handler  
&emsp;&emsp;**RabbitMQ** – Message broker for event-driven communication  
&emsp;&emsp;**MediatR** – CQRS and in-process messaging  
&emsp;&emsp;**Entity Framework Core** – Database management  
&emsp;&emsp;**YARP** - reverse proxy for gateway  

<h3 align="left">📁 Solution Structure</h3>  
&emsp;&emsp;The project is divided into multiple microservices, each with a specific responsibility.  



<h3 align="left">1️⃣ Gateway.API </h3>

**🔹 Purpose**: Handle Orders and Users request  
**🔹 Communication**:  
&emsp;&ensp;Gateway redirects Users requests to -> UserService through HTTP protocol  
&emsp;&ensp;Orders requests go to -> Orchestrator through AMPQ protocol
**🔹 Responsibilities**:  
&emsp;&ensp;Request routing 

<h3 align="left">2️⃣ Orchestrator (Implements Saga Pattern) </h3>  

**🔹 Purpose**: Manages long-running transactions across multiple microservices   
**🔹 Communication**:  
&emsp;&ensp;MessageBus → Listens for CreateOrderCommand  
&emsp;&ensp;Sends events to OrderService, StockService, and PaymentService  
**🔹 Responsibilities**:  
&emsp;&ensp;Handles order creation workflow  
&emsp;&ensp;Ensures consistency across services using Saga  
&emsp;&ensp;Rolls back actions if any step fails  

<h3 align="left">3️⃣ UserService </h3>   

**🔹 Purpose**: Manages users CRUD operations  
**🔹 Communication**:  
&emsp;&ensp;HTTP protocol  
**🔹 Responsibilities**:  
&emsp;&ensp;Saves users  
&emsp;&ensp;Read users

<h3 align="left">4️⃣ OrderService </h3>   

**🔹 Purpose**: Manages order processing  
**🔹 Communication**:  
&emsp;&ensp;MessageBus → Listens for order creation events  
**🔹 Responsibilities**:  
&emsp;&ensp;Saves order details  
&emsp;&ensp;Publishes order confirmation event

<h3 align="left">5️⃣ StockService </h3>  

**🔹 Purpose**: Manages inventory stock  
**🔹 Communication**:  
&emsp;&ensp;MessageBus → Listens for stock reservation requests  
**🔹 Responsibilities**:  
&emsp;&ensp;Reserves items for an order  
&emsp;&ensp;Publishes stock reservation success/failure

<h3 align="left">6️⃣ PaymentService  </h3> 

**🔹 Purpose**: Handles payments    
**🔹 Communication**:  
&emsp;&ensp;MessageBus → Listens for payment processing requests  
**🔹 Responsibilities**:  
&emsp;&ensp;Processes transactions  
&emsp;&ensp;Publishes payment success/failure

<h3 align="left">🚀 Future Improvements  </h3>  

&emsp;&ensp;**1.** Implement Idempotency   
&emsp;&ensp;**2.** Implement Distributed Tracing (OpenTelemetry)  
&emsp;&ensp;**3.** Add Authentication & Authorization (JWT)  
