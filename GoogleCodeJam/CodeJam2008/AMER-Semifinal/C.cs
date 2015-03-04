using System;
using System.IO;
using System.Globalization;
using System.Collections.Generic;

public class Solver
{
    public static void Main()
    {
        new C().run();
    }
}

public class C
{
    int c;

    StreamReader input;
    StreamWriter output;

    int Q, M;
    double[][] prob = new double[30][];

    List<double> combinationProb = new List<double>(10000);
    List<double> next = new List<double>(10000);

    double result;

    public C()
    {
        input = new StreamReader("dir/2008/AMER-Semifinal/C/C-large.in");
        output = new StreamWriter("dir/2008/AMER-Semifinal/C/C-large.out");

        c = int.Parse(input.ReadLine());
    }

    void read()
    {
        string[] l = input.ReadLine().Split(' ');
        M = int.Parse(l[0]);
        Q = int.Parse(l[1]);

        for (int i = 0; i < Q; i++)
        {
            string[] l2 = input.ReadLine().Split(' ');

            if(prob[i] == null)
                prob[i] = new double[4];

            for (int j = 0; j < 4; j++)
            {
                prob[i][j] = double.Parse(l2[j], CultureInfo.InvariantCulture);
            }
            Array.Sort(prob[i]);
            Array.Reverse(prob[i]);
        }
    }

    /**
     * Solving method for small dataset
     */
    void solve_small()
    {
        if (M >= Math.Pow(4, Q))
        {
            result = 1.0;
            return;
        }

        combinationProb.Clear();
        find(0, 1);
        combinationProb.Sort();
        combinationProb.Reverse();

        result = 0.0;
        for (int i = 0; i < M; i++)
            result += combinationProb[i];
    }

    void find(int q, double p)
    {
        if (q == Q)
        {
            combinationProb.Add(p);
            return;
        }
        else
        {
            for (int i = 0; i < 4; i++)
                find(q + 1, prob[q][i] * p);
        }
    }

    void solve()
    {
        if (M >= Math.Pow(4, Q))
        {
            result = 1.0;
            return;
        }
        
        combinationProb.Clear();
        combinationProb.Add(1.0);

        for (int i = 0; i < Q; i++)
        {
            next.Clear();
            double min = 1;

            for (int k = 0; k < 4; k++)
            {
                if (prob[i][k] == 0.0)
                    continue;

                for (int j = 0; j < combinationProb.Count; j++)
                {
                    double v = combinationProb[j] * prob[i][k];

                    if (next.Count < M)
                    {
                        next.Add(v);
                        min = v < min ? v : min;
                    }
                    else
                    {
                        if (v <= min)
                            break;
                        else
                        {
                            next.Add(v);
                        }
                    }
                }
            }

            next.Sort();

            combinationProb.Clear();
            for (int j = 0; j < Math.Min(next.Count, M); j++)
                combinationProb.Add(next[next.Count - 1 - j]);
        }

        result = 0.0;

        for (int i = 0; i < combinationProb.Count; i++)
            result += combinationProb[i];
    }

    void write(int I)
    {
        output.WriteLine("Case #{0}: {1}", I, result.ToString(CultureInfo.InvariantCulture));
    }

    public void run()
    {
        for (int I = 0; I < c; I++)
        {
            read();
            solve();
            write(I + 1);
        }

        input.Close();
        output.Close();
    }
}