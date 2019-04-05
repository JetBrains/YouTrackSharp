using System.Collections.Generic;
using System.Net.Http;
using System.Security.Authentication;

namespace YouTrackSharp.Tests.Infrastructure
{
    public class Connections
    {
        public static string ServerUrl 
            => "https://ytsharp.myjetbrains.com/youtrack/";
        
        public static Connection UnauthorizedConnection =>
            new BearerTokenConnection(ServerUrl, "invalidtoken", ConfigureTestsHandler);

        public static Connection Demo1Token => 
            new BearerTokenConnection(ServerUrl, "perm:ZGVtbzE=.WW91VHJhY2tTaGFycA==.AX3uf8RYk3y2bupWA1xyd9BhAHoAxc", ConfigureTestsHandler);
        
        public static Connection Demo2Token =>
            new BearerTokenConnection(ServerUrl, "perm:ZGVtbzI=.WW91VHJhY2tTaGFycA==.GQEOl33LyTtmJvhWuz0Q629wbo8dk0", ConfigureTestsHandler);

        public static Connection Demo3Token => 
            new BearerTokenConnection(ServerUrl, "perm:ZGVtbzM=.WW91VHJhY2tTaGFycA==.L04RdcCnjyW2UPCVg1qyb6dQflpzFy", handler => ConfigureTestsHandler(handler));

        public static class TestData
        {
            public static readonly List<object[]> ValidConnections
                = new List<object[]>
                {
                    new object[] { Demo1Token },
                    new object[] { Demo2Token }
                };
            
            public static readonly List<object[]> InvalidConnections
                = new List<object[]>
                {
                    new object[] { UnauthorizedConnection }
                };
        }
        
        private static void ConfigureTestsHandler(HttpClientHandler handler)
        {
            handler.SslProtocols = SslProtocols.Tls12;
        }
    }
} 