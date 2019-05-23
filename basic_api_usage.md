# Basic Api Usage

This example assumes your code will interrogate a data source and upload the results as ConfigItems to OSCiR. The process is:

1. Login, retrieve Jwt token for all subsequent requests
2. Find Owner that ConfigItem will be tagged to (All ConfigItems require an Owner)
3. Find Class that ConfigItem will be based on (All ConfigItems require a Class, eg "Physical Server")
4. Try to find an existing ConfigItem
5. Insert or update ConfigItem


## Login
Send the username and password to the server
```
curl -X POST "https://localhost:5001/Auth/Login" -H "accept: application/json" -H "Content-Type: application/json-patch+json" -d "{ \"username\": \"your_username\", \"your_password\": \"string\"}"
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

## Find Owner OwnerId
Every owner should have a unique OwnerCode. It is best to look up owners by their OwnerCode. The following example uses "P":
```
curl -X GET "https://localhost:5001/api/owner?ownerCodeEquals=P" -H "accept: application/json" -H "Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJuYW1laWQiOiI......."
```
The response is an array of owners. When using OwnerCode you shoudl expect 1 object returned
```
[
  {
    "ownerName": "PLATFORM",
    "ownerCode": "P",
    "alternateName1": null,
    "category": "Internal",
    "comments": "",
    "status": null,
    "id": "8addaaf5-f880-4164-9a4e-29d914b4439f",
    "modifiedOn": "2019-04-05T12:55:46.509711",
    "modifiedBy": "admin",
    "createdOn": "2019-01-12T14:42:04.182719",
    "createdBy": "Anonymous",
    "deletedOn": null,
    "deletedBy": null
  }
]
```
Store the id of the owner object for subsequent api calls



## Find Class ClassId

## Find an existing ConfigItem

## Insert or Update ConfigItems
