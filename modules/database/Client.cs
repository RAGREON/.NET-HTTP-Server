using System;
using Microsoft.Data.SqlClient;

namespace Database
{
  class Client
  {
    private string serverName;
    private string databaseName;
    private SqlConnection conn;

    public Client(string sName, string dbName)
    {
      serverName = sName;
      databaseName = dbName;     
    }

    public void Execute(string query)
    {
      if (conn == null || conn.State != System.Data.ConnectionState.Open)
      {
        Console.WriteLine("Connection is not open");
        return;
      }

      using SqlCommand cmd = new SqlCommand(query, conn);
      using SqlDataReader reader = cmd.ExecuteReader();

      while (reader.Read())
      {
        for (int i = 0; i < reader.FieldCount; i++)
        {
          Console.Write(reader[i] + "\t");
        }
        Console.WriteLine();
      }
    }

    public void Connect()
    {
      // connection string
      string connString = $"Server={serverName};Database={databaseName};Integrated Security=True;TrustServerCertificate=True;";

      conn = new SqlConnection(connString);

      try {
        conn.Open();
        Console.WriteLine("Connected to SQL server using windows authentication.");
        Console.WriteLine($"Server Name: {serverName}");
        Console.WriteLine($"Database Name: {databaseName}");
      }
      catch (Exception ex)
      {
        Console.WriteLine("Error connecting to datbase: " + ex.Message);
      }
    }
  }
}