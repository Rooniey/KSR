using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataSetParser
{
    public static class KeywordsAccessObject
    {
        public static void SaveKeywords(IEnumerable<string> keywords, string path)
        {
            using (FileStream fs = new FileStream(path, FileMode.Create))
            {
                using (StreamWriter sw = new StreamWriter(fs))
                {
                    sw.WriteLine(String.Join(" ", keywords));
                }
            }
        }

        public static IEnumerable<string> ReadKeywords(string path)
        {
            using (FileStream fs = new FileStream(path, FileMode.Open))
            {
                using (StreamReader sr = new StreamReader(fs))
                {
                    return sr.ReadToEnd().Split(' ');

                }
            }
        }
    }
}
