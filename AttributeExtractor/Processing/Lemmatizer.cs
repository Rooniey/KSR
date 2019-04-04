using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using LemmaSharp;


namespace AttributeExtractor.Processing
{
    public class Lemmatizer
    {
        private static ILemmatizer _lemmatizer = new LemmatizerPrebuiltCompact(LanguagePrebuilt.English);


        public static List<Token> Process(List<Token> tokens)
        {
            foreach (var token in tokens)
            {
                token.Word = _lemmatizer.Lemmatize(token.Word);
            }

            return tokens;

        }

    }
}
