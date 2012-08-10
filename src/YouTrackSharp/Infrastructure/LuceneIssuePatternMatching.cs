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
using System.Collections;
using System.Collections.Generic;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.QueryParsers;
using Lucene.Net.Search;
using Lucene.Net.Store;
using Version = Lucene.Net.Util.Version;

namespace YouTrackSharp.Infrastructure
{
    public class LuceneSearchEngine : ISearchEngine
    {
        static RAMDirectory _ramDirectory;
        IList<Document> _documents;
        IEnumerable _items;
        string _keyField;
        string[] _searchFields;

        public IEnumerable<string> FindMatchingKeys(string textToFind, IEnumerable items, string[] searchFields, string keyField)
        {
            _items = items;
            _searchFields = searchFields;
            _keyField = keyField;


            SetupDocumentsForIndexing();
            GenerateIndex();
            return Search(textToFind);
        }

        void SetupDocumentsForIndexing()
        {
            _documents = new List<Document>();

            foreach (var item in _items)
            {
                var document = new Document();

                foreach (var searchField in _searchFields)
                {
                    var propertyValue = item.GetType().GetProperty(searchField).GetValue(item, null);

                    if (propertyValue != null)
                    {
                        var field = new Field(searchField, propertyValue.ToString(), String.Compare(searchField, _keyField, true) == 0 ? Field.Store.YES : Field.Store.NO, Field.Index.ANALYZED);

                        document.Add(field);
                    }
                }

                _documents.Add(document);
            }
        }

        void GenerateIndex()
        {
            _ramDirectory = new RAMDirectory();

            var standardAnalyzer = new StandardAnalyzer(Version.LUCENE_29);

            var indexWriter = new IndexWriter(_ramDirectory, standardAnalyzer, true,
                                              IndexWriter.MaxFieldLength.UNLIMITED);
            foreach (var document in _documents)
            {
                indexWriter.AddDocument(document);
            }

            indexWriter.Optimize();
            indexWriter.Close();
        }


        IEnumerable<string> Search(string textToFind)
        {
            var reader = IndexReader.Open(_ramDirectory, true);
            var searcher = new IndexSearcher(reader);
            var analyzer = new StandardAnalyzer(Version.LUCENE_29);
            var parser = new MultiFieldQueryParser(Version.LUCENE_29, _searchFields, analyzer);

            var query = parser.Parse(textToFind);

            var collector = TopScoreDocCollector.create(100, true);

            searcher.Search(query, collector);

            var hits = collector.TopDocs().scoreDocs;
            var foundKeys = new List<string>();
            foreach (ScoreDoc scoreDoc in hits)
            {
                var document = searcher.Doc(scoreDoc.doc);
                var key = document.Get(_keyField);

                if (key != null && !foundKeys.Contains(key))
                {
                    foundKeys.Add(key);
                }
            }
            reader.Close();
            searcher.Close();
            analyzer.Close();
            return foundKeys;
        }
    }
}