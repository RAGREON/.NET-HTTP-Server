using System.Net;
using Modules;
using Interfaces;

class Program
{
  static void Main(string[] args)
  {

    IRouter router = new Router();

    router.Get("/hello", (req, res) => {
      HtmlResponse htmlResponse = new HtmlResponse("<h1>Hello World</h1>");
      ((IResponse)htmlResponse).SendResponse(res);
    });

    Server server = new Server(9090);
    server.EnableCors();

    server.UseRouter("/api/", router);

    server.Listen();
  }
}