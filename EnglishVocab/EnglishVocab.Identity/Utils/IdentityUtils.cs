
using Microsoft.AspNetCore.Http;
using System.Net.Sockets;
using System.Net;

namespace EnglishVocab.Identity.Utils;
public class IdentityUtils
{
    public static string GenerateIPAddress(HttpContext httpContext)
    {
        return httpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
    }

    public static string GetIpAddress()
    {
        var host = Dns.GetHostEntry(Dns.GetHostName());
        foreach (var ip in host.AddressList)
        {
            if (ip.AddressFamily == AddressFamily.InterNetwork)
            {
                return ip.ToString();
            }
        }
        return string.Empty;
    }
}
