using System;
using System.IO;
using System.Net;

namespace CustomSearch
{
    public static class CustomRequestResponse
    {
        public interface IHttpWebRequest
        {
            IHttpWebResponse GetResponse();
        }

        public interface IHttpWebResponse : IDisposable
        {
            Stream GetResponseStream();
        }


        public class WrapHttpWebRequest : IHttpWebRequest
        {
            private readonly HttpWebRequest _request;

            public WrapHttpWebRequest(HttpWebRequest request)
            {
                _request = request;
            }

            public virtual IHttpWebResponse GetResponse()
            {
                return new WrapHttpWebResponse((HttpWebResponse)_request.GetResponse());
            }
        }

        public class WrapHttpWebResponse : IHttpWebResponse
        {
            private WebResponse _response;

            public WrapHttpWebResponse(HttpWebResponse response)
            {
                _response = response;
            }

            public void Dispose()
            {
                Dispose(true);
                GC.SuppressFinalize(this);
            }

            private void Dispose(bool disposing)
            {
                if (disposing)
                {
                    if (_response != null)
                    {
                        _response.Dispose();
                        _response = null;
                    }
                }
            }

            public virtual Stream GetResponseStream()
            {
                return _response.GetResponseStream();
            }
        }
    }
}
