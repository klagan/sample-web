# Introduction
Add OpenAPI documentation to your API project

# Implementation

1. Add attributes to the controllers

```csharp
[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
[ApiVersion("2.0")]
```

2. Add the service (`ConfigureService` method)

```csharp
 services.AddMyOpenApiConfiguration(<xml documentation path>);
```

3. Inject provider into `Configure` method

```csharp
IApiVersionDescriptionProvider provider
```

4. Add to middleware pipeline (`Configure` method)

```csharp
app.UseMyOpenApiConfiguration(provider);
```

## Getting started

```url
http://localhost:5000/docs/index.html
```
