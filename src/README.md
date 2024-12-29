# JsonChisel

A .NET middleware for filtering JSON responses in minimal APIs.

JsonChisel allows you to selectively return only the requested fields from your API responses by specifying them in the request header. This is inspired by the field selection mechanism in GraphQL.

## Key Features:

- Field Selection: Specify the desired fields to include in the response using a comma-separated list in the fields header (header name is configurable).
- Minimal API Compatibility: Seamlessly integrates with .NET minimal APIs.
- Easy to Use: Simple middleware registration and usage.
- Customizable: Configure the fields header name and other options.

## Installation

Install the JsonChisel NuGet package:

```Bash

Install-Package JsonChisel
Usage

Register the middleware in Program.cs:
Фрагмент кода

var app = builder.Build();

app.UseMiddleware<JsonChiselMiddleware>();
//or 
//app.UseJsonChisel();

// ... rest of your app configuration
```

Send requests with the fields header:

```curl
GET /api/data
Host: localhost:5000
fields: id,name,address
```

Example

Response without fields header:

```JSON

{
"id": 1,
"name": "John Doe",
"address": {
"street": "123 Main St",
"city": "Anytown"
},
"email": "john.doe@example.com"
}
```

Response with fields header:

```JSON

{
"id": 1,
"name": "John Doe",
"address": {
"street": "123 Main St",
"city": "Anytown"
}
}
```

Contributing

Contributions are welcome! Please submit pull requests to the official repository: [https://github.com/dejandjenic/JsonChisel]

License

This project is licensed under the MIT license.

Disclaimer:

This middleware is intended for demonstration and educational purposes. It's recommended to carefully evaluate its suitability and security implications for your specific use case.
