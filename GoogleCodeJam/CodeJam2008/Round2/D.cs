using System;
using System.IO;

public class Solver
{
    public static void Main()
    {
        new PermRLE().run();
    }


}

public class PermRLE
{
    private int N;

    StreamReader input;
    StreamWriter output;

    private int k;
    private string S;

    private string[] segments;
    private int minLength;
    private int num_of_segments;

    //variables used for large input
    private Graph graph;

    public PermRLE()
    {
        input = new StreamReader("dir/2008/Round2/D/D-large.in");
        output = new StreamWriter("dir/2008/Round2/D/D-large.out");

        this.N = int.Parse(input.ReadLine());
    }

    public void run()
    {
        DateTime dt1 = DateTime.Now;

        for (int I = 0; I < N; I++)
        {
            read();
            solve_large();
            write(I);
        }

        DateTime dt2 = DateTime.Now;
        TimeSpan t = dt2 - dt1;
        Console.WriteLine("{0}", t);

        input.Close();
        output.Close();
    }

    private void read()
    {
        this.k = int.Parse(input.ReadLine());
        this.S = input.ReadLine();
    }

    private void write(int C)
    {
        output.WriteLine("Case #{0}: {1}", C + 1, minLength);
    }

    #region Methods for small input

    private void solve_small()
    {
        segments = new string[S.Length / k];
        int[] permutation = new int[k];

        for (int i = 0; i < k; i++)
            permutation[i] = i;

        int pos = 0;
        while (pos < S.Length)
        {
            segments[pos / k] = S.Substring(pos, k);
            pos += k;
        }

        minLength = int.MaxValue;
        search(permutation, 0);

    }

    private void search(int[] positions, int lim)
    {
        if (lim >= positions.Length - 1)
        {
            string permutated = "";
            foreach (string s in segments)
            {
                for (int i = 0; i < s.Length; i++)
                {
                    permutated += s[positions[i]];
                }
            }

            int e = evaluate(permutated);
            if (e < minLength)
                minLength = e;

        }
        else
        {
            search(positions, lim + 1);
            for (int i = lim + 1; i < positions.Length; i++)
            {
                int p = positions[lim];
                positions[lim] = positions[i];
                positions[i] = p;
                search(positions, lim + 1);
            }

            rotate(positions, lim);
        }
    }

    private void rotate(int[] x, int beg)
    {
        int first = x[beg];
        for (int i = beg; i < x.Length - 1; i++)
        {
            x[i] = x[i + 1];
        }
        x[x.Length - 1] = first;
    }

    private int evaluate(string s)
    {
        char last = s[0];
        int evaluation = 1;

        for (int i = 1; i < s.Length; i++)
        {
            if (s[i] != last)
            {
                evaluation++;
                last = s[i];
            }
        }

        return evaluation;
    }

    #endregion

    #region Methods for large input

    private void solve_large()
    {
        num_of_segments = S.Length / k;
        segments = new string[num_of_segments];

        int pos = 0;
        while (pos < S.Length)
        {
            segments[pos / k] = S.Substring(pos, k);
            pos += k;
        }

        minLength = int.MaxValue;
        graph = new Graph(k);

        for (int i = 0; i < k; i++)
        {
            for (int j = i + 1; j < k; j++)
            {
                int w = 0;
                for (int c = 0; c < num_of_segments; c++)
                {
                    if (segments[c][i] != segments[c][j])
                        w++;
                }
                graph.setWeight(i, j, w);
            }
        }



        for (int i = 0; i < k; i++)
        {
            for (int j = 0; j < k; j++)
            {
                if (i != j)
                {
                    int word_break = 0;
                    for (int c = 0; c < num_of_segments - 1; c++)
                    {
                        if (segments[c + 1][i] != segments[c][j])
                            word_break++;
                    }

                    int mask = (1 << k) - 1;
                    mask = mask & (~(1 << i));
                    mask = mask & (~(1 << j));

                    int hamLength = graph.calculateAlmostHamiltonianCycles(i, j, mask);
                    int v = hamLength + word_break + 1;
                    if (v < minLength)
                        minLength = v;
                }
            }
        }

    }

    #endregion


}


public class Graph
{
    private int num_of_nodes;
    private int[,] weights;

    /***
     * Length of path from s to e going through nodes in mask (nodes thaht have to be visited are == 1)
     ***/
    private int[, ,] dyn;


    public Graph(int n)
    {
        this.num_of_nodes = n;
        weights = new int[n, n];
        dyn = new int[n, n, 1 << n];

        for (int i = 0; i < n; i++)
        {
            for (int j = 0; j < n; j++)
            {
                for (int k = 0; k < 1 << n; k++)
                {
                    dyn[i, j, k] = -1;
                }
            }
        }
    }

    public void setWeight(int n1, int n2, int w)
    {
        weights[n1, n2] = w;
        weights[n2, n1] = w;
    }

    public int calculateAlmostHamiltonianCycles(int s, int e, int mask)
    {
        int v = dyn[s, e, mask];
        if (v != -1)
            return v;

        if (mask == 0)
        {
            dyn[s, e, mask] = weights[s, e];
            return weights[s, e];
        }

        int optimum = int.MaxValue;
        for (int i = 0; i < num_of_nodes; i++)
        {
            if ((mask & (1 << i)) != 0)
            {
                optimum = Math.Min(optimum, weights[s, i] + calculateAlmostHamiltonianCycles(i, e, mask ^ (1 << i)));
                dyn[s, e, mask] = optimum;
                dyn[e, s, mask] = optimum;
            }
        }

        return optimum;
    }

}