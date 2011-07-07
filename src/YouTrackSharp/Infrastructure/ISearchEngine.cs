using System.Collections;
using System.Collections.Generic;

namespace YouTrackSharp.Infrastructure
{
    public interface ISearchEngine
    {
        IEnumerable<string> FindMatchingKeys(string textToFind, IEnumerable items, string[] searchFields, string keyField);
    }
}