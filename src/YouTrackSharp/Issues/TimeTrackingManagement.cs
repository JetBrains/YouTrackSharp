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

using EasyHttp.Http;
using EasyHttp.Infrastructure;
using System;
using System.Collections.Generic;
using System.Xml;
using YouTrackSharp.Infrastructure;

namespace YouTrackSharp.Issues
{
    /// <summary>
    /// Interfaces with the time tracking REST API.
    /// </summary>
    public class TimeTrackingManagement
    {
        private readonly IConnection _connection;

        public TimeTrackingManagement(IConnection connection)
        {
            _connection = connection;
        }

        /// <summary>
        /// Get the list of configured work types for a specific project.
        /// </summary>
        /// <param name="projectIdentifier">Project identifier.</param>
        /// <returns>Collection of work types.</returns>
        public IEnumerable<WorkType> GetWorkTypesForProject(string projectIdentifier)
        {
            return _connection.Get<IEnumerable<WorkType>>(String.Format("admin/project/{0}/timetracking/worktype", projectIdentifier));
        }

        /// <summary>
        /// Create a new work item for a particular issue.
        /// </summary>
        /// <param name="issueId">Issue identifier.</param>
        /// <param name="workItem">Work item detail.</param>
        public void CreateWorkItem(string issueId, WorkItem workItem)
        {
            if (!_connection.IsAuthenticated)
            {
                throw new InvalidRequestException(Language.YouTrackClient_CreateIssue_Not_Logged_In);
            }

            try
            {
                XmlDocument document = new XmlDocument();
                XmlElement workItemElement = (XmlElement)document.AppendChild(document.CreateElement("workItem"));
                workItemElement.AppendChild(document.CreateElement("date")).InnerText = workItem.Date.ToString();
                workItemElement.AppendChild(document.CreateElement("duration")).InnerText = workItem.Duration.ToString();

                if (!String.IsNullOrEmpty(workItem.Description))
                {
                    workItemElement.AppendChild(document.CreateElement("description")).InnerText = workItem.Description;
                }

                if (!String.IsNullOrEmpty(workItem.Type))
                {
                    XmlElement workTypeElement = document.CreateElement("workType");
                    workTypeElement.AppendChild(document.CreateElement("name")).InnerText = workItem.Type;
                    workItemElement.AppendChild(workTypeElement);
                }

                if (!String.IsNullOrEmpty(workItem.AuthorLogin))
                {
                    XmlElement authorElement = document.CreateElement("author");
                    authorElement.SetAttribute("login", workItem.AuthorLogin);
                    workItemElement.AppendChild(authorElement);
                }

                _connection.Post(String.Format("issue/{0}/timetracking/workitem", issueId), document.OuterXml,
                    HttpContentTypes.ApplicationXml, HttpContentTypes.ApplicationXml);
            }
            catch (HttpException httpException)
            {
                throw new InvalidRequestException(httpException.StatusDescription, httpException);
            }
        }
    }
}
