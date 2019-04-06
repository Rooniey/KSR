﻿using System.Collections.Generic;

namespace AttributeExtractor.Extracting
{
    public interface ITokenValueCalculator
    {
        double Calculate(string token, List<string> articleWords, List<List<string>> allArticleWords);
    }
}
