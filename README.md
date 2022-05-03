# .NET JWT authentication example

## !! WARNING !!

This is an example project and as such any sensitive information should **not** be 
reused "as is". In a real implementation any sensitive data (encryption key)
would be stored in a secure repository.

## Introduction

A simple .NET 6 project which illustrates how to generate a JWT token which can subsequently
be used against a protected API in the same project.

```
curl -X 'GET'
  'https://localhost:7238/Account/protected-endpoint'
  -H 'accept: text/plain'
  -H 'Authorization: Bearer XXX'
```

Further documentation can be found in the Swagger when launching the project in VS.

## Rate Limiting

The [Polly](https://github.com/App-vNext/Polly) package is used in this project as an
example of how one could implement a rudimentary rate limiting to avoid brute force
attacks. An HTTP code of 429 (too many requests) is returned if the redefined rate is
exceeded (60 calls per minute). 

One of the disadvantages of this type of protection is that a bad actor can 
easily block the service for all users. A more intelligent rate limiting mechanism would
be to rate limit by IP, but even this would not protect against a more sophisticated
bot attack.

### The solution

The solution to this problem is of course a 2nd authentication factor.

