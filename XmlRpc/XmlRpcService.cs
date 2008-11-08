/* 
XML-RPC.NET library
Copyright (c) 2001-2006, Charles Cook <charlescook@cookcomputing.com>

Permission is hereby granted, free of charge, to any person 
obtaining a copy of this software and associated documentation 
files (the "Software"), to deal in the Software without restriction, 
including without limitation the rights to use, copy, modify, merge, 
publish, distribute, sublicense, and/or sell copies of the Software, 
and to permit persons to whom the Software is furnished to do so, 
subject to the following conditions:

The above copyright notice and this permission notice shall be 
included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, 
EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES 
OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND 
NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT 
HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, 
WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, 
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
DEALINGS IN THE SOFTWARE.
*/

namespace CookComputing.XmlRpc
{
  using System;
  using System.Web;
  using System.Web.SessionState;

  public class XmlRpcService : XmlRpcHttpServerProtocol, 
                                IHttpHandler, IRequiresSessionState 
  {
    // note: IRequiresSessionState specifies that the target HTTP handler 
    // interface has read and write access to session-state values. 
    // This is a marker interface only and has no methods.

    protected HttpContext Context { get { return context; } }

    void IHttpHandler.ProcessRequest(HttpContext RequestContext) 
    {
      try
      {
        // store context for access from application code in derived classes
        context = RequestContext;
        // XmlRpc classes delegate to the corresponding Context classes
        XmlRpcHttpRequest httpReq = new XmlRpcHttpRequest(context.Request);
        XmlRpcHttpResponse httpResp = new XmlRpcHttpResponse(context.Response);
        HandleHttpRequest(httpReq, httpResp);
      }
      catch (Exception ex)
      {
        RequestContext.Response.StatusCode = 500;  // "Internal server error"
        RequestContext.Response.StatusDescription = ex.Message;
      }
    }

    bool IHttpHandler.IsReusable
    {
      get { return true; }
    } 

    private HttpContext context;
  }
}
