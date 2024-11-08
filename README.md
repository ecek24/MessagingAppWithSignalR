# Real-Time Messaging App
Features Real-Time Messaging: Real-time communication between users with SignalR. <br>
JWT Authentication: Secure user authentication with JSON Web Tokens. ASP.NET Core Identity: User management for registration, login, and authentication. EF Core (Code-First): Database structure is managed with Entity Framework Core's code-first approach. Onion Architecture: Separation of concerns and organized layers.

Technologies Used .NET 8 ASP.NET Core MVC Entity Framework Core (Code-First) ASP.NET Core Identity JWT Authentication SignalR for real-time messaging

*Update the appsettings.json with your database connection string if not using User Secrets. *Use User Secrets to store sensitive information like the connection string and JWT keys.

Usage User Registration: Register as a new user through the registration page. Login: Log in to receive a JWT, stored in cookies for secure authentication. Start Messaging: After login, navigate to the chat page, enter a recipient’s mail address, and start sending messages in real-time.
