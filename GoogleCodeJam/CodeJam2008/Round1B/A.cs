using System;
using System.IO;

public class Solver
{
    public static void Main()
    {
        new CropTriangles().run();
    }
}

public class CropTriangles
{
    private int N;

    private ulong[] paramValues;

    private ulong n;

    private ulong triangleNumber;

    StreamReader input;
    StreamWriter output;

    ulong[,] coords = new ulong[3, 3];

    public CropTriangles()
    {
        input = new StreamReader("dir/2008/Round1B/A/A-large.in");
        output = new StreamWriter("dir/2008/Round1B/A/A-large.out");

        N = int.Parse(input.ReadLine());
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
        paramValues = Array.ConvertAll<string, ulong>(input.ReadLine().Split(' '), ulong.Parse);
    }

    private void solve()
    {

        initPoints();

        triangleNumber = 0;

        for (int i = 0; i < 9; i++)
        {
            ulong p = coords[i/3,i%3];
            triangleNumber += p * (p - 1) * (p - 2) / 6;
        }

        for (int i = 0; i < 9; i++)
        {
            for (int j = i + 1; j < 9; j++)
            {
                for (int k = j + 1; k < 9; k++)
                {
                    if ((i / 3 + j / 3 + k / 3) % 3 == 0 && (i % 3 + j % 3 + k % 3) % 3 == 0)
                    {
                        triangleNumber += coords[i / 3, i % 3] * coords[j / 3, j % 3] * coords[k / 3, k % 3];
                    }
                }
            }
        }

    }

    private void write(int I)
    {
        output.WriteLine("Case #{0}: {1}", I + 1, triangleNumber);
    }

    private void initPoints()
    {

        for (int i = 0; i < 3; i++)
            for (int j = 0; j < 3; j++) coords[i, j] = 0;

        this.n = paramValues[0];

        ulong A = paramValues[1];
        ulong B = paramValues[2];
        ulong C = paramValues[3];
        ulong D = paramValues[4];
        ulong M = paramValues[7];

        ulong X = paramValues[5]; ulong Y = paramValues[6];
        coords[X % 3, Y % 3]++;

        for (ulong i = 1; i <= n - 1; i++)
        {
            X = (A * X + B) % M;
            Y = (C * Y + D) % M;

            coords[X % 3, Y % 3]++;
        }
    }
}