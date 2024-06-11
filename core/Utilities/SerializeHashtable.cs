//
// This autonomous intelligent system is the intellectual property of Christopher Allen Tucker and The Cartheur Company. Copyright 2006 - 2022, all rights reserved.
//
using System;
using System.Collections;
using System.Xml;
using System.Xml.Serialization;

namespace Cartheur.Animals.Utilities
{
    /// <summary>
    /// The serial hashtable class.
    /// </summary>
    public class SerializeHashtable : Hashtable, IXmlSerializable
    {
        public System.Xml.Schema.XmlSchema GetSchema()
        {
           return null;
        }
        /// <summary>
        /// Generates an object from its XML representation.
        /// </summary>
        /// <param name="reader">The <see cref="T:System.Xml.XmlReader" /> stream from which the object is deserialized.</param>
        public void ReadXml(XmlReader reader)
        {
            // Start to use the reader.
            reader.Read();
            // Read the first element ie root of this object
            reader.ReadStartElement("root");

            // Read all elements
            while(reader.NodeType != XmlNodeType.EndElement)
            {
                // parsing the item
                reader.ReadStartElement("item");
                
                // PArsing the key and value 
                string key = reader.ReadElementString("key");
                string value = reader.ReadElementString("value");

                // en reading the item.
                reader.ReadEndElement();
                reader.MoveToContent();

                // add the item
                Add(key, value);
            }
            // Extremely important to read the node to its end. Next call of the reader methods will crash if not called.
            reader.ReadEndElement();
        }
        /// <summary>
        /// Converts an object into its XML representation.
        /// </summary>
        /// <param name="writer">The <see cref="T:System.Xml.XmlWriter" /> stream to which the object is serialized.</param>
        public void WriteXml(XmlWriter writer)
        {
            // Write the root elemnt 
            writer.WriteStartElement("root");

            // For each object in this
            foreach(object key in Keys)
            {
                object value = this[key];
                // Write item, key and value
                writer.WriteStartElement("item");
                writer.WriteElementString("key", key.ToString());
                writer.WriteElementString("value", value.ToString());

                // write </item>
                writer.WriteEndElement();
            }
            // write </root>
            writer.WriteEndElement();
        }
        /// <summary>
        /// Saves the Xml hash to a file.
        /// </summary>
        /// <param name="hash">The hash.</param>
        /// <param name="filename">The filename.</param>
        public static void SaveXml(Hashtable hash, string filename)
        {
            System.IO.StreamWriter stream = null;
            try
            {
                stream = new System.IO.StreamWriter(filename);
                XmlSerializer serialize = new XmlSerializer(hash.GetType());
                serialize.Serialize(stream, hash);
                stream.Close();
            }
            catch(Exception ex)
            {
                if (stream != null)
                    stream.Close();
                Logging.WriteLog(ex.Message, Logging.LogType.Error, Logging.LogCaller.Get);
            }
        }
    }
}
