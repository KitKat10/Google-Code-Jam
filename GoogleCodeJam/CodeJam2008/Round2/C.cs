using System;
using System.IO;
using System.Globalization;

public class Solver
{
    public static void Main()
    {
        new StarWars().run();
    }
}

public class StarWars
{
    private int T;

    private int N;

    StreamReader input;
    StreamWriter output;

    float optimal;

    private Ship[] map;

    public StarWars()
    {
        input = new StreamReader("dir/2008/Round2/C/C-large.in");
        output = new StreamWriter("dir/2008/Round2/C/C-large.out");

        this.T = int.Parse(input.ReadLine());
    }

    private void read()
    {
        N = int.Parse(input.ReadLine());

        map = new Ship[N];

        for (int i = 0; i < N; i++)
        {
            map[i] = new Ship(input.ReadLine().Split(' '));
        }
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

    private void solve()
    {
        optimal = 0;

        for (int i = 0; i < N; i++)
        {
            Ship s1 = map[i];
            for (int j = i + 1; j < N; j++)
            {
                Ship s2 = map[j];
                float v = (float)(Math.Abs(s1.x - s2.x) + Math.Abs(s1.y - s2.y) + Math.Abs(s1.z - s2.z)) / (s1.p + s2.p);

                if (v > optimal)
                    optimal = v;
            }
        }
    }

    private void write(int c)
    {
        output.WriteLine("Case #{0}: {1}", c + 1, optimal.ToString("0.000000", CultureInfo.InvariantCulture));
    }
}

public class Ship
{
    public int x { get; set; }
    public int y { get; set; }
    public int z { get; set; }
    public int p { get; set; }

    public Ship(string[] parameters)
    {
        this.x = int.Parse(parameters[0]);
        this.y = int.Parse(parameters[1]);
        this.z = int.Parse(parameters[2]);
        this.p = int.Parse(parameters[3]);
    }

    public float minCruiserStrength(int cx, int cy, int cz)
    {
        float cp = 0;

        cp += Math.Abs(x - cx);
        cp += Math.Abs(y - cy);
        cp += Math.Abs(z - cz);
        cp /= p;

        return cp;
    }
}