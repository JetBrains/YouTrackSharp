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
    using System.Text;

    public class ProjectVersion
    {
        public string Name { get; set; }
        public string BuildName { get; set; }
        public string Description { get; set; }
        public int? ColorIndex { get; set; }
        public string ReleaseDate { get; set; }
        [JsonName("released")]
        public bool? IsReleased { get; set; }
        [JsonName("archived")]
        public bool? IsArchived { get; set; }

        public string GetQueryString()
        {            
            var sb = new StringBuilder();
            sb.Append(Name);
            if (
                !(!string.IsNullOrEmpty(BuildName) || !string.IsNullOrEmpty(Description) || ColorIndex.HasValue
                  || !string.IsNullOrEmpty(ReleaseDate) || IsReleased.HasValue || IsArchived.HasValue)) return sb.ToString();

            sb.Append("?");
            var description = "description=" + Description.Replace(" ", "+");
            var colorindex = "colorIndex=" + ColorIndex;
            var releaseDate = "releasedate=" + ReleaseDate;
            var released = "released=" + IsReleased;
            var archived = "archived=" + IsArchived;

            sb.Append(string.Join("&", new[] {description, colorindex, releaseDate, released, archived}));
            return sb.ToString();
        }
    }
}