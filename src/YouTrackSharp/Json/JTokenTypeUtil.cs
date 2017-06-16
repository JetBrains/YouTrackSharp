using Newtonsoft.Json.Linq;

namespace YouTrackSharp.Json
{
    internal static class JTokenTypeUtil
    {   
        public static bool IsSimpleType(JTokenType tokenType)
        {
            switch (tokenType)
            {
                case JTokenType.Boolean:
                case JTokenType.Float:
                case JTokenType.Integer:
                case JTokenType.Guid:
                case JTokenType.String:
                case JTokenType.Uri:
                    return true;
                    
                default:
                    return false;
            }
        }
    }
}