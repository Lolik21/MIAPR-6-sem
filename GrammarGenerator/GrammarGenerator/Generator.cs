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
            return null;
        }

        private static void GetNotRecursiveGrammar()
        {

        }

        private static void GetRecursiveGrammar()
        {

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
