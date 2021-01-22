# SilverSpy Architecture

## Frontend

## API

Technology: C# / Dotnet

### Functionality

#### Users

- Create User
- Delete User
- Edit User
- Get User by ID

#### Transactions

- Create Transaction
- Import Transactions
- Get Transactions for date
- Get Transactions for date range
- Get Transactions by type
- Edit Transaction
- Delete Transaction

## Database

Technology: PostGreSQL

### Tables

#### User Table

- User ID (Primary Key)
- First Name
- Last Name
- Email

#### Transactions Table

Columns from importing from CSV:

- Transaction ID (Primary Key)
- User ID (Foreign Key)
- Date Processed
- Transaction Date
- Transaction Type
- Reference
- Description
- Amount

Columns for User to modify:

- Code (Groceries, Fuel, Food etc...)
- Type (Fixed / Variable / Periodic etc...)
- Nature (elective / expense...)

#### Bank Config Table

This is optional, as the data could be denormalised into the user record.

- Config ID (Primary Key)
- User ID (Foreign Key)
- Bank Type (ASB, BNZ etc...)
...