using System.Net;
using System.Collections.Generic;
using HTTP;

class Program
{
  static void Main(string[] args)
  {
    Server server = new Server(9000);

    server.setRoute("/", (req, res) => {
      var serverResponse = new {
        message = "Hello, This is a message from the server."
      };

      byte[] buffer = System.Text.Encoding.UTF8.GetBytes(
        System.Text.Json.JsonSerializer.Serialize(serverResponse)
      );

      res.ContentType = "application/json";
      res.ContentLength64 = buffer.Length;
      res.OutputStream.Write(buffer, 0, buffer.Length);
      res.OutputStream.Close();
    });

    server.setRoute("/item", (req, res) => {
      var itemInfo = new {
        name = "Laptop",
        model = "NITRO-5",
        processor = "i5-11400h"
      };

      var serverResponse = new Response(
        statusCode: 200,
        contentType: "application/json",
        body: System.Text.Json.JsonSerializer.Serialize(itemInfo)
      );

      server.sendResponse(serverResponse);
    });

    server.listen();
  }
}