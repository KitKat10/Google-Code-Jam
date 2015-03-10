using System;
using System.IO;

public class Solver
{
    public static void Main()
    {
        new D().run();
    }
}

/**
 * See https://code.google.com/codejam/contest/32008/dashboard#s=a&a=3
 * */
public class D
{
    int N;

    StreamReader input;
    StreamWriter output;

    int R, C;
    int KR, KC;
    char[,] board = new char[15, 15];
    int[, ,] memo = new int[15, 15, 1 << 16];//memoization
    char winner;

    public D()
    {
        input = new StreamReader("dir/2008/AMER-Semifinal/D/D-large.in");
        output = new StreamWriter("dir/2008/AMER-Semifinal/D/D-large.out");

        N = int.Parse(input.ReadLine());
    }

    private void read()
    {
        string[] l = input.ReadLine().Split(' ');
        R = int.Parse(l[0]);
        C = int.Parse(l[1]);

        for (int i = 0; i < R; i++)
        {
            string row = input.ReadLine();
            for (int j = 0; j < C; j++)
            {
                board[i, j] = row[j];
                if (row[j] == 'K')
                {
                    KR = i;
                    KC = j;
                }
            }
        }
    }

    private void init()
    {
        for (int i = 0; i < R; i++)
            for (int j = 0; j < C; j++)
                for (int k = 0; k < 1 << (C + 1); k++)
                        memo[i, j, k] = -1;
    }

    /**
     * Method that uses dynamic programming to solve problem:
     * nx - column of node being procesed
     * ny - row of node being procesed
     * p - mask which tells which nodes can be matched with current node. 
     **/
    private int findMaximumMatching(int nx, int ny, int p)
    {
        if (nx == C)
        {
            nx = 0;
            ny++;
            if (ny == R)
                return 0;
        }

        int ret = memo[ny, nx, p];
        if (ret != -1)
            return ret;

        int pn = (p << 1) & ((1 << (C + 1)) - 1);

        if (board[ny, nx] != '.')
            ret = findMaximumMatching(nx + 1, ny, pn);
        else
        {
            ret = findMaximumMatching(nx + 1, ny, pn + 1);//trying to match current node with next node

            if (nx >= 1 && (p & 1) != 0)//case where current node is matched with left neighboring node
                ret = Math.Max(ret, 1 + findMaximumMatching(nx + 1, ny, pn - 2));
            if (nx >= 1 && (p & (1 << C)) != 0)//case where current node is matched with upper left neighboring node
                ret = Math.Max(ret, 1 + findMaximumMatching(nx + 1, ny, pn));
            if ((p & (1 << (C - 1))) != 0)//case where current node is matched with upper neighboring node
                ret = Math.Max(ret, 1 + findMaximumMatching(nx + 1, ny, pn - (1 << C)));
            if (nx < C - 1 && (p & (1 << (C - 2))) != 0)//case where current node is matched with upper right node
                ret = Math.Max(ret, 1 + findMaximumMatching(nx + 1, ny, pn - (1 << (C - 1))));
        }

        memo[ny, nx, p] = ret;
        return ret;
    }

    private void solve()
    {
        init();
        int maxm1 = findMaximumMatching(0, 0, 0);
        board[KR, KC] = '.';
        init();
        int maxm2 = findMaximumMatching(0, 0, 0);
        winner = (maxm2 > maxm1) ? 'A' : 'B';
    }

    private void write(int I)
    {
        output.WriteLine("Case #{0}: {1}", I, winner);
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
