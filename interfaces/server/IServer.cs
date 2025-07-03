using Interfaces;

namespace Interfaces
{
  public interface IServer
  { 
    void UseRouter(string apiEndpoint, IRouter router);
    void Listen();
    void EnableCors();
    // void SendResponse();
  }
}