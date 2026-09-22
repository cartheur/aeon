//
// Copyright 2003-2026 Cartheur. All rights reserved. Reference-only use is permitted under the LICENSE file.
//
using System.Xml;

namespace Aeon.Library
{
    /// <summary>Extracts interpreter instruction-tag names from a category template.</summary>
    public static class InstructionTagExtractor
    {
        /// <summary>Returns distinct, lower-case instruction tags in document order. Invalid template XML has no tags.</summary>
        public static IReadOnlyList<string> Extract(string templateXml)
        {
            if (string.IsNullOrWhiteSpace(templateXml)) return Array.Empty<string>();
            try
            {
                var document = new XmlDocument();
                document.LoadXml(templateXml);
                var tags = new List<string>();
                Collect(document.DocumentElement, tags);
                return tags.Distinct(StringComparer.OrdinalIgnoreCase).ToArray();
            }
            catch (XmlException)
            {
                return Array.Empty<string>();
            }
        }

        private static void Collect(XmlNode node, List<string> tags)
        {
            foreach (XmlNode child in node.ChildNodes)
            {
                if (child.NodeType != XmlNodeType.Element) continue;
                if (!string.Equals(child.Name, "li", StringComparison.OrdinalIgnoreCase))
                {
                    tags.Add(child.Name.ToLowerInvariant());
                }
                Collect(child, tags);
            }
        }
    }
}
