using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace ParseLoader
{
    class XMLParseStrategy : ParseStrategy
    {
        public override IEnumerable<Data> Parse(string data)
        {
            using var reader = new StringReader(data);
            XmlReader serializer = XmlReader.Create(reader);
            serializer.MoveToContent();
            serializer.ReadStartElement();

            // deserialize nodes
            // return data node by node

            return Array.Empty<Data>();
        }
    }
}