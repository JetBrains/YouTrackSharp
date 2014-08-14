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
using System.Linq;

namespace YouTrackSharp.Issues
{
    public class Issue : DynamicObject
    {
        readonly IDictionary<string, object> _allFields = new Dictionary<string, object>(StringComparer.InvariantCultureIgnoreCase);

        string _id;

        public string Id
        {
            get { return _id ?? (_id = (string) _allFields["id"]); }
        }

        public override IEnumerable<string> GetDynamicMemberNames()
        {
            var keys = _allFields.Keys.OrderBy(o => o).ToList();
            int tmpCT = keys.Count;
            for (int ct = 0; ct < tmpCT; ct++)
                keys.Add("PropertyByIndex_"+ct.ToString());
            return keys;
        }

        public ExpandoObject ToExpandoObject()
        {
            IDictionary<string, object> expando = new ExpandoObject();


            foreach (dynamic field in _allFields)
            {
                if (String.Compare(field.Key, "ProjectShortName", StringComparison.InvariantCultureIgnoreCase) == 0)
                {
                    expando.Add("project", field.Value);
                }
                else if (String.Compare(field.Key, "permittedGroup", StringComparison.InvariantCultureIgnoreCase) == 0)
                {
                    expando.Add("permittedGroup", field.Value);
                }
                else
                {
                    expando.Add(field.Key.ToLower(), field.Value);
                }
            }
            return (ExpandoObject) expando;
        }

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            if (!binder.Name.ToLower().StartsWith("propertybyindex_"))
            {

                if (_allFields.Any(a => a.Key.ToLower().Replace(" ", "_") == binder.Name.ToLower()))
                {
                    //result = _allFields[binder.Name];
                    if (_allFields.Count(c => c.Key.ToLower().Replace(" ", "_") == binder.Name.ToLower()) == 1)
                        result = _allFields.First(w => w.Key.ToLower().Replace(" ", "_") == binder.Name.ToLower()).Value;
                    else
                    {
                        result =
                            _allFields.Where(w => w.Key.ToLower().Replace(" ", "_") == binder.Name.ToLower())
                                .ToDictionary(d => d.Key, d => d.Value);
                    }
                    return true;
                }
            }
            else
            {
                int index;
                if (!int.TryParse(binder.Name.Split('_')[1], out index))
                    index = -1;
                if (index > -1 && index < _allFields.Count)
                {
                    result = _allFields.ElementAt(index);
                    return true;
                }
            }
            if (base.TryGetMember(binder, out result))
                return true;

            
            result = null;
            return false;
        }

        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            if (String.Compare(binder.Name, "field", StringComparison.OrdinalIgnoreCase) == 0)
            {
                foreach (var val in (IEnumerable<dynamic>) value)
                {
                    _allFields[val.name] = val.value;
                }
                return true;
            }
            _allFields[binder.Name] = value;
            return true;
        }

        public override bool TryGetIndex(GetIndexBinder binder, object[] indexes, out object result)
        {
            int index = (int)indexes[0];
            return _allFields.TryGetValue("PropertyByIndex_" + index, out result);
        }

        public override bool TrySetIndex(
        SetIndexBinder binder, object[] indexes, object value)
        {
            int index = (int)indexes[0];

            if (index > -1)
            {
                if (index < _allFields.Count)
                    _allFields[_allFields.ElementAt(index).Key] = value;
            }
            else
            {
                if (!_allFields.ContainsKey(((KeyValuePair<string, object>) value).Key))
                    _allFields.Add(((KeyValuePair<string, object>) value).Key,
                        ((KeyValuePair<string, object>) value).Value);
            }
            return true;
        }
    }
}
