## Postman requests used for testing

### Create Customer
http://localhost:7071/api/CreateCustomer

body:   
{   
    "name": "{{$randomFullName}}",   
    "address": "{{$randomStreetAddress}}",   
    "iban": "{{$randomBankAccountIban}}"   
}   

### Get Customer
http://localhost:7071/api/GetCustomers

### Get Single Customer
http://localhost:7071/api/GetSingleCustomer?iban=GE42TT0091003095036019

### Create Transaction
http://localhost:7072/api/CreateTransaction

body:   
{   
    "amount": "{{$randomInt}}",   
    "senderIban": "{{$randomBankAccountIban}}",   
    "receiverIban": "{{$randomBankAccountIban}}"   
}

### Get Transaction For Customer
http://localhost:7072/api/GetTransactionForCustomer?iban=GE42TT0091003095036019

### Get Report For IBAN
http://localhost:7073/api/GetReport

body:   
{   
    "iban": "GE42TT0091003095036019"   
}   