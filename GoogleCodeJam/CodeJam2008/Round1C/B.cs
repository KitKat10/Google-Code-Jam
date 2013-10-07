using System;
using System.IO;

public class Solver
{
    public static void Main(string[] args)
    {
        new UglyNumbers().run();
    }
}

public class UglyNumbers
{
    private int N;

    private string number;

    StreamReader input;
    StreamWriter output;


    private long[,] dyn;
    private const int MOD = 210;
    long counter = 0;

    public UglyNumbers()
    {
        input = new StreamReader("dir/2008/Round1C/B/B-large.in");
        output = new StreamWriter("dir/2008/Round1C/B/B-large.out");

        this.N = int.Parse(input.ReadLine());
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
        number = input.ReadLine();
    }

    private void solve()
    {
        int len = number.Length;
        dyn = new long[len + 1, MOD];
        dyn[0, 0] = 1;

        for (int i = 0; i < len; i++)
        {
            long num = 0;
            for (int j = i; j < len; j++)
            {
                num = (num * 10 + number[j] - '0') % MOD;
                for (int k = 0; k < MOD; k++)
                {
                    dyn[j + 1, k] += dyn[i, (k + num) % MOD];
                    if (i > 0) dyn[j + 1, k] += dyn[i, (k - num + MOD) % MOD];
                }
            }
        }

        counter = 0;
        for (int i = 0; i < MOD; i++)
        {
            if (i % 2 == 0 || i % 3 == 0 || i % 5 == 0 || i % 7 == 0)
            {
                counter += dyn[number.Length, i];
            }
        }

        //counter /= 2;
    }

    private void write(int I)
    {
        output.WriteLine("Case #{0}: {1}", I + 1, counter);
    }
}