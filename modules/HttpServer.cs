using System.Net;
using System.Collections.Generic;

namespace HTTP
{
  class Response {
    public int statusCode { get; set; }
    public string contentType { get; set; }
    public string body { get; set; } 

    public Response(int statusCode, string contentType, string body) {
      this.statusCode = statusCode;
      this.contentType = contentType;
      this.body = body;
    }
  }

  enum Method {
    GET, POST, PUT, DELETE
  }

  class Router {
    private Dictionary<string, Action<HttpListenerRequest, HttpListenerResponse>>[] router;

    public Router()
    {
      router = new Dictionary<string, Action<HttpListenerRequest, HttpListenerResponse>>[4];
    }

    public void get(string apiRoute, Action<HttpListenerRequest, HttpListenerResponse> controllerFn)
    {
      router[Method.GET][apiRoute] = controllerFn;
    }

    public void post(string apiRoute, Action<HttpListenerRequest, HttpListenerResponse> controllerFn)
    {
      router[Method.POST][apiRoute] = controllerFn;
    }

    public void put(string apiRoute, Action<HttpListenerRequest, HttpListenerResponse> controllerFn)
    {
      router[Method.PUT][apiRoute] = controllerFn;
    }

    public void delete(string apiRoute, Action<HttpListenerRequest, HttpListenerResponse> controllerFn)
    {
      router[Method.DELETE][apiRoute] = controllerFn;
    }

    public void handleRoute(Method method, string apiRoute, HttpListenerRequest req, HttpListenerResponse res)
    {
      try
      {
        if (Enum.IsDefined(typeof(Method), method) && !IsNullOrWhiteSpace(apiRoute) && router[method].ContainsKey(apiRoute)) 
        {
          router[method][apiRoute](req, res);
        }
      } catch (Exception ex)
      {
        Console.WriteLine($"Error handling route: [METHOD: ${method.ToString()}] [ROUTE: ${apiRoute}] -> {ex.Message}");
      }
    }
  }

  class Server
  {
    private readonly int PORT;

    private HttpListener listener;

    private HttpListenerContext context;
    private HttpListenerRequest request;
    private HttpListenerResponse response;

    private Dictionary<string, Action<HttpListenerRequest, HttpListenerResponse>> router;
    

    public Server(int PORT)
    {
      this.PORT = PORT;
      router = new Dictionary<string, Action<HttpListenerRequest, HttpListenerResponse>>();

      try {
        listener = new HttpListener();
        listener.Prefixes.Add($"http://localhost:{PORT}/");
        listener.Start();

        Console.WriteLine($"Server is running at: http://localhost:{PORT}/");
      } catch (Exception ex)
      {
        Console.WriteLine($"Error initiating HTTP server at port {PORT}: {ex.Message}");
      }
    }

    void getRequest()
    {
      context = listener.GetContext();
      request = context.Request;
      response = context.Response;
    }

    public void sendResponse(Response responseObj) 
    {
      response.StatusCode = responseObj.statusCode;
      response.ContentType = responseObj.contentType;

      byte[] buffer = System.Text.Encoding.UTF8.GetBytes(responseObj.body);
      
      response.ContentLength64 = buffer.Length;
      response.OutputStream.Write(buffer, 0, buffer.Length);
      response.OutputStream.Close();
    }

    public void setRoute(string apiRoute, Action<HttpListenerRequest, HttpListenerResponse> controllerFn)
    {
      router[apiRoute] = controllerFn;
    }

    public void handleRoutes()
    {
      string route = request.Url.AbsolutePath;

      if (!string.IsNullOrWhiteSpace(route) && router.ContainsKey(route))
      {
        router[route](request, response);
      }
    }

    public void listen()
    {
      while (true)
      {
        getRequest();
        handleRoutes();

        Console.WriteLine($"received: {request.HttpMethod} | {request.Url}");
      }
    }
  }
}