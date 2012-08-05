using System.Collections.Generic;
using System.Linq;
using Machine.Specifications;
using YouTrackSharp.Infrastructure;
using YouTrackSharp.Issues;

namespace YouTrackSharp.Specs.Specs
{
    [Ignore("Low priority as this is not actually used in code")]
    [Subject(typeof(ISearchEngine))]
    public class when_matching_issues_given_text_and_list_of_issues
    {
        Establish context = () =>
        {
            dynamic issue1 = new Issue();
            issue1.Id = "ID1";
            issue1.Summary = "A general exception has been raised";

            dynamic issue2 = new Issue();
            issue2.Id = "ID2";
            issue2.Summary = "Method to invocate has failed while connecting";

            dynamic issue3 = new Issue();
            issue3.Id = "ID3";
            issue3.Summary = "Failure when connecting to service";

            dynamic issue4 = new Issue();
            issue4.Id = "ID4";
            issue4.Summary = "Failure when trying to ping a connection";

            items = new List<Issue>() { issue1, issue2, issue3, issue4};

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