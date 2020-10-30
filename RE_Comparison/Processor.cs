using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RE_Comparison
{
    class Processor
    { 
        public void Run()
        {
            //custom directory option
            //string[] input_files = System.IO.Directory.GetFiles(@"C:\Users\Thomas\source\repos\RE_Comparison\RE_Comparison\inputs\", "*.txt");
            string[] input_files = System.IO.Directory.GetFiles(@"C:\inputs\", "*.txt");

            for (int i = 0; i < input_files.Length; i++)
            {
                List<Requirement> nfr = new List<Requirement>();
                List<Requirement> fr = new List<Requirement>();

                InputText(nfr, fr, System.IO.File.ReadAllLines(input_files[i]));
                JaccardIndex(nfr, fr);
                OutputResults(fr, "Run" + (i+1) + "-output");
            }
        }

        private void JaccardIndex(List<Requirement> nfr, List<Requirement> fr)
        {
            foreach (Requirement nf_req in nfr)
            {
                foreach (Requirement f_req in fr)
                {
                    double index =  (double)nf_req.getTokens().Intersect(f_req.getTokens()).ToList().Count /
                                    (double)nf_req.getTokens().Union(f_req.getTokens()).ToList().Count;
                    f_req.appendSimilarity(index);
                }
            }
        }

        private void InputText(List<Requirement> nfr, List<Requirement> fr, string[] input_txt)
        {
            foreach (string line in input_txt)
            {
                if (line.Length < 1)
                {
                    continue;
                }
                (string type, string text) = InputSplitter(line);
                if (type.Contains("NFR"))
                {
                    nfr.Add(new Requirement(text, Requirement.Type.NFR));
                }
                else
                {
                    fr.Add(new Requirement(text, Requirement.Type.FR));
                }
            }
        }

        private (string type, string text) InputSplitter(string line)
        {
            string[] split = line.Split(":".ToCharArray());
            return (split[0], split[1]);
        }

        private void OutputResults(List<Requirement> requirements, string output_name)
        {
            string output = "";
            for (int i = 0; i < requirements.Count; i++)
            {
                output += "FR" + (i + 1).ToString();
                foreach (bool similarity in requirements[i].getSimilarityList())
                {
                    output += similarity ? ",1" : ",0";
                }
                output.TrimEnd(',');
                output += '\n';
            }
            //System.IO.File.WriteAllText(@"C:\Users\Thomas\source\repos\RE_Comparison\RE_Comparison\outputs\" + output_name + ".txt", output);
            System.IO.File.WriteAllText(@"C:\outputs\" + output_name + ".txt", output);
        }
    }
}
