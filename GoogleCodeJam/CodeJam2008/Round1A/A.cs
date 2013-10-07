using System;
using System.IO;

public class Solver
{
    public static void Main()
    {
        new MinimumScalarProduct().run();
    }
}

public class MinimumScalarProduct
{
    private int T;

    private int D;
    private Int64[] vector1;
    private Int64[] vector2;

    private Int64 scalarProduct;

    StreamReader input;
    StreamWriter output;

    public MinimumScalarProduct()
    {
        input = new StreamReader("dir/2008/Round1A/A/A-large.in");
        output = new StreamWriter("dir/2008/Round1A/A/A-large.out");

        this.T = int.Parse(input.ReadLine());
    }

    public void run()
    {
        for (int I = 0; I < T; I++)
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
        this.D = int.Parse(input.ReadLine());
        vector1 = Array.ConvertAll<String, Int64>(input.ReadLine().Split(' '), Int64.Parse);
        vector2 = Array.ConvertAll<String, Int64>(input.ReadLine().Split(' '), Int64.Parse);
    }

    private void solve()
    {
        Array.Sort(vector1);
        Array.Sort(vector2); Array.Reverse(vector2);

        this.scalarProduct = 0;

        for (int i = 0; i < D; i++)
        {
            this.scalarProduct += vector1[i] * vector2[i];
        }
    }

    private void write(int I)
    {
        output.WriteLine("Case #{0}: {1}", I, scalarProduct);
    }
}