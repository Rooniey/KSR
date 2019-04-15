using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using Common.Model;

namespace DataSetParser
{
    public static class SgmDataReader
    {
        public static List<Article> GetAllArticles(string dirPath, string labelName)
        {
            return Directory.GetFiles(dirPath)
                .Where(p => Path.GetExtension(p) == ".sgm")
                .SelectMany(f => ReadAllSamples(f, labelName))
                .ToList();
        }

        public static List<Article> ReadAllSamples(string filePath, string labelName)
        {
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException($"File not found under: {filePath}");
            }

            List<Article> samples = new List<Article>();
            string label = "unknown";
            using (var reader = new XmlTextReader(filePath))
            {
                while (reader.Read())
                {
                    if (reader.NodeType == XmlNodeType.Element)
                    {
                        if (string.Equals(reader.Name, labelName, StringComparison.InvariantCultureIgnoreCase))
                        {
                            label = "unknown";
                            while (reader.Read())
                            {
                                while (reader.Name.ToUpperInvariant() == "D")
                                {
                                    reader.Read();
                                }

                                if (!string.IsNullOrWhiteSpace(reader.Value))
                                {
                                    label = reader.Value;
                                    break;
                                }

                                if (reader.NodeType == XmlNodeType.EndElement
                                    && string.Equals(reader.Name, $"{labelName}",
                                        StringComparison.InvariantCultureIgnoreCase))
                                {
                                    break;
                                }
                            }
                        }

                        if (reader.Name.ToUpperInvariant() != "TITLE") continue;

                        reader.Read();
                        string body = ReadTextElement(reader, "body");
                        if (body != null && label != "unknown")
                        {
                            samples.Add(new Article(body, label));
                        }
                    }
                }
            }
            return samples;
        }

        private static string ReadTextElement(XmlTextReader reader, string elementName)
        {
            bool readerState = true;
            while (!string.Equals(reader.Name, elementName, StringComparison.InvariantCultureIgnoreCase)
                && readerState)
            {
                readerState = reader.Read();
            }

            if (!readerState) return null;

            reader.Read();
            return reader.Value;

        }
    }
}
