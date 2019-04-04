using DataSetParser.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace DataSetParser
{
    public static class SgmDataReader
    {
        public static List<LabeledArticle> ReadAllSamples(string filePath, string labelName)
        {
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException($"File not found under: {filePath}");
            }
            else
            {
                List<LabeledArticle> samples = new List<LabeledArticle>();
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
                            if (reader.Name.ToUpperInvariant() == "TITLE")
                            {
                                reader.Read();
                                string title = reader.Value;
                                string dateline = ReadTextElement(reader, "dateline");
                                string body = ReadTextElement(reader, "body");
                                if (title != null && dateline != null && body != null)
                                {
                                    samples.Add(
                                        new LabeledArticle(
                                            new Article() { Title = title, Body = body },
                                            label
                                    ));
                                }
                            }
                        }
                    }
                }
                return samples;
            }
        }

        private static string ReadTextElement(XmlTextReader reader, string elementName)
        {
            bool readerState = true;
            while (!string.Equals(reader.Name, elementName, StringComparison.InvariantCultureIgnoreCase)
                && readerState)
            {
                readerState = reader.Read();
            }

            if (readerState)
            {
                reader.Read();
                return reader.Value;
            }

            return null;
        }
    }
}
