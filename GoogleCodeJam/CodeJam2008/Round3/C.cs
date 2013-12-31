using System;
using System.IO;
using System.Collections.Generic;

public class Solver
{
    public static void Main()
    {
        new NoCheating().run();
    }
}

public class NoCheating
{
    private int C;

    StreamReader input;
    StreamWriter output;

    private int M;
    private int N;

    private char[,] map;

    private int[,] nbr, nbc;
    private bool[,] visited = new bool[100, 100];

    private int max;

    public NoCheating()
    {
        input = new StreamReader("dir/2008/Round3/C/C-large.in");
        output = new StreamWriter("dir/2008/Round3/C/C-large.out");

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
        int[] dims = Array.ConvertAll<string, int>(input.ReadLine().Split(' '), int.Parse);
        this.M = dims[0];
        this.N = dims[1];

        map = new char[M, N];
        for (int i = 0; i < M; i++)
        {
            string row = input.ReadLine();
            for (int j = 0; j < N; j++)
            {
                map[i, j] = row[j];
            }
        }
    }

    private void solve()
    {
        nbr = new int[M, N];
        nbc = new int[M, N];
        for (int i = 0; i < M; i++)
        {
            for (int j = 0; j < N; j++)
            {
                nbr[i, j] = -1;
                nbc[i, j] = -1;
            }
        }


        this.max = 0;
        for (int i = 0; i < M; i++)
        {
            for (int j = 0; j < N; j++)
            {
                if (map[i, j] == '.')
                {
                    visited = new bool[M, N];
                    max++;//count every space
                    if (j % 2 == 0 && bpm(i, j))
                        max--;//if there is established conncection,one space has to be free
                }
            }
        }
    }

    private void write(int C)
    {
        output.WriteLine("Case #{0}: {1}", C + 1, max);
    }

    /*Method checks whether node(squre) on (r,c) is possible to connect with other nodes and connects them if possible.*/
    private bool bpm(int r, int c)
    {
        if (r == -1)
            return true;//-1 is initial value
        if (visited[r, c])//if this node is already visited
            return false;

        visited[r, c] = true;

        for (int i = r - 1; i <= r + 1; i++)
        {
            for (int j = c - 1; j <= c + 1; j = j + 2)
            {
                if (i >= 0 && i < M && j >= 0 && j < N && map[i, j] == '.')
                {
                    if (bpm(nbr[i, j], nbc[i, j]))
                    {
                        nbr[i, j] = r;
                        nbc[i, j] = c;
                        nbr[r, c] = i;
                        nbc[r, c] = j;
                        return true;
                    }
                }
            }
        }

        return false;
    }
   
}
