using System;
using System.Collections.Generic;
using System.Linq;
using Pluralize.NET;

namespace RE_Comparison
{
    class Requirement
    {
        public enum Type { NFR, FR }; //Nonfunctional Requirement, Functional Requirement
        private string input_text;
        private Type type;
        private List<string> filtered_tokens = new List<string>();
        private List<bool> similarity_results = new List<bool>(); //only applicable for fr
        private char[] chars_to_ignore = { ' ', '.', ',', ';', ':', '-', '%', '(', ')' };

        private double similarity_cutoff = 0.13;

        public Requirement(string input, Type req_type)
        {
            type = req_type;
            input_text = input;
            process_tokens();
        }

        public Type getType() { return type; }
        public List<string> getTokens() { return filtered_tokens; }
        public bool[] getSimilarityList()
        {
            return similarity_results.ToArray();
        }
        public void appendSimilarity(double result)
        {
            similarity_results.Add(result > similarity_cutoff ? true : false);
        }

        private void process_tokens()
        {
            IPluralize pluralizer = new Pluralizer();
            //string[] stopwords = System.IO.File.ReadAllLines(@"C:\Users\Thomas\source\repos\RE_Comparison\RE_Comparison\stopwords.txt");
            string[] stopwords = Stopwords.getStopwords();

            foreach (string token in input_text.Split(chars_to_ignore, StringSplitOptions.RemoveEmptyEntries))
            {
                if (!stopwords.Contains(token.ToLower()))
                {
                    filtered_tokens.Add(pluralizer.Singularize(token.ToLower()));
                }
            }
        }
    }
}
