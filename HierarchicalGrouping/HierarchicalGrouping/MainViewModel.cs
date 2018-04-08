using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HierarchicalGrouping
{
    class MainViewModel
    {
        public List<List<int>> Distanses { get; set; } = new List<List<int>>();

        public MainViewModel()
        {
            Random random = new Random();
            for (int i = 0; i < 99; i++)
            {
                Distanses.Add(new List<int>());
                for (int j = 0; j < 99; j++)
                {
                    Distanses[i].Add(0);
                }
            }
        }

        public void SetDistanses(int N)
        {
            Random random = new Random();
            Distanses.Clear();

            for (int i = 0; i < N; i++)
            {
                Distanses.Add(new List<int>());
                for (int j = 0; j < N; j++)
                {
                    Distanses[i].Add(0);
                }
            }

            for (int i = 0; i < N; i++)
            {
                for (int j = 0; j < N; j++)
                {
                    if (i == j)
                    {
                        Distanses[i][j] = 0;
                    }
                    else
                    {
                        Distanses[i][j] = random.Next(1, N * 4);
                        Distanses[j][i] = Distanses[i][j];
                    }
                }
            }
        }
    }
}
