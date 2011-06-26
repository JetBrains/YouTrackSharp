using Machine.Specifications;
using YouTrackSharp.Infrastructure;

namespace YouTrackSharp.Specs.Bugs
{
    [Subject("DefaultUrlConstructor")]
    public class when_providing_a_path
    {
        Establish context = () =>
        {
            uriConstructor = new DefaultUriConstructor("http", "myinstance.myjetbrains.com", 80, "youtrack/somesubpath");
        };

        Because of = () =>
        {
            url = uriConstructor.ConstructBaseUri("login");
        };

        It should_create_correct_url = () =>
        {
            url.ShouldEqual("http://myinstance.myjetbrains.com:80/youtrack/somesubpath/rest/login");
        };
        static DefaultUriConstructor uriConstructor;
        static string url;
    }

    [Subject("DefaultUrlConstructor")]
    public class when_not_providing_a_path
    {
        Establish context = () =>
        {
            uriConstructor = new DefaultUriConstructor("http", "myinstance.myjetbrains.com", 80, null);
        };

        Because of = () =>
        {
            url = uriConstructor.ConstructBaseUri("login");
        };

        It should_create_correct_url = () =>
        {
            url.ShouldEqual("http://myinstance.myjetbrains.com:80/rest/login");
        };

        static DefaultUriConstructor uriConstructor;
        static string url;
    } 

}