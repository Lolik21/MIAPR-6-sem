using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrammarGenerator
{
    public class Grammar
    {
        public List<string> Vn { get; set; } = new List<string> { "S", "A1", "A2" };
        public List<string> Vt { get; set; } = new List<string> { "a", "b", "c" };
        public List<Link> Links { get; set; } = new List<Link> {
            new StartLink{ StartSumbol = "S", MeadbleSumbol = "c", NextHope = "A1" },
            new StartLink{ StartSumbol = "S", MeadbleSumbol = "b", NextHope = "A4" },
            new MeadleLink{ StartSumbol = "A1", MeadbleSumbol = "a", NextHope = "A1"},
            new MeadleLink{ StartSumbol = "A4", MeadbleSumbol = "b", NextHope = "A1"},
            new EndLink{ StartSumbol = "A1", MeadbleSumbol = "b"}
        };

        public override string ToString()
        {
            string resultStr = string.Empty;

            resultStr += "G(Vn, Vt, P, S) ";
            resultStr += "Vn = ";
            CreateBrakets(Vn,ref resultStr);
            resultStr += " ";
            resultStr += "Vt = ";
            CreateBrakets(Vt,ref resultStr);
            resultStr += " ";
            RulesToString(Links,ref resultStr);
            return resultStr;
        }

        private void RulesToString(List<Link> links,ref string resultStr)
        {
            resultStr += "P: ";
            foreach(var link in links)
            {
                resultStr += link.StartSumbol + " -> " + link.MeadbleSumbol + link.NextHope ;
                if (links.Last() == link)
                {
                    resultStr += ".";                   
                }
                else
                {
                    resultStr += ", ";
                }             
            }
            
        }

        private void CreateBrakets(List<string> strList,ref string resultStr)
        {
            resultStr += "(";
            foreach (var item in strList)
            {
                if (strList.Last() == item)
                {
                    resultStr += item + ")";
                }
                else
                {
                    resultStr += item + ", ";
                }
            }
        }
    }
}
