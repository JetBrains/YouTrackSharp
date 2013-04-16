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

using JsonFx.Json;

namespace YouTrackSharp.Projects
{
    using System.Collections.Generic;
    using System.Text;

    public class ProjectVersion
    {        
        [JsonName("value")]
        public string Name { get; set; }        
        public string Description { get; set; }
        public int? ColorIndex { get; set; }
        public long? ReleaseDate { get; set; }
        [JsonName("released")]
        public bool? IsReleased { get; set; }
        [JsonName("archived")]
        public bool? IsArchived { get; set; }

        public string GetQueryString()
        {            
            var sb = new StringBuilder();
            sb.Append(Name);
            if (
                !(!string.IsNullOrEmpty(Name) || !string.IsNullOrEmpty(Description) || ColorIndex.HasValue
                  || ReleaseDate.HasValue || IsReleased.HasValue || IsArchived.HasValue)) return sb.ToString();

            sb.Append("?");

            if (!string.IsNullOrEmpty(Description)) sb.Append("&description=" + Description.Replace(' ', '+'));
            if (ColorIndex.HasValue) sb.Append("&colorIndex=" + ColorIndex.Value);
            if (ReleaseDate.HasValue) sb.Append("&releaseDate=" + ReleaseDate.Value.ToString());
            if (IsReleased.HasValue) sb.Append("&released=" + IsReleased.Value.ToString());
            if (IsArchived.HasValue) sb.Append("&archived=" + IsArchived.Value.ToString());
            
            return sb.ToString();
        }
    }

    public class VersionBundle
    {
        public string Name { get; set; }
        public IEnumerable<ProjectVersion> Version { get; set; }
    }
}