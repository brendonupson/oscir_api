# Basic Api Usage

Swagger is available at https://localhost:5001/swagger/index.html
This example assumes your code will interrogate a data source and upload the results as ConfigItems to OSCiR. The process is:

1. Login, retrieve Jwt token for all subsequent requests
2. Find Owner that ConfigItem will be tagged to (All ConfigItems require an Owner)
3. Find Class that ConfigItem will be based on (All ConfigItems require a Class, eg "Physical Server")
4. Try to find an existing ConfigItem
5. Insert or update ConfigItem


## Login
Send the username and password to the server
```
curl -X POST "https://localhost:5001/Auth/Login" -H "accept: application/json" -H "Content-Type: application/json-patch+json" -d "{ \"username\": \"your_username\", \"password\": \"your_password\"}"
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

## Find Owner Id
Every owner should have a unique OwnerCode. It is best to look up owners by their OwnerCode. The following example uses "P":
```
curl -X GET "https://localhost:5001/api/owner?ownerCodeEquals=P" -H "accept: application/json" -H "Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJuYW1laWQiOiI......."
```
The response is an array of owners. When using OwnerCode you should expect 1 object returned
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


## Find Class Id
The following example tries to find the "UCS Blade" Class
```
curl -X GET "https://localhost:5001/api/class?classNameEquals=UCS%20Blade" -H "accept: application/json" -H "Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJuYW1laWQiOiI......."
```
which returns an array of Classes:
```
[
  {
    "className": "UCS Blade",
    "comments": "",
    "category": "",
    "isInstantiable": true,
    "isPromiscuous": false,
    "allowAnyData": true,
    "properties": null,
    "extends": null,
    "sourceRelationships": null,
    "targetRelationships": null,
    "id": "b1b3becd-e93e-4ec1-97ad-9615ce55f096",
    "modifiedOn": "2019-05-12T10:20:49.878286",
    "modifiedBy": "admin",
    "createdOn": "2019-03-25T14:43:41.755392",
    "createdBy": "admin",
    "deletedOn": null,
    "deletedBy": null
  }
]
```
Store the id of the class object for subsequent api calls

## Find an existing ConfigItem
When syncing with an external system, each item being synced must have a globally unique reference. This is referred to as a "ConcreteReference". This ConcreteReference is defined externally, and can be a concatenation of multiple keys to make a globally unique reference. Any lookups should be against this ConcreteReference.

```
curl -X GET "https://localhost:5001/api/configitem?concreteReferenceEquals=ABCC1234&startRowIndex=0&resultPageSize=25" -H "accept: application/json" -H "Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJuYW1laWQiOiI......."
```
ConfigItem results returned in a paging object so that very large datasets can be iterated through without impacting server performance. Results accessed from the "data" array.
```
{
  "currentResultCount": 1,
  "startRowIndex": 0,
  "totalRecordCount": 1,
  "data": [
    {
      "name": "sys/chassis-1/blade-1",
      "comments": "",
      "concreteReference": "ABCC1234",
      "properties": {
        "association": "associated",
        "availability": "unavailable",
        "dn": "sys/chassis-1/blade-1",
        "mfgTime": "2015-05-09T00:00:00Z",
        "availableMemory": 393216,
        "totalMemory": 393216,
        "chassisId": 1,
        "memorySpeed": 1866,
        "numOfCpus": 2,
        "numOfCores": 20,
        "numOfCoresEnabled": 20,
        "numOfThreads": 40,
        "numOfAdaptors": 1,
        "numOfEthHostIfs": 6,
        "numOfFcHostIfs": 2,
        "usrLbl": "",
        "slotId": 1,
        "managingInst": "B",
        "operPower": "on",
        "uuid": "587c407a-a526-11e4-0000-000000000069",
        "serial": "ABCC1234",
        "vendor": "Cisco Systems Inc",
        "model": "UCSB-B200-M4",
        "cpuVendor": "Intel(R) Corporation",
        "cpuModel": "Intel(R) Xeon(R) CPU E5-2660 v3 @ 2.60GHz",
        "cpuSpeed": 2.6,
        "cpuStepping": 2,
        "fwBIOS": "B200M4.3.1.1a.0.121720151230",
        "fwBoot": "3.1(1e).36",
        "fwRunning": "3.1(1e)",
        "fwBMC": "10.0"
      },
      "classEntityId": "b1b3becd-e93e-4ec1-97ad-9615ce55f096",
      "ownerId": "8addaaf5-f880-4164-9a4e-29d914b4439f",
      "sourceRelationships": [
        {
          "sourceConfigItemEntityId": "1561dec1-3f63-45a5-a001-e2c921e15fdc",
          "relationshipDescription": "Is Installed in",
          "targetConfigItemEntityId": "2f780010-443b-4acd-91e8-10e6c270647d",
          "id": "870b500c-7447-4ff7-853d-e13895792cc1",
          "modifiedOn": "2019-05-16T18:49:37.48513",
          "modifiedBy": "admin",
          "createdOn": "2019-05-16T18:49:37.485129",
          "createdBy": "admin",
          "deletedOn": null,
          "deletedBy": null
        }
      ],
      "targetRelationships": [],
      "id": "1561dec1-3f63-45a5-a001-e2c921e15fdc",
      "modifiedOn": "2019-05-17T13:18:37.369493",
      "modifiedBy": "admin",
      "createdOn": "2019-05-02T18:01:56.574971",
      "createdBy": "admin",
      "deletedOn": null,
      "deletedBy": null
    }
  ]
}
```

## Insert ConfigItem
```
{
  "name": "Your new CI name",
  "comments": "",
  "concreteReference": "YT66521-00987",
  "properties": {
    "custom1": "abc",
    "custom2": 998,
    "custom3": true
  },
  "classEntityId": "b1b3becd-e93e-4ec1-97ad-9615ce55f096",
  "ownerId": "8addaaf5-f880-4164-9a4e-29d914b4439f",
}
```

```
curl -X POST "https://localhost:5001/api/configitem" -H "accept: application/json" -H "Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJuYW1laWQiOiI......." -H "Content-Type: application/json-patch+json" -d "{ \"name\": \"Your new CI name\", \"comments\": \"\", \"concreteReference\": \"YT66521-00987\", \"properties\": { \"custom1\": \"abc\", \"custom2\": 998, \"custom3\": true }, \"classEntityId\": \"b1b3becd-e93e-4ec1-97ad-9615ce55f096\", \"ownerId\": \"8addaaf5-f880-4164-9a4e-29d914b4439f\",}"
```

Response (201):
```
{
  "name": "Your new CI name",
  "comments": "",
  "concreteReference": "YT66521-00987",
  "properties": {
    "custom1": "abc",
    "custom2": 998,
    "custom3": true
  },
  "classEntityId": "b1b3becd-e93e-4ec1-97ad-9615ce55f096",
  "ownerId": "8addaaf5-f880-4164-9a4e-29d914b4439f",
  "sourceRelationships": null,
  "targetRelationships": null,
  "id": "3932ffbb-4fe5-438c-8a76-13d1f8de3332",
  "modifiedOn": "2019-05-23T10:05:23.322008+04:00",
  "modifiedBy": "admin",
  "createdOn": "2019-05-23T10:05:23.322006+04:00",
  "createdBy": "admin",
  "deletedOn": null,
  "deletedBy": null
}
```

## Update ConfigItem
There are two ways to update a ConfigItem
1. Read the entire object from OSCiR, update the object properties, then PUT the entire object back
2. Make a cutdown object with just the fields to update, the PATCH that back to OSCiR. This approach can be used on multiple ConfigItems in a single call, such as moving ConfigItems from one owner to another. This is the preferred update method, since the first option may inadvertently overwrite data from another user.


### Patch
```
{
  "configItemIds": [
    "3932ffbb-4fe5-438c-8a76-13d1f8de3332"
  ],
  "patchConfigItem": 
  {
    "name": "Patched CI",
    "properties": {
      "age": 37,
      "canDelete": true,
      "profileName": "ABC 123"
    }
  }
}
```
```
curl -X PATCH "https://localhost:5001/api/configitem" -H "accept: application/json" -H "Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJuYW1laWQiOiI......." -H "Content-Type: application/json-patch+json" -d "{ \"configItemIds\": [ \"3932ffbb-4fe5-438c-8a76-13d1f8de3332\" ], \"patchConfigItem\": {\"name\": \"Patched CI\",\"ownerId\": \"8addaaf5-f880-4164-9a4e-29d914b4439f\"}}"
```

