# .NET Jwt authentication example

A simple .NET 6 project which illustrates how to generate a JWT token which can subsequently
be used against a protected API in the same project.

```
curl -X 'GET'
  'https://localhost:7238/Account/protected-endpoint'
  -H 'accept: text/plain'
  -H 'Authorization: Bearer XXX'
```

Further documentation can be found in the Swagger when launching the project in VS.

## !! WARNING !!

This is an example project and as such any sensitive information should not be 
reused "as is". In a real implementation any sensitive data (encryption key)
would be stored in a secure repository.

