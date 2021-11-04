using System;

namespace ParseLoader
{
    public class XMlProperty
    {
        /// <summary>
        /// Creates property assigning filter for xml parser
        /// </summary>
        /// <param name="assignPropertyName">Property to which expected assign of searched value</param>
        /// <param name="pathParts">Parts of property search filter
        /// <list type="bullet">
        /// <item>
        /// typeStruct : struct { Property }
        /// <para/>path : Property
        /// </item>
        /// <item>
        /// typeStruct : struct { struct { Property } }
        /// <para/>path : struct.Property
        /// </item>
        /// </list>
        /// </param>
        /// <param name="valueType">Type of property value</param>
        public XMlProperty(string assignPropertyName, string pathParts, Type valueType)
        {
            PathParts = pathParts.Split('.', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
            AssignPropertyName = assignPropertyName;
            ValueType = valueType;
        }

        public string AssignPropertyName { get; }
        public string[] PathParts { get; }
        public Type ValueType { get;}
    }
}