using Interfaces;
using Modules;
using System.Collections.Generic;

class Program
{
  static void Main(string[] args)
  {
    Model model = new Model();

    Field usernameField = new Field(
        name: "USERNAME",
        type: "VARCHAR(50)",
        nullable: false,
        primaryKey: false,
        foreignKey: false
    );

    Field passwordField = new Field(
        name: "PASSWORD",
        type: "VARCHAR(50)",
        nullable: false,
        primaryKey: false,
        foreignKey: false
    );
    
    List<IField> fields = new List<IField>() {
      usernameField, passwordField
    };

    foreach (IField field in fields)
    {
      field.Properties();
    }

    Console.WriteLine("Database Client Test");
    string connectionString = "Server=DESKTOP-NE882SF\\MSSQLSERVER01;Database=Crextio;Trusted_Connection=True;Encrypt=True;TrustServerCertificate=True;";

    DatabaseClient dbClient = new DatabaseClient(connectionString);

    dbClient.Connect();
    dbClient.Disconnect();
  }
}