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
using System.Management.Automation;

namespace YouTrackSharp.Issues
{
    public class WorkItem
    {
        // TODO: Convert to DateTime
        /// <summary>
        /// Date and time of the work item in Unix Epoch time format.
        /// </summary>
        [Parameter(Mandatory = true, HelpMessage = "Date and time of the new work item in Unix Epoch time format")]
        public Int64 Date { get; set; }

        /// <summary>
        /// Duration of the work item, in minutes.
        /// </summary>
        [Parameter(Mandatory = true, HelpMessage = "Duration of the work item, in minutes")]
        public int Duration { get; set; }

        /// <summary>
        /// Activity description.
        /// </summary>
        [Parameter(HelpMessage = "Activity description")]
        public string Description { get; set; }

        /// <summary>
        /// Work item type.
        /// </summary>
        [Parameter(HelpMessage = "Work item type")]
        public string Type { get; set; }

        /// <summary>
        /// User that this work item belongs to.
        /// </summary>
        [Parameter(HelpMessage = "User that this work item belongs to")]
        public string AuthorLogin { get; set; }
    }
}
