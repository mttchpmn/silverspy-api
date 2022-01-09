using Database;

namespace Test.Utilities;

public class TestDatabase
     {
         private readonly string DatabaseName = "silverspy_test";
         private readonly DatabaseHelper _databaseHelper;
         private string _connectionString;
 
         public TestDatabase(string connectionString)
         {
             _connectionString = connectionString;
             _databaseHelper = new DatabaseHelper(_connectionString);
         }
 
         public void Initialise()
         {
             _databaseHelper.CreateDatabase(DatabaseName);
             _connectionString += $"Database={DatabaseName};";
             _databaseHelper.MigrateDatabase(_connectionString);
         }
 
         public void Destroy()
         {
             _databaseHelper.DropDatabase(DatabaseName);
         }
     }