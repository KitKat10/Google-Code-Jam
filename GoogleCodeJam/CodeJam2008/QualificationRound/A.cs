using System;
using System.IO;
using System.Collections.Generic;

public class Solver
{
    public static void Main()
    {
        new Universe().run();
    }
}

public class Universe
{
    private int N;

    private int Q;
    private int S;
    private string[] queries;
    private string[] engines;

    private int minimumSwitches;

    StreamReader input;
    StreamWriter output;

    private ISet<string> qSet;
    private ISet<string> comparingSet;

    public Universe()
    {

        input = new StreamReader("dir/2008/QualificationRound/A/A-large.in");
        output = new StreamWriter("dir/2008/QualificationRound/A/A-large.out");

        this.N = int.Parse(input.ReadLine());
        qSet = new HashSet<string>();
    }

    public void run()
    {
        for (int I = 0; I < N; I++)
        {
            read();
            solve();
            write(I);
        }

        input.Close();
        output.Close();
    }

    private void read()
    {
        this.S = int.Parse(input.ReadLine());
        engines = new string[S];

        for (int i = 0; i < S; i++)
        {
            engines[i] = input.ReadLine();
        }

        this.Q = int.Parse(input.ReadLine());
        queries = new string[Q];

        for (int i = 0; i < Q; i++)
        {
            queries[i] = input.ReadLine();
        }

        comparingSet = new HashSet<string>(engines);
    }

    private void solve()
    {
        minimumSwitches = 0;
        qSet.Clear();

        for (int i = 0; i < Q; i++)
        {
            qSet.Add(queries[i]);
            if (qSet.Count >= S && comparingSet.IsSubsetOf(qSet))
            {
                minimumSwitches++;
                qSet.Clear();
                qSet.Add(queries[i]);
            }
        }
    }

    private void write(int I)
    {
        output.WriteLine("Case #{0}: {1}", I + 1, minimumSwitches);
    }
}