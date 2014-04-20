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
using System.Dynamic;

namespace YouTrackSharp.Issues
{
    public class WorkItems : List<WorkItem>
    {

    }

    public class WorkItem : DynamicObject
    {
        readonly IDictionary<string, object> _allFields = new Dictionary<string, object>(StringComparer.InvariantCultureIgnoreCase);
        string _id;

        public string Id
        {
            get { return _id ?? (_id = (string)_allFields["id"]); }
        }

        DateTime? _date;
        public DateTime Date
        {
            get
            {
                if (_date.HasValue)
                    return _date.Value;

                // Ref: http://stackoverflow.com/questions/249760/how-to-convert-unix-timestamp-to-datetime-and-vice-versa
                // Java timestamp is millisecods past epoch
                var dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0);
                dtDateTime = dtDateTime.AddSeconds(Math.Round((long)_allFields["date"] / (double)1000)).ToLocalTime();
                return (_date = dtDateTime).Value;
            }
        }

        int? _duration;

        public TimeSpan Duration
        {
            get
            {
                if (_duration.HasValue)
                    return new TimeSpan(0, 0, _duration.Value, 0);
                _duration = (int)_allFields["duration"];
                return new TimeSpan(0, 0, _duration.Value, 0);
            }
        }

        string _description;

        public string Description
        {
            get { return _description ?? (_description = (string)_allFields["description"]); }
        }

        string _login;

        public string Login
        {
            get
            {
                if (_login != null)
                    return _login;
                IDictionary<string, object> author = (ExpandoObject)_allFields["author"];
                return (_login = (string)author["login"]);
            }
        }


        bool _worktypeInitialized;
        string _worktype;

        public string Worktype
        {
            get
            {
                if (_worktypeInitialized)
                    return _worktype;
                var key = "worktype";
                if (_allFields.ContainsKey(key))
                {
                    IDictionary<string, object> wtype = (ExpandoObject)_allFields["worktype"];
                    _worktype = (string)wtype["name"];
                }
                _worktypeInitialized = true;
                return _worktype;
            }
        }

        public ExpandoObject ToExpandoObject()
        {
            IDictionary<string, object> expando = new ExpandoObject();

            foreach (dynamic field in _allFields)
            {
                expando.Add(field.Key.ToLower(), field.Value);
            }
            return (ExpandoObject)expando;
        }

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            if (_allFields.ContainsKey(binder.Name))
            {
                result = _allFields[binder.Name];
                return true;
            }
            return base.TryGetMember(binder, out result);
        }

        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            if (String.Compare(binder.Name, "field", StringComparison.OrdinalIgnoreCase) == 0)
            {
                foreach (var val in (IEnumerable<dynamic>)value)
                {
                    _allFields[val.name] = val.value;
                }
                return true;
            }
            _allFields[binder.Name] = value;
            return true;
        }
    }
}