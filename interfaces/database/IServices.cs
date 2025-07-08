using Interfaces;
using Modules;

namespace Services
{
  public interface DatabaseServices
  {
    int Insert(IDatabaseClient client, IField field);
    // int Update(IDatabaseClient client, IRecord record);
  }

}