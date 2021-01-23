# Notes

## Helpful commands

### Scaffold controller

```commandline
dotnet aspnet-codegenerator controller -name PaymentsController -async -api -m Payment -dc DataContext -outDir Controllers
```

### Generate Migration

NB: After creating models and context

```commandline
dotnet ef migrations add InitialCreate
```

### Create DB and Tables

NB: After creating migrations

```commandline
dotnet ef database update
```