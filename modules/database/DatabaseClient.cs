using System;
using System.Data;
using Microsoft.Data.SqlClient;
using Interfaces;

namespace Modules 
{
  class DatabaseClient: IDatabaseClient
  {
    private SqlConnection Connection;

    private bool Connected = false;
    private string ConnectionString;

    public bool IsConnected() => Connection?.State == ConnectionState.Open;

    public DatabaseClient(string connectionString)
    {
      ConnectionString = connectionString;
    }

    public void Connect()
    {
      Connection = new SqlConnection(ConnectionString); 
      Connection.Open();
    }

    public void Disconnect()
    {
      if (IsConnected())      
        Connection.Close();
    }

    public DataTable ExecuteQuery(string query)
    {
      using var command = new SqlCommand(query, Connection);
      using var adapter = new SqlDataAdapter(command);

      var table = new DataTable();
      adapter.Fill(table);

      return table;
    }

    public int ExecuteNonQuery(string query)
    {
      using var command = new SqlCommand(query, Connection);
      return command.ExecuteNonQuery();
    }

    public object ExecuteScalar(string query)
    {
      using var command = new SqlCommand(query, Connection);
      return command.ExecuteScalar();
    }
  }
}