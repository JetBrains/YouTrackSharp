using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YouTrackSharp.Infrastructure;

namespace YouTrackSharp.Admin
{
	public class GroupManagement
	{
		readonly IConnection _connection;
		public GroupManagement(IConnection connection)
		{
			_connection = connection;
		}

		public IEnumerable<Group> GetAllGroups()
		{
			IEnumerable<Group> groups = _connection.Get<IEnumerable<Group>>("admin/group");

			return groups;
		}
	}
}
