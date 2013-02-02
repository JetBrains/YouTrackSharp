﻿#region License

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
using YouTrackSharp.Infrastructure;

namespace YouTrackSharp.Projects
{
    public class ProjectManagement
    {
        readonly IConnection _connection;

        public ProjectManagement(IConnection connection)
        {
            try
            {
                _connection = connection;
            }
            catch (ConnectionException e)
            {
                Console.WriteLine(e);
            }
        }

        public IEnumerable<Project> GetProjects()
        {
            return _connection.Get<IEnumerable<Project>>("project/all");
        }


        public IEnumerable<ProjectPriority> GetPriorities()
        {
            return _connection.Get<IEnumerable<ProjectPriority>>("project/priorities");
        }

        public IEnumerable<ProjectState> GetStates()
        {
            return _connection.Get<IEnumerable<ProjectState>>("project/states");
        }

        public IEnumerable<ProjectIssueTypes> GetIssueTypes()
        {
            return _connection.Get<IEnumerable<ProjectIssueTypes>>("project/types");
        }

        public IEnumerable<ProjectResolutionType> GetResolutions()
        {
            return _connection.Get<IEnumerable<ProjectResolutionType>>("project/resolutions");
        }

        public Project GetProject(string projectName)
        {
            return _connection.Get<Project>(String.Format("admin/project/{0}", projectName));
        }

        public void AddSubsystem(string projectName, string subsystem)
        {            
            _connection.Put(String.Format("admin/project/{0}/subsystem/{1}", projectName, subsystem), null);
        }

        
    }
}