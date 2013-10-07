using System;
using System.IO;

public class Solver
{
    public static void Main()
    {
        new FlySwatter().run();
    }
}

public class FlySwatter
{
    private int N;

    private double f;
    private double R;
    private double t;
    private double r;
    private double g;

    private double P;

    StreamReader input;
    StreamWriter output;

    public FlySwatter()
    {
        input = new StreamReader("dir/2008/QualificationRound/C/C-large.in");
        output = new StreamWriter("dir/2008/QualificationRound/C/C-large.out");

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
        String[] dimensions = input.ReadLine().Split(' ');

        this.f = double.Parse(dimensions[0].Replace('.', ','));
        this.R = double.Parse(dimensions[1].Replace('.', ','));
        this.t = double.Parse(dimensions[2].Replace('.', ','));
        this.r = double.Parse(dimensions[3].Replace('.', ','));
        this.g = double.Parse(dimensions[4].Replace('.', ','));
    }

    private void solve()
    {
        P = 0.0;
        double rad = R - t - f;

        if (2 * f >= g) { this.P = 1.0; return; }

        for (double x = r + f; x <= R - t - f; x = x + g + 2 * r)
        {
            for (double y = r + f; y <= R - t - f; y = y + g + 2 * r)
            {
                double p1x = x; double p1y = y;
                double p2x = x + g - 2 * f; double p2y = y;
                double p3x = x + g - 2 * f; double p3y = y + g - 2 * f;
                double p4x = x; double p4y = y + g - 2 * f;

                if (!isInside(p1x, p1y)) continue;

                if (isInside(p3x, p3y))
                {
                    P = P + (g - 2 * f) * (g - 2 * f);
                }
                else if (!isInside(p3x, p3y) && isInside(p4x, p4y) && isInside(p2x, p2y))
                {
                    double xi = Math.Sqrt(rad * rad - p4y * p4y);
                    double yi = Math.Sqrt(rad * rad - p2x * p2x);

                    double a = Math.Sqrt((xi - p2x) * (xi - p2x) + (yi - p4y) * (yi - p4y));
                    double angle = 2 * Math.Asin(a / (2 * rad));

                    P = P + (g - 2 * f) * (g - 2 * f) - 0.5 * (p3x - xi) * (p3y - yi) +
                        0.5 * rad * rad * (angle - Math.Sin(angle));

                }
                else if (!isInside(p3x, p3y) && !isInside(p4x, p4y) && isInside(p2x, p2y))
                {
                    p3y = Math.Sqrt(rad * rad - p3x * p3x);
                    p4y = Math.Sqrt(rad * rad - p4x * p4x);

                    double a = Math.Sqrt((p3x - p4x) * (p3x - p4x) + (p3y - p4y) * (p3y - p4y));
                    double angle = 2 * Math.Asin(a / (2 * rad));

                    P = P + (p2x - p1x) * (p3y - p2y) + 0.5 * (p2x - p1x) * (p4y - p3y) +
                        0.5 * rad * rad * (angle - Math.Sin(angle));
                }
                else if (!isInside(p3x, p3y) && !isInside(p2x, p2y) && isInside(p4x, p4y))
                {
                    p3x = Math.Sqrt(rad * rad - p3y * p3y);
                    p2x = Math.Sqrt(rad * rad - p2y * p2y);

                    double a = Math.Sqrt((p2x - p3x) * (p2x - p3x) + (p3y - p2y) * (p3y - p2y));
                    double angle = 2 * Math.Asin(a / (2 * (R - t - f)));

                    P = P + (p4y - p1y) * (p3x - p4x) + 0.5 * (p4y - p1y) * (p2x - p3x)
                        + 0.5 * rad * rad * (angle - Math.Sin(angle));
                }
                else if (!isInside(p2x, p2y) && !isInside(p3x, p3y) && !isInside(p4x, p4y))
                {
                    p2x = Math.Sqrt(rad * rad - p2y * p2y);
                    p4y = Math.Sqrt(rad * rad - p4x * p4x);

                    double a = Math.Sqrt((p2x - p4x) * (p2x - p4x) + (p4y - p2y) * (p4y - p2y));
                    double angle = 2 * Math.Asin(a / (2 * (R - t - f)));

                    P = P + 0.5 * (p2x - p1x) * (p4y - p1y) +
                        0.5 * rad * rad * (angle - Math.Sin(angle));
                }
            }
        }

        P = 1.0 - 4 * P / (R * R * Math.PI);
    }

    private bool isInside(double x, double y)
    {
        if (x * x + y * y < (R - t - f) * (R - t - f)) return true;
        else return false;
    }

    private void write(int I)
    {
        output.WriteLine("Case #{0}: {1}", I + 1, P.ToString("0.000000").Replace(',','.'));
    }
}