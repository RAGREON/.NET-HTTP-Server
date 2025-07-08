using Interfaces;
using System.Net;

namespace Modules 
{
  public class Response: IResponse
  {
    public int StatusCode { get; set; }
    public string ContentType { get; set; }
    public string Body { get; set; }

    public Response(int statusCode, string contentType, string body)
    {
      this.StatusCode = statusCode;
      this.ContentType = contentType;
      this.Body = body;
    }

    static public void SendResponse(HttpListenerResponse res, IResponse response)
    {
      var buffer = System.Text.Encoding.UTF8.GetBytes(response.Body);

      res.ContentType = response.ContentType;
      res.OutputStream.Write(buffer, 0, buffer.Length);
      res.OutputStream.Close();
    }
  }

  public class HtmlResponse: IResponse
  {
    public int StatusCode { get; set; }
    public string ContentType { get; set; }
    public string Body { get; set; }

    public HtmlResponse(string html)
    {
      StatusCode = 200;
      ContentType = "text/html";
      Body = html;
    }
  }

  public class JsonResponse: IResponse
  {
    public int StatusCode { get; set; }
    public string ContentType { get; set; }
    public string Body { get; set; }

    public JsonResponse(object data)
    {
      StatusCode = 200;
      ContentType = "application/json";
      Body = System.Text.Json.JsonSerializer.Serialize(data);
    }
  }

  public class ErrorResponse: IResponse
  {
    public int StatusCode { get; set; }
    public string ContentType { get; set; }
    public string Body { get; set; }

    public ErrorResponse(int statusCode, string message)
    {
      StatusCode = statusCode;
      Body = message;
    }
  }
}