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
    GET, POST, PUT, DELETE, OPTIONS
  }

  class Router {
    private Dictionary<string, Action<HttpListenerRequest, HttpListenerResponse>>[] router;

    public Router()
    {
      int methodCount = Enum.GetNames(typeof(Method)).Length;
      router = new Dictionary<string, Action<HttpListenerRequest, HttpListenerResponse>>[methodCount];

      for (int i = 0; i < methodCount; i++)
      {
        router[i] = new Dictionary<string, Action<HttpListenerRequest, HttpListenerResponse>>();
      }
    }

    public void get(string apiRoute, Action<HttpListenerRequest, HttpListenerResponse> controllerFn)
    {
      router[(int) Method.GET][apiRoute] = controllerFn;
    }

    public void post(string apiRoute, Action<HttpListenerRequest, HttpListenerResponse> controllerFn)
    {
      router[(int) Method.POST][apiRoute] = controllerFn;
    }

    public void put(string apiRoute, Action<HttpListenerRequest, HttpListenerResponse> controllerFn)
    {
      router[(int) Method.PUT][apiRoute] = controllerFn;
    }

    public void delete(string apiRoute, Action<HttpListenerRequest, HttpListenerResponse> controllerFn)
    {
      router[(int) Method.DELETE][apiRoute] = controllerFn;
    }

    public void handleRoute(Method method, string apiRoute, HttpListenerRequest req, HttpListenerResponse res)
    {
      try
      {
        if (Enum.IsDefined(typeof(Method), method) && !string.IsNullOrWhiteSpace(apiRoute) && router[(int) method].ContainsKey(apiRoute)) 
        {
          router[(int) method][apiRoute](req, res);
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
    private bool corsEnabled = false;

    private HttpListener listener;

    private HttpListenerContext context;
    private HttpListenerRequest request;
    private HttpListenerResponse response;

    private Dictionary<string, Router> router;

    public Server(int PORT)
    {
      this.PORT = PORT;
      router = new Dictionary<string, Router>();

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
    }

    public void handleRoutes()
    {
      string path = request.Url.AbsolutePath;
      string[] segments = path.Split('/', StringSplitOptions.RemoveEmptyEntries);
      string apiEndpoint;
      string remainingPath = "/";

      if (segments.Length < 2) return;
      
      apiEndpoint = "/" + segments[0] + "/" + segments[1] + "/";

      if (segments.Length > 2)
      {
        remainingPath += string.Join("/", segments.Skip(2));
      }

      Method method = (Method) Enum.Parse(typeof(Method), request.HttpMethod);

      if (method == Method.OPTIONS)
      {
        response.StatusCode = 200;
        response.AddHeader("Access-Control-Allow-Origin", "*");
        response.AddHeader("Access-Control-Allow-Methods", "GET, POST, OPTIONS");
        response.AddHeader("Access-Control-Allow-Headers", "Content-Type");
        response.OutputStream.Close();
        return;
      }

      Console.WriteLine($"---\nMethod: {method.ToString()}\nAPI End-Point: {apiEndpoint}\n---");

      if (router.ContainsKey(apiEndpoint))
      {

        router[apiEndpoint].handleRoute(method, remainingPath, request, response);
      }
    }

    public void useRouter(string apiEndpoint, Router router)
    {
      if (apiEndpoint[apiEndpoint.Length - 1] != '/')
        apiEndpoint += "/";

      this.router[apiEndpoint] = router;
    }

    public void EnableCors()
    {
      corsEnabled = true;
    }

    public void listen()
    {
      while (true)
      {
        getRequest();

        if (corsEnabled)
        {
          response.AddHeader("Access-Control-Allow-Origin", "*");
          response.AddHeader("Access-Control-Allow-Methods", "GET, POST, OPTIONS");
          response.AddHeader("Access-Control-Allow-Headers", "Content-Type"); 
        }

        handleRoutes();
        response.OutputStream.Close();
      }
    }
  }
}