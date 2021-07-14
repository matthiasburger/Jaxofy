using System.Text;
using System.Web;
using Jaxofy.Services.TrackService;
using Microsoft.AspNetCore.WebUtilities;

namespace Jaxofy.Services.HttpEncoder
{
    public class HttpEncoder : IHttpEncoder
    {
        public string HtmlEncode(string s)
        {
            return HttpUtility.HtmlEncode(s);
        }

        public string UrlDecode(string s)
        {
            return HttpUtility.UrlDecode(s);
        }

        public string UrlEncode(string s)
        {
            return HttpUtility.UrlEncode(s);
        }

        public string UrlEncodeBase64(byte[] input)
        {
            return WebEncoders.Base64UrlEncode(input);
        }

        public string UrlEncodeBase64(string input)
        {
            return WebEncoders.Base64UrlEncode(Encoding.ASCII.GetBytes(input));
        }
    }
}