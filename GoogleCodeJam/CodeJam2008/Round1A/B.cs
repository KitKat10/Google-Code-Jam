using System;
using System.IO;

public class Solver
{
    public static void Main()
    {
        new Milkshakes().run();
    }
}

public class Milkshakes
{
    private int C;

    private int N;
    private int M;

    private short[,] table;

    private short[] solution;
 
    StreamReader input;
    StreamWriter output;

    public Milkshakes()
    {
        input = new StreamReader("dir/2008/Round1A/B/B-large.in");
        output = new StreamWriter("dir/2008/Round1A/B/B-large.out");

        this.C = int.Parse(input.ReadLine());
    }

    public void run()
    {
        for (int I = 0; I < C; I++)
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
        this.N = int.Parse(input.ReadLine());
        this.M = int.Parse(input.ReadLine());

        table = new short[M + 1, N + 1];
        for (int i = 0; i <= M; i++)
            for (int j = 0; j <= N; j++) table[i, j] = -1;

        for (int i = 1; i <= M; i++)
        {
            short[] line = Array.ConvertAll<string, short>(input.ReadLine().Split(' '), short.Parse);
            for (int j = 1; j < 2 * line[0]; j = j + 2)
            {
                table[i, line[j]] = line[j + 1];
            }
        }
    }

    private void solve()
    {
        bool[] satisfied = new bool[M];
        this.solution = new short[N];

        if(update(satisfied, solution)) return;
        else
        {
            for (int i = 1; i <= M; i++)
            {

                if (!satisfied[i - 1])
                {
                    for (int j = 1; j <= N; j++)
                    {
                        if (table[i, j] == 1 && solution[j - 1] == 0)
                        {
                            solution[j - 1] = 1;
                            update(satisfied, solution);
                            i = 0;
                            break;
                        }
                    }
                }
            }
        }

        if (update(satisfied, solution)) return;
        else
        {
            solution = null;
            return;
        }

    }

    private void write(int I)
    {
        if (this.solution == null)
        {
            output.WriteLine("Case #{0}: IMPOSSIBLE", I + 1);
        }
        else
        {
            String sol = "";
            for (int i = 0; i < N; i++) sol += solution[i] + " ";

            sol = sol.TrimEnd();
            output.WriteLine("Case #{0}: {1}", I + 1, sol);
        }
    }

    private bool update(bool[] satisfied, short[] solution)
    {
        bool isAcceptable = true;
        for (int i = 1; i <= M; i++)
        {
            satisfied[i - 1] = false;
            for (int j = 1; j <= N; j++)
            {
                if (solution[j - 1] == table[i, j])
                {
                    satisfied[i - 1] = true;
                    break;
                }
            }
            if (!satisfied[i - 1]) isAcceptable = false;
        }

        return isAcceptable;

    }
}
