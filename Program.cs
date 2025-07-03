using System.Net;
using Modules;
using Interfaces;

class Program
{
  static void Main(string[] args)
  {

    IRouter router = new Router();
    IRouter serverRouter = new Router();

    router.Get("/hello", (req, res) => {
      Response.SendResponse(res, new HtmlResponse(
        "<h1>Testing Response</h1>"
      ));
    });

    serverRouter.Get("/", (req, res) => {
      Response.SendResponse(res, new HtmlResponse(
        "<h1>You are in the home page at the moment.</h1>"
      ));
    });

    Server server = new Server(9090);
    
    server.EnableCors();

    server.UseRouter("/api/", router);
    server.UseRouter("/", serverRouter);

    server.Listen();
  }
}