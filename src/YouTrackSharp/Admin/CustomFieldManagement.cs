using System;
using System.Collections.Generic;
using JsonFx.Json;
using YouTrackSharp.Infrastructure;

namespace YouTrackSharp.Admin
{
    public class CustomFieldManagement
    {
        private readonly IConnection _connection;

        public CustomFieldManagement(IConnection connection)
        {
            _connection = connection;
        }

        public IEnumerable<TargetRef> GetCustomFields(string project)
        {
            if (project == null)
            {
                throw new ArgumentNullException("project");
            }

            var fields = _connection.Get<IEnumerable<TargetRef>>(String.Format("admin/project/{0}/customfield", project));
            return fields;
        }

        public CustomField GetCustomField(TargetRef fieldRef)
        {
            if (fieldRef == null)
            {
                throw new ArgumentNullException("fieldRef");
            }


            Console.WriteLine(fieldRef.RestQuery);
            var customField = _connection.Get<CustomField>(fieldRef.RestQuery);
            return customField;
        }

        public IEnumerable<TargetRef> GetAllVersionBundles()
        {
            var bundles = _connection.Get<IEnumerable<TargetRef>>("admin/customfield/versionBundle");
            return bundles;
        }

        public VersionBundle GetVersionBundle(TargetRef bundleRef)
        {
            var query = Uri.EscapeUriString(bundleRef.RestQuery);
            Console.WriteLine(query);
            var bundles = _connection.Get<VersionBundle>(query);
            return bundles;
        }

        public void AddVersionToVersionBundle(TargetRef versionBundle, Version version)
        {
            var command = String.Format("{0}/{1}", versionBundle.RestQuery, version.Value);
            Console.WriteLine(command);
            _connection.Put(command, version);
        }
    }

    public class Param
    {
        public string Name { get; set; }
        public string Value { get; set; }
    }

    public class TargetRef
    {
        public string Name { get; set; }
        public string Url { get; set; }

        [JsonIgnore]
        public string RestQuery
        {
            get
            {
                var uri = new Uri(Url);
                const string restPath = "/rest/";
                var restIndex = uri.LocalPath.IndexOf(restPath);
                var restQuery = uri.LocalPath.Remove(0, restIndex + restPath.Length);
                return restQuery;
            }
        }
    }

    public class CustomField
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public string EmptyText { get; set; }
        public IEnumerable<Param> Param { get; set; }
    }

    public class VersionBundle
    {
        public string Name { get; set; }
        public IEnumerable<Version> Version { get; set; }
    }

    public class Version
    {
        private static readonly DateTime _epoch = new DateTime(1970, 1, 1);
        public string Value { get; set; }
        public Int64 ReleaseDate { get; set; }
        public bool Released { get; set; }
        public bool Archived { get; set; }

        [JsonIgnore]
        public DateTime ReleaseDateTime
        {
            get { return _epoch + TimeSpan.FromMilliseconds(ReleaseDate); }
            set { ReleaseDate = (Int64)(value - _epoch).TotalMilliseconds; }
        }
    }
}