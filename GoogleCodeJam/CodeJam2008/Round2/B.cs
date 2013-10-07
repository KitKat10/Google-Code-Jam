using System;
using System.IO;

public class Solver
{
    public static void Main()
    {
        new TriangleArea().run();
    }
}

public class TriangleArea
{
    private int C;

    private int N, M, A;

    StreamReader input;
    StreamWriter output;

    //x1 = 0; x2 = 0;
    private bool isPossible;
    private int x2, y2, x3, y3;

    public TriangleArea()
    {
        input = new StreamReader("dir/2008/Round2/B/B-large.in");
        output = new StreamWriter("dir/2008/Round2/B/B-large.out");

        this.C = int.Parse(input.ReadLine());
    }

    private void read()
    {
        string[] p = input.ReadLine().Split(' ');
        N = int.Parse(p[0]);
        M = int.Parse(p[1]);
        A = int.Parse(p[2]);
    }

    public void run()
    {
        for (int I = 0; I < C; I++)
        {
            read();
            solve();
            write(I + 1);
        }

        input.Close();
        output.Close();
    }

    private void solve()
    {
        isPossible = true;

        if (A > N * M)
        {
            isPossible = false;
        }
        else if (A == N * M)
        {
            x2 = N;
            y2 = 0;
            x3 = 0;
            y3 = M;
        }
        else if (A % M == 0)
        {
            x2 = A / M;
            y2 = 0;
            x3 = 0;
            y3 = M;
        }
        else if (A % M != 0)
        {
            x2 = 1;
            y2 = M;
            x3 = A / M + 1;
            y3 = (M - A % M);
        }
    }

    private void write(int c)
    {
        if (isPossible)
            output.WriteLine("Case #{0}: 0 0 {1} {2} {3} {4}", c, x2, y2, x3, y3);
        else
            output.WriteLine("Case #{0}: IMPOSSIBLE", c);
    }
}