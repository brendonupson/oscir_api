# Login
Send the username and password to the server
```
curl -X POST "https://localhost:5001/Auth/Login" -H "accept: application/json" -H "Content-Type: application/json-patch+json" -d "{ \"username\": \"string\", \"password\": \"string\"}"
```
which returns a bearer token (Jwt)
```
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJuYW1laWQiOiI.......",
  "username": "admin",
  "firstname": "",
  "lastname": "Admin",
  "lastLogin": "2019-05-17T13:18:22.419522",
  "id": "05c8d091-acb3-4b75-b8e4-6c28af8c5116",
  "isAdmin": true
}
```

Take the token value and set http header for all subsequent requests:
```
Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJuYW1laWQiOiI.......
```

Any errors will result in a payload similar to:
```
{
  "message": "Invalid username or password"
}
```

# Find Owner OwnerId

# Find Class ClassId

# Insert or Update ConfigItems
