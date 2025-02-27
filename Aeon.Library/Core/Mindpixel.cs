using System.Xml.Linq;

namespace Aeon.Runtime
{
    public class MindpixelXmlHandler
    {
        private readonly string _directoryPath;

        public MindpixelXmlHandler(string directoryPath)
        {
            if (string.IsNullOrEmpty(directoryPath))
            {
                throw new ArgumentException("Directory path cannot be null or empty", nameof(directoryPath));
            }

            _directoryPath = directoryPath;
        }

        public XDocument LoadXmlFile(string fileName)
        {
            string filePath = Path.Combine(_directoryPath, fileName);
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException($"The file {fileName} does not exist in the directory {_directoryPath}");
            }

            return XDocument.Load(filePath);
        }

        public void SaveXmlFile(string fileName, XDocument document)
        {
            string filePath = Path.Combine(_directoryPath, fileName);
            document.Save(filePath);
        }

        public void AddElement(string fileName, XElement element)
        {
            XDocument document = LoadXmlFile(fileName);
            document.Root.Add(element);
            SaveXmlFile(fileName, document);
        }

        public XElement GetElementById(string fileName, string elementId)
        {
            XDocument document = LoadXmlFile(fileName);
            return document.Root.Element(elementId);
        }

        public void RemoveElementById(string fileName, string elementId)
        {
            XDocument document = LoadXmlFile(fileName);
            XElement element = document.Root.Element(elementId);
            if (element != null)
            {
                element.Remove();
                SaveXmlFile(fileName, document);
            }
        }
    }
}
