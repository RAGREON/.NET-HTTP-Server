using System.Collections.Generic;
using System.Net;

namespace Interfaces 
{
  public delegate void Controller(HttpListenerRequest req, HttpListenerResponse res);

  public interface IGetRoute
  {
    void Get(string route, Controller handler);
  }

  public interface IPostRoute
  {
    void Post(string route, Controller handler);
  }

  public interface IPutRoute
  {
    void Put(string route, Controller handler);
  }

  public interface IDeleteRoute
  {
    void Delete(string route, Controller handler);
  }

  public interface IOptionRoute
  {
    void Option(string route, Controller handler);
  }

  public interface IRouter: IGetRoute, IPostRoute, IPutRoute
  {
    void HandleRoute(Method method, string route, HttpListenerRequest req, HttpListenerResponse res);
  }
}