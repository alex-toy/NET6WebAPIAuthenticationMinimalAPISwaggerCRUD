# NET 6 Web API Authentication | Minimal API & Swagger (CRUD)

In this project we’ll create a .NET 6 Web API (Minimal API) that will enforce JWT Authentication and Authorization. Furthermore, we’ll add Swagger support and configure Bearer Authentication for it, so that all of our endpoints can be testable from Swagger UI using a bearer token.

We will study the following points :
- How to create a Minimal API project
- Create and use Data Transfer Objects
- Create Services that expose data
- Understand the basics Minimal API in .NET 6
- Setup a CRUD .NET 6 Web API (Minimal API) that will expose services methods
- Configure the API Endpoints for Authentication and Authorization with JWT Bearer
- Setup Swagger support and configure bearer authentication with Swagger
- Understand the basics of Swagger Documentation in .NET 6

### Packages to install
```
Microsoft.AspNetCore.Authentication.JwtBearer
Microsoft.IdentityModel.Tokens
Swashbuckle.AspNetCore
System.IdentityModel.Tokens.Jwt
```

### Use the API

- login with wrong password
<img src="/pictures/api.png" title="login with wrong password"  width="900">

- login with correct password
<img src="/pictures/api2.png" title="login with wrong password"  width="900">

- use protected route without credentials
<img src="/pictures/api3.png" title="login with wrong password"  width="900">


- use protected route with credentials
<img src="/pictures/api4.png" title="login with wrong password"  width="900">



## Build a role-based user authentication system using ASP.NET Core Web API and Angular

### Install packages

- install packages in *CoffeeShopAPI*
```
Microsoft.EntityFrameworkCore.SqlServer -Version 6.0.0
Microsoft.EntityFrameworkCore.Tools Version="6.0.1"
BCrypt.Net-Next Version="4.0.2"
Microsoft.AspNetCore.Authentication.JwtBearer Version="6.0.0"
Microsoft.AspNetCore.Mvc.NewtonsoftJson Version="6.0.0"
Microsoft.EntityFrameworkCore.Design Version="6.0.0"
Microsoft.EntityFrameworkCore.InMemory Version="6.0.0"
Microsoft.VisualStudio.Web.CodeGeneration.Design Version="6.0.0"
Microsoft.AspNetCore.Mvc.Cors Version="2.2.0"
Newtonsoft.Json Version="13.0.1"
PayPal.SDK.NETCore Version="1.9.1.2"
RestSharp Version="106.11.8-alpha.0.14"
Select.HtmlToPdf.NetCore Version="21.1.0"
Stripe.net Version="39.31.0"
```

- Migrations
```
Add-Migration InitialCreate
Update-Database
```

### Use the API

- signup
<img src="/pictures/signup.png" title="signup"  width="900">
<img src="/pictures/signup2.png" title="signup"  width="900">

- send-recovery-link
<img src="/pictures/send_recovery_link.png" title="send recovery link"  width="900">




