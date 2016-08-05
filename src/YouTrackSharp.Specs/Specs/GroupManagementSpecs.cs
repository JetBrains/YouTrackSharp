#region License
// Distributed under the BSD License
// =================================
// 
// Copyright (c) 2010-2011, Hadi Hariri
// All rights reserved.
// 
// Redistribution and use in source and binary forms, with or without
// modification, are permitted provided that the following conditions are met:
//     * Redistributions of source code must retain the above copyright
//       notice, this list of conditions and the following disclaimer.
//     * Redistributions in binary form must reproduce the above copyright
//       notice, this list of conditions and the following disclaimer in the
//       documentation and/or other materials provided with the distribution.
//     * Neither the name of Hadi Hariri nor the
//       names of its contributors may be used to endorse or promote products
//       derived from this software without specific prior written permission.
// 
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND
// ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED
// WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
// DISCLAIMED. IN NO EVENT SHALL <COPYRIGHT HOLDER> BE LIABLE FOR ANY
// DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES
// (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES;
// LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND
// ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
// (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS
// SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
// =============================================================
#endregion
using System.Collections.Generic;
using System.Linq;
using Machine.Specifications;
using YouTrackSharp.Admin;
using YouTrackSharp.Specs.Helpers;

namespace YouTrackSharp.Specs.Specs
{
	[Subject(typeof(GroupManagement))]
	public class when_retrieving_a_list_of_groups_given_authenticated_connection : AuthenticatedYouTrackConnectionForGroupSpecs
	{
		Because of = () => { groups = groupManagement.GetAllGroups(); };

		It should_return_all_groups = () => { groups.ShouldNotBeEmpty(); };

		It should_contain_valid_group_data = () => { groups.First().Name.ShouldNotBeEmpty(); };

		It should_contain_valid_project_data = () => { groups.First().Name.ShouldNotBeEmpty(); };

		It should_contain_all_users_group = () => { groups.Any(group => group.Name == "All Users").ShouldBeTrue(); };

		static IEnumerable<Group> groups;
	}

}