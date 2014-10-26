using System;
using System.IO;
using System.Globalization;

public class Solver
{
    public static void Main()
    {
        new Millionaire().run();
    }
}

public class Millionaire
{
    private int N;

    private StreamReader input;
    private StreamWriter output;

    private int M, X;
    private double P;

    private double maxProbability;
    private int numOfPartitions;

    private double[,] memoizedValues = new double[1 << 15, 15];

    public Millionaire()
    {
        input = new StreamReader("dir/2008/APAC-Semifinal/C/C-large.in");
        output = new StreamWriter("dir/2008/APAC-Semifinal/C/C-large.out");

        N = int.Parse(input.ReadLine());
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

    private void read()
    {
        string[] line = input.ReadLine().Split(' ');
        M = int.Parse(line[0]);
        P = double.Parse(line[1], CultureInfo.InvariantCulture);
        X = int.Parse(line[2]);
    }

    private void solve()
    {
        numOfPartitions = 1 << M;
        int current = (int)(((double)X * (double)numOfPartitions) / 1000000.0);
        initMemoization();
        maxProbability = findProbability(0, current);
    }

    private void write(int I)
    {
        output.WriteLine("Case #{0}: {1}", I, maxProbability.ToString("0.000000", CultureInfo.InvariantCulture));
        //Console.WriteLine("Case #{0}: {1}", I, maxProbability.ToString("0.000000", CultureInfo.InvariantCulture));
    }

    private double findProbability(int round, int partition)
    {
        if (partition >= numOfPartitions)
            return 1.0;
        else if (round >= M)
            return 0.0;

        if (memoizedValues[partition, round] >= 0.0)
            return memoizedValues[partition, round];

        double maxExpectation = 0.0;
        
        for (int i = 0; i <= partition; i+= 1<<round)
        {
            double expectation = 0.0;

            if (i == 0)
            {
                double p = findProbability(round + 1, partition);
                expectation = P * p + (1 - P) * p;
            }
            else
            {
                double pWin = findProbability(round + 1, partition + i);
                double pLose = findProbability(round + 1, partition - i);
                expectation = P * pWin + (1 - P) * pLose;
            }

            if (maxExpectation < expectation)
                maxExpectation = expectation;
        }

        memoizedValues[partition, round] = maxExpectation;
        return maxExpectation;
    }

    private void initMemoization()
    {
        for (int i = 0; i < 1 << 15; i++)
            for (int j = 0; j < 15; j++)
                memoizedValues[i, j] = -1.0;
    }
}