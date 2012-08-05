using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YouTrack.Web.Helpers
{
    public class HelperClasses
    {
        public static StringBuilder BuildNames(IDictionary<string, string> values)
        {

            IEnumerable<string> names = GetNames();
            var enumerable = names as List<string> ?? names.ToList();
            foreach (var name in enumerable)
                Console.WriteLine("Found " + name);
            var allNames = new StringBuilder();
            foreach (var name in enumerable)
                allNames.Append(name + " ");

            return allNames;
        }


        static IEnumerable<string> GetNames()
        {
           return new List<string>();
        }

        public async Task<int> GetLongRunningResult()
        {
            int result = await GetLongRunningAsync();
            return result;
        }



        Task<int> GetLongRunningAsync()
        {
            Task<int> abc = null;
            return abc;
        }
    }
}