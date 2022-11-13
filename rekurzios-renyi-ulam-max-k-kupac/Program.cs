using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace rekurzios_renyi_ulam_max_k_kupac
{
    class Program
    {
        const int BIGNUMBER = 1000000;
        const int SIZE_K = 50; // maximum of k
        const int SIZE_SUM = 2*SIZE_K; // maximum of x0+x1
        struct State
        {
            public int numOfQuestions;
            public List<int[]> winningSteps;
        }
        static State[,,] state = new State[151,151,76]; // [x0,x1,k]
        static StreamWriter sw = new StreamWriter("output.txt");

        static void Main(string[] args)
        {
            CalculateStates();
            Console.WriteLine("Done! Results can be found in output.txt. Press enter to exit.");
            Console.ReadLine();
        }

        static State recursion(int x0, int x1, int k)
        {
            State answer;
            answer.numOfQuestions = BIGNUMBER;
            answer.winningSteps = new List<int[]>();
            for (int i = 0; i <= k && i<=x0; i++)
            {
                for (int j = 0; j <= k && j<=x1; j++)
                {
                    if(i+j<=k && i + j > 0) // trivial conditions
                    {
                        int y0 = i;
                        int y1 = x0 - i + j;
                        int z0 = x0 - i;
                        int z1 = x1 + i - j;
                        if(state[y0,y1,k].numOfQuestions+1 < answer.numOfQuestions && state[z0, z1, k].numOfQuestions+1 < answer.numOfQuestions)
                        {
                            // It is a better question
                            answer.winningSteps = new List<int[]>();
                            answer.numOfQuestions = Math.Max(state[y0, y1, k].numOfQuestions + 1, state[z0, z1, k].numOfQuestions + 1);
                            int[] goodQuestion = { i, j };
                            answer.winningSteps.Add(goodQuestion);
                        }
                        else if (state[y0, y1, k].numOfQuestions+1 <= answer.numOfQuestions && state[z0, z1, k].numOfQuestions+1 <= answer.numOfQuestions)
                        {
                            // It is a not worse and not better question
                            int[] goodQuestion = { i, j };
                            answer.winningSteps.Add(goodQuestion);
                        }
                    }
                }
            }

            return answer;
        }

        static void CalculateStates()
        {
            for (int i = 0; i <= SIZE_K; i++)
            {
                state[0, 0, i].numOfQuestions = 0;
                state[0, 1, i].numOfQuestions = 0;
            }
            sw.Write("x0" + "\t" + "x1" + "\t" + "k" + "\t" + "questions needed" + "\t" + "[...winning steps]"); // Header
            sw.WriteLine();
            for (int sum = 1; sum <= SIZE_SUM; sum++)
            {
                Console.WriteLine("Sum: " + sum + " / " + SIZE_SUM);
                for (int x0 = 0; x0 <= sum; x0++)
                {
                    int x1 = sum - x0;
                    for (int k = 1; k <= SIZE_K; k++)
                    {
                        if (2 * (k - 1) < x0 + x1 && (x0 > 0 || x1 > 1)) // reason for the second condition is that for (0,1) we shuld get 0
                        {
                            state[x0, x1, k] = recursion(x0, x1, k);
                            string s = "";
                            for (int i = 0; i < state[x0, x1, k].winningSteps.Count; i++)
                            {
                                s += "\t"+string.Join(",", state[x0, x1, k].winningSteps[i]);
                            }
                            sw.Write(x0 + "\t" + x1 + "\t" + k + "\t" + state[x0, x1, k].numOfQuestions + s);
                            sw.WriteLine();
                        }
                        else
                        {
                            // Using symmetry
                            state[x0, x1, k] = state[x0, x1, k - 1];
                        }
                    }
                }
            }
        }
    }
}
