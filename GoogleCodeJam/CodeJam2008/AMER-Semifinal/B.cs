using System;
using System.IO;
using System.Collections.Generic;

public class Solver
{
    public static void Main()
    {
        new CodeSequence().run();
    }
}

public class CodeSequence
{
    const int MOD = 10007;

    int T;

    StreamReader input;
    StreamWriter output;

    int N;
    int[] sequence = new int[1001];

    bool determined;
    int next;
    List<int> diff = new List<int>();

    public CodeSequence()
    {
        input = new StreamReader("dir/2008/AMER-Semifinal/B/B-large.in");
        output = new StreamWriter("dir/2008/AMER-Semifinal/B/B-large.out");

        T = int.Parse(input.ReadLine());
    }

    private void read()
    {
        N = int.Parse(input.ReadLine());
        string[] e = input.ReadLine().Split(' ');

        for (int i = 0; i < N; i++)
            sequence[i] = int.Parse(e[i]);
    }

    private void solve()
    {
        diff.Clear();
        for (int i = 1; i < N; i++)
            diff.Add(moduloSub(sequence[i], sequence[i - 1]));

        while(true)
        {
            if(diff.Count <= 2)
            {
                determined = false;
                break;
            }

            bool evenEq = true;
            bool oddEq = true;

            for (int i = 0; i < diff.Count - 2; i += 2)
            {
                if (diff[i] != diff[i + 2])
                {
                    evenEq = false;
                    break;
                }
            }

            for (int i = 1; i < diff.Count - 2; i += 2)
            {
                if (diff[i] != diff[i + 2])
                {
                    oddEq = false;
                    break;
                }
            }

            if(evenEq ^ oddEq)
            {
                if (evenEq && diff.Count % 2 == 0)
                {
                    determined = true;
                    next = (sequence[N - 1] + diff[0]) % MOD;
                    break;
                }
                else if (oddEq && diff.Count % 2 == 1)
                {
                    determined = true;
                    next = (sequence[N - 1] + diff[1]) % MOD;
                    break;
                }
                else if (evenEq)
                {
                    int idx = 0;
                    while (idx < diff.Count)
                        diff.RemoveAt(idx++);
                }
                else if (oddEq)
                {
                    int idx = 1;
                    while (idx < diff.Count)
                        diff.RemoveAt(idx++);
                }
            }
            else
            {
                determined = false;
                return;
            }
        }
    }

    private void write(int I)
    {
        if (determined)
            output.WriteLine("Case #{0}: {1}", I, next);
        else
            output.WriteLine("Case #{0}: UNKNOWN", I);
    }

    private int moduloSub(int x, int y)
    {
        int s = x % MOD - y % MOD;
        if (s < 0)
            s += MOD;
        return s;
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
}
