using System;
using System.IO;

public class Solver
{
    public static void Main()
    {
        new TextMessaging().run();
    }
}

public class TextMessaging
{
    private int N;

    private int P;
    private int K;
    private int L;
    private int[] frequencies;

    private ulong nOfPresses;

    StreamReader input;
    StreamWriter output;


    public TextMessaging()
    {
        input = new StreamReader("dir/2008/Round1C/A/A-large.in");
        output = new StreamWriter("dir/2008/Round1C/A/A-large.out");

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
        string[] line1 = input.ReadLine().Split(' ');

        this.P = int.Parse(line1[0]);
        this.K = int.Parse(line1[1]);
        this.L = int.Parse(line1[2]);

        this.frequencies = Array.ConvertAll<string, int>(input.ReadLine().Split(' '), int.Parse);
    }

    private void solve()
    {
        Array.Sort(frequencies);
        Array.Reverse(frequencies);

        this.nOfPresses = 0;

        for (int i = 1; i <= P; i++)
        {
            for (int j = 0; j < K; j++)
            {
                if ((i - 1) * K + j >= L) return;
                nOfPresses += (ulong)(i * frequencies[(i - 1) * K + j]);
            }
        }
    }

    private void write(int I)
    {
        output.WriteLine("Case #{0}: {1}", I + 1, this.nOfPresses);
    }
}