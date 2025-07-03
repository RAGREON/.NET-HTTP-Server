using System;
using System.Net;
using Interfaces;
using System.Linq;

namespace Modules
{
  public class Server: IServer
  {
    private readonly HttpListener listener = null;
    private HttpListenerContext context = null;
    private HttpListenerRequest request = null;
    private HttpListenerResponse response = null;

    private readonly int PORT;
    private readonly Dictionary<string, IRouter> routers;
    private bool corsEnabled = false;

    private void GetRequest()
    {
      context = listener.GetContext();
      request = context.Request;
      response = context.Response;
    }

    public Server(int port)
    {
      PORT = port;
      routers = new Dictionary<string, IRouter>();

      try {
        listener = new HttpListener();
        listener.Prefixes.Add($"http://localhost:{PORT}/");
        listener.Start();
        Console.WriteLine($"Server is running at: http://localhost:{PORT}");
      } 
      catch (Exception ex)
      {
        Console.WriteLine("Error initiating server: ", ex.Message);
      }
    }

    public void EnableCors()
    {
      corsEnabled = true;
    }

    public void UseRouter(string apiEndPoint, IRouter router)
    {
      if (!apiEndPoint.EndsWith("/"))
        apiEndPoint += "/";
      routers[apiEndPoint] = router;
    }

    public void Listen()
    {
      while (true)
      {
        GetRequest();

        if (corsEnabled)
        {
          response.AddHeader("Access-Control-Allow-Origin", "*");
          response.AddHeader("Access-Control-Allow-Methods", "GET, POST, PUT, DELETE, OPTIONS");
          response.AddHeader("Access-Control-Allow-Headers", "Content-Type");
        }

        if (request.HttpMethod == "OPTIONS")
        {
          response.StatusCode = 200;
          response.Close();
          continue;
        }

        var method = (Method)Enum.Parse(typeof(Method), request.HttpMethod);
        var path = request.Url.AbsolutePath;

        bool matched = false;

        foreach(var (basePath, router) in routers.OrderByDescending(r => r.Key.Length))
        {
          if (path.StartsWith(basePath))
          {
            var relativePath = path.Substring(basePath.Length - 1);
            if (!relativePath.StartsWith("/"))  
              relativePath = "/" + relativePath;

            router.HandleRoute(method, relativePath, request, response);
            matched = true;
            break;
          }
        }
      }
    }
  }
}