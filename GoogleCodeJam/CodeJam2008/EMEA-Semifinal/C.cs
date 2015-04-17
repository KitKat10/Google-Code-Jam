using System;
using System.IO;
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
    const long MOD = 1000000009;

    int c;

    StreamReader input;
    StreamWriter output;

    int n, k;

    List<int>[] tree = new List<int>[501];
    long result;

    public C(){
    
        input = new StreamReader("dir/2008/EMEA-Semifinal/C/C-large.in");
        output = new StreamWriter("dir/2008/EMEA-Semifinal/C/C-large.out");

        c = int.Parse(input.ReadLine());

        for (int i = 1; i <= 500; i++)
            tree[i] = new List<int>();
    }

    private void read()
    {
        string[] l = input.ReadLine().Split(' ');
        n = int.Parse(l[0]);
        k = int.Parse(l[1]);

        for (int i = 1; i <= n; i++)
            tree[i].Clear();

        for (int i = 0; i < n - 1; i++)
        {
            l = input.ReadLine().Split(' ');
            int n1 = int.Parse(l[0]);
            int n2 = int.Parse(l[1]);
            tree[n1].Add(n2);
            tree[n2].Add(n1);
        }
    }

    private long calculate(int node, int parent)
    {
        if (tree[node].Count == 1 && tree[node][0] == parent)
            return 1;

        long S = (parent == 0) ? 0 : tree[parent].Count;
        int numOfChild = parent == 0 ? tree[node].Count : tree[node].Count - 1;
        if (k - S - numOfChild < 0)
            return 0;

        long res = 1;
        for (int i = 0; i < numOfChild; i++)
            res = (res * (k - S - i)) % MOD;

        for (int i = 0; i < tree[node].Count; i++)
        {
            if (tree[node][i] == parent)
                continue;

            res = (res * calculate(tree[node][i], node)) % MOD;
        }

        return res;
    }

    private void solve()
    {
        result = calculate(1, 0);
    }

    private void write(int I)
    {
        output.WriteLine("Case #{0}: {1}", I, result);
    }

    public void run()
    {
        for(int I=0;I<c;I++)
        {
            read();
            solve();
            write(I + 1);
        }

        input.Close();
        output.Close();
    }
}