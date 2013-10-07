using System;
using System.IO;

public class Solver
{
    public static void Main()
    {
        new Numbers().run();
    }
}

public class Numbers
{
    private int T;

    private int n;

    private string digits;

    private int[,] matrix = new int[,] { { 3, 5 }, { 1, 3 } };

    StreamReader input;
    StreamWriter output;

    public Numbers()
    {
        input = new StreamReader("dir/2008/Round1A/C/C-large.in");
        output = new StreamWriter("dir/2008/Round1A/C/C-large.out");

        this.T = int.Parse(input.ReadLine());
    }

    public void run()
    {
        for (int I = 0; I < T; I++)
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
        this.n = int.Parse(input.ReadLine());
    }

    private void solve()
    {
        int[,] sequenceMatrix = fastExponent(matrix, n);
        int x = (2 * sequenceMatrix[0, 0] - 1) % 1000;

        this.digits = x.ToString("000");
    }

    private void write(int I)
    {
        output.WriteLine("Case #{0}: {1}", I + 1, digits);
    }

    private int[,] multiplication(int[,] x, int[,] y)
    {
        int[,] result = new int[2, 2];

        for (int i = 0; i < 2; i++)
        {
            for (int j = 0; j < 2; j++)
            {
                for (int k = 0; k < 2; k++)
                {
                    result[i, j] += x[i, k] * y[k, j];
                    result[i, j] %= 1000;
                }

            }
        }

        return result;
    }

    private int[,] fastExponent(int[,] m, int n)
    {
        if (n == 1) return m;

        if (n % 2 == 0)
        {
            return fastExponent(multiplication(m, m), n / 2);
        }
        else
        {
            return multiplication(m, fastExponent(m, n - 1));
        }
    }
}