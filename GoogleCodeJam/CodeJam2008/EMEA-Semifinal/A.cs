using System;
using System.IO;
using System.Globalization;

public class Solver
{
    public static void Main()
    {
        new A().run();
    }
}

/**
 * Solved using Mobius transform. Each triangle point is represented with complex number.
 * Calculate paramters of transform which transforms first triangle to second triangle.
 * Then calculate point which is transformed to itself with Mobius transform. This is fixed point.
 * */
public class A
{
    Complex ONE;
    private int N;

    StreamReader input;
    StreamWriter output;

    Complex z1, z2;
    Complex w1, w2;

    Complex f;

    public A()
    {
        ONE = new Complex(1, 0);
        input = new StreamReader("dir/2008/EMEA-Semifinal/A/A-large.in");
        output = new StreamWriter("dir/2008/EMEA-Semifinal/A/A-large.out");

        N = int.Parse(input.ReadLine());
    }

    private void read()
    {
        string[] p = input.ReadLine().Split(' ');
        z1 = new Complex(int.Parse(p[0]), int.Parse(p[1]));
        z2 = new Complex(int.Parse(p[2]), int.Parse(p[3]));
        p = input.ReadLine().Split(' ');
        w1 = new Complex(int.Parse(p[0]), int.Parse(p[1]));
        w2 = new Complex(int.Parse(p[2]), int.Parse(p[3]));
    }

    private void solve()
    {
        Complex A = (w1 - w2) / (z1 - z2);
        Complex B = (w2 * z1 - w1 * z2) / (z1 - z2);
        f = B / (ONE - A);
    }

    private void write(int I)
    {
        output.WriteLine("Case #{0}: {1} {2}", I,
            f.Re.ToString(CultureInfo.InvariantCulture), f.Im.ToString(CultureInfo.InvariantCulture));
    }

    public void run()
    {
        for (int I = 0; I < N; I++)
        {
            read();
            solve();
            write(I + 1);
        }

        input.Close();
        output.Close();
    }

}

class Complex
{
    public double Re { get; set; }
    public double Im { get; set; }

    public Complex(double re, double im)
    {
        this.Re = re;
        this.Im = im;
    }

    public static Complex operator +(Complex z1, Complex z2)
    {
        return new Complex(z1.Re + z2.Re, z1.Im + z2.Im);
    }

    public static Complex operator -(Complex z1, Complex z2)
    {
        return new Complex(z1.Re - z2.Re, z1.Im - z2.Im);
    }

    public static Complex operator *(Complex z1, Complex z2)
    {
        return new Complex(z1.Re * z2.Re - z1.Im * z2.Im, z1.Re * z2.Im + z1.Im * z2.Re);
    }

    public static Complex operator /(Complex z1, Complex z2)
    {
        double d = z2.Re * z2.Re + z2.Im * z2.Im;
        return new Complex((z1.Re * z2.Re + z1.Im * z2.Im) / d, (z1.Im * z2.Re - z1.Re * z2.Im) / d);
    }
}
