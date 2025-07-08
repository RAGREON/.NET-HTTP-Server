using System.Data;
using Microsoft.Data.SqlClient;

namespace Interfaces
{
  public interface IDatabaseClient
  {
    void Connect();
    void Disconnect();
    DataTable ExecuteQuery(string query);
    int ExecuteNonQuery(string query);
    object ExecuteScalar(string query);
    bool IsConnected();
  }
}