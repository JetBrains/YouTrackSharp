using System.Collections.Generic;

namespace YouTrackSharp.Tests.Infrastructure
{
    public class Connections
    {
        public static string ServerUrl 
            => "https://ytsharp.myjetbrains.com/youtrack/";
        
        public static Connection UnauthorizedConnection =>
            new UsernamePasswordConnection(ServerUrl, "demogod", "demogod");
        
        public static Connection Demo1Password =>
            new UsernamePasswordConnection(ServerUrl, "demo1", "demo1");

        public static Connection Demo1Token => 
            new BearerTokenConnection(ServerUrl, "perm:ZGVtbzE=.WW91VHJhY2tTaGFycA==.AX3uf8RYk3y2bupWA1xyd9BhAHoAxc");
        
        public static Connection Demo2Password =>
            new UsernamePasswordConnection(ServerUrl, "demo2", "demo2");

        public static Connection Demo3Token => 
            new BearerTokenConnection(ServerUrl, "perm:ZGVtbzM=.WW91VHJhY2tTaGFycA==.L04RdcCnjyW2UPCVg1qyb6dQflpzFy");

        public static class TestData
        {
            public static readonly List<object[]> ValidConnections
                = new List<object[]>
                {
                    new object[] { Demo1Password },
                    new object[] { Demo1Token },
                    new object[] { Demo2Password }
                };
            
            public static readonly List<object[]> InvalidConnections
                = new List<object[]>
                {
                    new object[] { UnauthorizedConnection }
                };
        }
    }
} 