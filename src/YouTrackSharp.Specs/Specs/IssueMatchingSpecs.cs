using System.Collections.Generic;
using System.Linq;
using Machine.Specifications;
using YouTrackSharp.Infrastructure;
using YouTrackSharp.Issues;

namespace YouTrackSharp.Specs.Specs
{
    [Subject(typeof(ISearchEngine))]
    public class when_matching_issues_given_text_and_list_of_issues
    {
        Establish context = () =>
        {
            items = new List<Issue> {
                new Issue {Id= "ID1", Summary = "A general exception has been raised"}, 
                new Issue {Id= "ID2", Summary = "Method to invocate has failed while connecting"}, 
                new Issue {Id= "ID3", Summary = "Failure when connecting to service"}, 
                new Issue {Id = "ID4", Summary = "Failure when trying to ping a connection"}};

            luceneSearchEngine = new LuceneSearchEngine();

        };

        Because of = () =>
        {
            issues = luceneSearchEngine.FindMatchingKeys("\"connecting to service\"", items, new [] { "Id", "Summary", "Description" },"Id");
        };

        It should_return_correct_number_of_issues = () =>
        {
            issues.Count().ShouldEqual(1);

        };
        It should_find_all_matching_issues = () =>
        {
            issues.First().ShouldEqual("ID3");
            
          
        };

        static LuceneSearchEngine luceneSearchEngine;
        static IEnumerable<string> issues;
        static IEnumerable<Issue> items;
    }

 }