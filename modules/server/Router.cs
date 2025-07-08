using Interfaces;
using System.Net;

namespace Modules 
{
  public class Router: IRouter
  {
    private readonly Dictionary<Method, Dictionary<string, Controller>> routes;

    public Router() {
      routes = new();

      foreach (Method method in Enum.GetValues(typeof(Method)))
      {
        routes[method] = new Dictionary<string, Controller>();
      }
    }

    public void Get(string route, Controller handler) => routes[Method.GET][route] = handler; 
    public void Post(string route, Controller handler) => routes[Method.POST][route] = handler; 
    public void Put(string route, Controller handler) => routes[Method.PUT][route] = handler; 
    public void Delete(string route, Controller handler) => routes[Method.DELETE][route] = handler; 

    public void HandleRoute(Method method, string route, HttpListenerRequest req, HttpListenerResponse res)
    {
      if (routes.TryGetValue(method, out var routeMap) && 
          routeMap.TryGetValue(route, out var handler))
      {
        handler(req, res);
        return;
      }

      res.StatusCode = 404;
      byte[] buffer = System.Text.Encoding.UTF8.GetBytes($"Route: {route} => not found");
      res.OutputStream.Write(buffer, 0, buffer.Length);
    }
  }
}