using System;
using System.IO;

public class Solver
{
    public static void Main()
    {
        new Apocalypse().run();
    }
}

public class Apocalypse
{
    private int T;

    StreamReader input;
    StreamWriter output;

    private int C, R, c, r;
    private int[,] S;

    private int depth;
    private int[] dr = { -1, 0, 0, 1 };
    private int[] dc = { 0, -1, 1, 0 };


    public Apocalypse()
    {
        input = new StreamReader("dir/2008/APAC-Semifinal/B/B-large.in");
        output = new StreamWriter("dir/2008/APAC-Semifinal/B/B-large.out");

        T = int.Parse(input.ReadLine());
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
        string[] line1 = input.ReadLine().Split(' ');
        C = int.Parse(line1[0]);
        R = int.Parse(line1[1]);
        c = int.Parse(line1[2])-1;
        r = int.Parse(line1[3])-1;

        S = new int[R, C];

        for (int i = 0; i < R; i++)
        {
            string[] l = input.ReadLine().Split();
            for (int j = 0; j < C; j++)
            {
                S[i, j] = int.Parse(l[j]);
            }
        }
    }

    private void solve()
    {
        depth = 0;
        search(depth, S);
    }

    private void search(int d, int[,] state)
    {
        if (state[r, c] <= 0)
            return;

        if (depth < d)
            depth = d;

        int nr, nc;

        bool areAdjacentDestroyed = true;
        for (int i = 0; i < 4; i++)
        {
            nr = r + dr[i];
            nc = c + dc[i];
            if (nr < 0 || nc < 0 || nr >= R || nc >= C)
                continue;

            areAdjacentDestroyed &= (state[nr, nc] <= 0);
        }

        if (areAdjacentDestroyed)
        {
            this.depth = int.MaxValue;
            return;
        }

        int[,] copy = new int[R, C];
        for (int i = 0; i < R; i++)
        {
            for (int j = 0; j < C; j++)
                copy[i, j] = state[i, j];
        }

        for (int i = 0; i < R; i++)
        {
            for (int j = 0; j < C; j++)
            {
                if ((i == r && j == c) || state[i, j] == 0)
                    continue;

                int max = 0;
                int max_k = -1;
                for (int k = 0; k < 4; k++)
                {
                    nr = i + dr[k];
                    nc = j + dc[k];

                    if (nr < 0 || nc < 0 || nr >= R || nc >= C)
                        continue;

                    if (state[nr, nc] > max)
                    {
                        max = state[nr, nc];
                        max_k = k;
                    }
                }

                if (max_k != -1 && state[i, j] > 0)
                    copy[i + dr[max_k], j + dc[max_k]] -= state[i, j];
            }
        }

        search(d + 1, copy);

        for (int k = 0; k < 4; k++)
        {
            nr = r + dr[k];
            nc = c + dc[k];
            if (nr < 0 || nc < 0 || nr >= R || nc >= C)
                continue;

            if (copy[nr, nc] > 0)
            {
                copy[nr, nc] -= state[r, c];
                search(d + 1, copy);
                copy[nr, nc] += state[r, c];
            }
        }
    }

    private void write(int C)
    {
        if (depth == int.MaxValue)
            output.WriteLine("Case #{0}: forever", C);
        else
            output.WriteLine("Case #{0}: {1} day(s)", C, depth);
    }
 }