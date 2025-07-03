using System.Net;

namespace Interfaces 
{
  public interface IResponse
  {
    public int StatusCode { get; set; }
    public string ContentType { get; set; }
    public string Body { get; set; }

    public void SendResponse(HttpListenerResponse res)
    {
      var buffer = System.Text.Encoding.UTF8.GetBytes(Body);
      res.ContentType = ContentType;
      res.OutputStream.Write(buffer, 0, buffer.Length);
      res.OutputStream.Close();
    }
  }
}