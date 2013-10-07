using System;
using System.IO;

public class Solver
{
    public static void Main()
    {
        new IncreasingSequence().run();
    }
}

public class IncreasingSequence
{
    private int N;

    private const int MOD = 1000000007;

    StreamReader input;
    StreamWriter output;

    private long[] A;
    private long n, m, X, Y, Z;

    private long total;

    long[] sequence;
    long[] normalizedSequence;
    long[] dyn;

    long[] tree;

    public IncreasingSequence()
    {
        input = new StreamReader("dir/2008/Round1C/C/C-large.in");
        output = new StreamWriter("dir/2008/Round1C/C/C-large.out");

        this.N = int.Parse(input.ReadLine());
    }

    public void run()
    {
        for (int i = 0; i < N; i++)
        {
            read();
            solve();
            write(i);
        }

        input.Close();
        output.Close();
    }

    private void read()
    {
        long[] parameters = Array.ConvertAll<string, long>(input.ReadLine().Split(' '), long.Parse);

        n = parameters[0];
        m = parameters[1];
        X = parameters[2];
        Y = parameters[3];
        Z = parameters[4];

        A = new long[m];
        for (int i = 0; i < m; i++) A[i] = long.Parse(input.ReadLine());

        sequence = new long[n];
        for (int i = 0; i < n; i++)
        {
            sequence[i] = A[i % m];
            A[i % m] = (X * A[i % m] + Y * (i + 1)) % Z;
        }
    }

    private void solveSmall()
    {
        dyn = new long[n];

        dyn[0] = 1;

        for (int i = 1; i < n; i++)
        {
            long numOfIncreasing = 1;

            for (int j = 0; j < i; j++)
            {
                if (sequence[i] > sequence[j])
                {
                    numOfIncreasing += dyn[j];
                    numOfIncreasing %= MOD;
                }
            }

            dyn[i] = numOfIncreasing;
        }

        total = 0;
        for (int i = 0; i < dyn.Length; i++)
        {
            total += dyn[i];
            total %= MOD;
        }
    }

    private void setValue(long val, long index)
    {
        while (index < tree.Length)
        {
            tree[index] += val;
            tree[index] %= MOD;
            index += (index & -index);
        }
    }

    private long readCumulative(long index)
    {
        long sum = 0;
        while (index > 0)
        {
            sum += tree[index];
            sum %= MOD;
            index -= (index & -index);
        }

        return sum;
    }

    private void solve()
    {
        normalizedSequence = new long[n];

        Array.Copy(sequence, normalizedSequence, sequence.Length);
        Array.Sort(sequence);

        for (int i = 0; i < sequence.Length; i++)
        {
            normalizedSequence[i] = Array.BinarySearch(sequence, normalizedSequence[i]) + 1;
        }

        tree = new long[n + 1];
        this.setValue(1, normalizedSequence[0]);

        for (int i = 1; i < n; i++)
        {
            long numOfIncreasing = this.readCumulative(normalizedSequence[i] - 1);
            this.setValue(numOfIncreasing + 1, normalizedSequence[i]);
        }

        total = this.readCumulative((int)n) % MOD;

    }

    private void write(int c)
    {
        output.WriteLine("Case #{0}: {1}", c + 1, total);
    }
}
