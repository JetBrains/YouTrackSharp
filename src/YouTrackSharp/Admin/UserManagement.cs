#region License

// Distributed under the BSD License
//   
// YouTrackSharp Copyright (c) 2010-2012, Hadi Hariri and Contributors
// All rights reserved.
//   
//  Redistribution and use in source and binary forms, with or without
//  modification, are permitted provided that the following conditions are met:
//      * Redistributions of source code must retain the above copyright
//         notice, this list of conditions and the following disclaimer.
//      * Redistributions in binary form must reproduce the above copyright
//         notice, this list of conditions and the following disclaimer in the
//         documentation and/or other materials provided with the distribution.
//      * Neither the name of Hadi Hariri nor the
//         names of its contributors may be used to endorse or promote products
//         derived from this software without specific prior written permission.
//   
//   THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS
//   "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED
//   TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A 
//   PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL 
//   <COPYRIGHTHOLDER> BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL,
//   SPECIAL,EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT
//   LIMITED  TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE,
//   DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND  ON ANY
//   THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
//   (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF
//   THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
//   

#endregion

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Web;
using YouTrackSharp.Infrastructure;

namespace YouTrackSharp.Admin
{
	public class UserManagement
	{
		readonly IConnection _connection;

		public UserManagement(IConnection connection)
		{
			_connection = connection;
		}

		public IEnumerable<User> GetAllUsers()
		{
			return this.GetUsersFromPath("admin/user");
		}

		/// <summary>
		/// Extended options for getting users.
		/// </summary>
		/// <example>userManagement.GetAllUsers(group: "all users")</example>
		/// <example>userManagement.GetAllUsers("root")</example>
		/// <returns></returns>
		public IEnumerable<User> GetUsers(string query = null, string group = null, string role = null,
		                                  string project = null, string permission = null, bool? onlineOnly = null,
		                                  int? start = null)
		{
			string command = "admin/user";
			var queryString = HttpUtility.ParseQueryString("");

			if (query != null)      queryString["q"] = query;
			if (group != null)      queryString["group"] = group;
			if (role != null)       queryString["role"] = role;
			if (project != null)    queryString["project"] = project;
			if (permission != null) queryString["permission"] = permission;
			if (onlineOnly != null) queryString["onlineOnly"] = onlineOnly == true ? "true" : "false";
			if (start != null)      queryString["start"] = start.ToString();

			if (queryString.Count > 0)
			{
				command += "?" + queryString.ToString();
			}

			return this.GetUsersFromPath(command);
		}

		private IEnumerable<User> GetUsersFromPath(string path)
		{

			ICollection<User> users = new Collection<User>();
			IEnumerable<AllUsersItem> userItems = _connection.Get<IEnumerable<AllUsersItem>>(path);

			foreach (AllUsersItem userItem in userItems)
            {
				users.Add(GetUserByUserName(userItem.Login));
			}

			return users;
		}

		public User GetUserByUserName(string username)
		{
			var user = _connection.Get<User>(String.Format("user/bylogin/{0}", username));

			if (user != null)
			{
				user.Username = username;
				return user;
			}

			throw new InvalidRequestException(Language.Server_GetUserByUserName_User_does_not_exist);
		}

		public IEnumerable<Filter> GetFiltersByUsername(string username)
		{
			return _connection.Get<IEnumerable<Filter>>(String.Format("user/filters/{0}", username));
		}
	}
}