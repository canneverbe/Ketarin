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
  using System.Collections;
  
  public class XmlRpcStruct : Hashtable
  {
    public override void Add(object key, object value)
    {
      if (!(key is string))
      {
        throw new ArgumentException("XmlRpcStruct key must be a string.");
      }
      if (XmlRpcServiceInfo.GetXmlRpcType(value.GetType()) 
          == XmlRpcType.tInvalid)
      {
        throw new ArgumentException(String.Format(
          "Type {0} cannot be mapped to an XML-RPC type", value.GetType()));
      }
      base.Add(key, value);
    }

    public override object this[object key]
    {
      get
      {
        return base[key];
      }
      set
      {
        if (!(key is string))
        {
          throw new ArgumentException("XmlRpcStruct key must be a string.");
        }
        if (XmlRpcServiceInfo.GetXmlRpcType(value.GetType())
            == XmlRpcType.tInvalid)
        {
          throw new ArgumentException(String.Format(
            "Type {0} cannot be mapped to an XML-RPC type", value.GetType()));
        }
        base[key] = value;
      }
    }

    public override bool Equals(Object obj)
    {
      if (obj.GetType() != typeof(XmlRpcStruct))
        return false;
      XmlRpcStruct xmlRpcStruct = (XmlRpcStruct)obj;
      if (this.Keys.Count != xmlRpcStruct.Count)
        return false;
      foreach (String key in this.Keys)
      {
        if (!xmlRpcStruct.ContainsKey(key))
          return false;
        if (!this[key].Equals(xmlRpcStruct[key]))
          return false;
      }
      return true;
    }

    public override int GetHashCode()
    {
      int hash = 0;
      foreach (object obj in Values)
      {
        hash ^= obj.GetHashCode();
      }
      return hash;
    }
  }
}