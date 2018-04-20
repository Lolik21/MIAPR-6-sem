using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrammarGenerator
{
    public static class Generator
    {
        public static Grammar GenerateGrammar(List<string> chains)
        {
            int index = 1;
            Grammar grammar = new Grammar();
            grammar.Links.Clear();
            foreach (var chain in chains)
            {
                ProcessOneChain(grammar, chain,ref index);
            }
            GetRecursiveGrammars(grammar);

            return grammar;
        }

        private static void ProcessOneChain(Grammar grammar, string chain, ref int index)
        {
            for (int i = 0; i < chain.Length; i++)
            {
                if (i == 0)
                {
                    grammar.Links.Add(new StartLink
                    {
                        StartSumbol = "S",
                        MeadbleSumbol = chain[i].ToString()
                    });
                    if (chain.Length > i + 1)
                    {
                        grammar.Links.Last().NextHope = "A" + Convert.ToString(index);
                        index++;
                    }
                }
                else
                {
                    if (i == chain.Length - 1)
                    {
                        grammar.Links.Add(new EndLink
                        {
                            StartSumbol = "A" + Convert.ToString(index - 1),
                            MeadbleSumbol = chain[i].ToString()
                        });
                    }
                    else
                    {
                        grammar.Links.Add(new MeadleLink
                        {
                            StartSumbol = "A" + Convert.ToString(index - 1),
                            MeadbleSumbol = chain[i].ToString(),
                            NextHope = "A" + Convert.ToString(index)
                        });
                        index++;
                    }
                }
            }
        }

        private static void GetStringFromChain()
        {

        }

        private static void GetRecursiveGrammars(Grammar grammar)
        {
            int i = 0;
            while(i < grammar.Links.Count-1)
            {
                bool endOfPair = false;
                int startInd = i;
                int endInd = i;
                while (grammar.Links[i] is MeadleLink && grammar.Links[i+1] is MeadleLink && !endOfPair)
                {
                    if (grammar.Links[i].MeadbleSumbol == grammar.Links[i+1].MeadbleSumbol)
                    {
                        endInd += 1;
                        i++;
                    }
                    else
                    {
                        endOfPair = true;
                    }             
                }
                if (!endOfPair) i++;

                if (startInd != endInd)
                {
                    GetRecursiveGrammar(grammar, startInd, endInd);
                }    
            }
        }

        private static void GetRecursiveGrammar(Grammar grammar, int startInd, int endInd)
        {
            Link link1 = grammar.Links[startInd];
            Link link2 = grammar.Links[startInd + 1];
            Link link3 = grammar.Links[endInd + 1];

            link1.NextHope = link1.StartSumbol;
            link2.StartSumbol = link1.StartSumbol;
            link2.NextHope = link3.StartSumbol;

            if (startInd + 2 <= endInd)
            {
                grammar.Links.RemoveRange(startInd + 2, endInd - startInd - 1);
            }
        }

        private static void SimplifyGrammar()
        {

        }

        public static string GenerateChains(Grammar grammar, int chansCount)
        {
            List<string> chains = new List<string>();
            string resultStr = string.Empty;
            Random random = new Random();
            var startLinks = grammar.Links.Where((link) => link is StartLink);
            for (int i = 0; i < chansCount; i++)
            {
                var startLink = startLinks.ElementAt(random.Next(startLinks.Count()));
                ProcessLink(grammar, startLink, ref resultStr, random);
                chains.Add(resultStr);
                resultStr = string.Empty;
            }

            resultStr = string.Empty;

            foreach (var chain in chains)
            {
                resultStr += chain + ", ";
            }
            return resultStr;
        }

        private static void ProcessLink(Grammar grammar, Link link, ref string resultStr, Random random)
        {
            resultStr += link.MeadbleSumbol;
            if (link.NextHope != null)
            {
                var nextLinks = grammar.Links.Where(
                    (nextlink) => nextlink.StartSumbol == link.NextHope);
                var nextLink = nextLinks.ElementAt(random.Next(nextLinks.Count()));
                ProcessLink(grammar, nextLink, ref resultStr, random);
            }
        }
    }
}
