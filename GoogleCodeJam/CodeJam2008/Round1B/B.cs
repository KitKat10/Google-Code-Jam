using System;
using System.IO;

public class Solver
{
    public static void Main()
    {
        new NumberSets().run();
    }
}

public class NumberSets
{
    private int C;

    private long A;
    private long B;
    private int P;

    StreamReader input;
    StreamWriter output;

    private int setNumber;

    public NumberSets()
    {
        input = new StreamReader("dir/2008/Round1B/B/B-large.in");
        output = new StreamWriter("dir/2008/Round1B/B/B-large.out");

        this.C = int.Parse(input.ReadLine());
    }

    public void run()
    {
        for (int I = 0; I < C; I++)
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
        string[] p = input.ReadLine().Split(' ');

        this.A = long.Parse(p[0]);
        this.B = long.Parse(p[1]);

        this.P = int.Parse(p[2]);
    }

    private void solve()
    {
        DisjointSet ds = new DisjointSet(B - A + 1);

        for (long d = P; d < B - A; d++)
        {
            if (isPrime((int)d))
            {
                for (long i = (d - A % d) % d; d + i <= B - A; i = i + d) ds.union(i, d + i);
            }
        }

        setNumber = ds.numberOfSets();
    }

    private void write(int I)
    {
        output.WriteLine("Case #{0}: {1}", I + 1, setNumber);
    }

    private bool isPrime(int x)
    {
        if (x == 2) return true;
        for (int i = 2; i * i <= x; i++)
        {
            if (x % i == 0) return false;
        }
        return true;
    }
}

public class DisjointSet
{
    long[] relations;

    public DisjointSet(long n)
    {
        relations = new long[n];

        for (int i = 0; i < n; i++) relations[i] = -1;
    }

    public long findRoot(long x)
    {
        if (relations[x] < 0) return x;
        else return relations[x] = findRoot(relations[x]);
    }

    public void union(long x, long y)
    {
        x = findRoot(x);
        y = findRoot(y);
        if (x == y) return;
        if (relations[x] > relations[y])
        {
            long v = relations[x];
            relations[x] = relations[y];
            relations[y] = v;
        }
        relations[x] += relations[y];
        relations[y] = x;
    }

    public int numberOfSets()
    {
        int n = 0;
        for (int i = 0; i < relations.Length; i++) if (relations[i] < 0) n++;
        return n;
    }

}

