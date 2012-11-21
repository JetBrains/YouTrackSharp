using System;
using System.Collections.Generic;
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
            var bundles = _connection.Get<VersionBundle>(query);
            return bundles;
        }

        public void AddVersionToVersionBundle(TargetRef versionBundle, Version version)
        {
            var command = String.Format("{0}/{1}", versionBundle.RestQuery, version.Value);
            _connection.Put(command, version);
        }
    }
}