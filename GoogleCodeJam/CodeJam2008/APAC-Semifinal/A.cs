using System;
using System.IO;

public class Solver
{
    public static void Main()
    {
        new Birds().run();
    }
}

public class Birds
{
    private int C;

    StreamReader input;
    StreamWriter output;

    private const byte BIRD = 1;
    private const byte NOT_BIRD = 0;
    private const byte UNKNOWN = 2;

    int xMin, yMin, xMax, yMax;

    int K;
    int[] nx = new int[1000];
    int[] ny = new int[1000];

    //new animals
    int M;
    int[] x = new int[1000];
    int[] y = new int[1000];
    byte[] result = new byte[1000];

    public Birds()
    {
        input = new StreamReader("dir/2008/APAC-Semifinal/A/A-large.in");
        output = new StreamWriter("dir/2008/APAC-Semifinal/A/A-large.out");

        C = int.Parse(input.ReadLine());
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

    private void read()
    {
        int N = int.Parse(input.ReadLine());

        xMin = int.MaxValue;
        yMin = int.MaxValue;
        xMax = -1;
        yMax = -1;

        K = 0;
        for (int i = 0; i < N; i++)
        {
            string[] p = input.ReadLine().Split(' ');

            int px = int.Parse(p[0]);
            int py = int.Parse(p[1]);

            //update rectangle coordinates
            if (p[2].StartsWith("B"))
            {
                xMin = (px < xMin) ? px : xMin;
                yMin = (py < yMin) ? py : yMin;
                xMax = (px > xMax) ? px : xMax;
                yMax = (py > yMax) ? py : yMax;
            }
            else
            {
                nx[K] = px;
                ny[K] = py;
                K++;
            }
        }

        M = int.Parse(input.ReadLine());
        for (int i = 0; i < M; i++)
        {
            string[] p = input.ReadLine().Split(' ');
            x[i] = int.Parse(p[0]);
            y[i] = int.Parse(p[1]);
        }
    }

    private void solve()
    {
        for (int i = 0; i < M; i++)
        {
            if (x[i] <= xMax && x[i] >= xMin && y[i] >= yMin && y[i] <= yMax)
                result[i] = BIRD;
            else
            {
                bool isUnknown = true;
                for (int j = 0; j < K && isUnknown; j++)
                {
                    isUnknown = (nx[j] > xMax && nx[j] > x[i]) || (nx[j] < xMin && nx[j] < x[i])
                        || (ny[j] > yMax && ny[j] > y[i]) || (ny[j] < yMin && ny[j] < y[i]);
                }

                if (isUnknown)
                    result[i] = UNKNOWN;
                else
                    result[i] = NOT_BIRD;
            }
        }
    }

    private void write(int c)
    {
        output.WriteLine("Case #{0}:", c);
        for (int i = 0; i < M; i++)
        {
            byte b = result[i];
            string s = (b == BIRD) ? "BIRD" : ((b == UNKNOWN) ? "UNKNOWN" : "NOT BIRD");
            output.WriteLine(s);
        }
    }
}