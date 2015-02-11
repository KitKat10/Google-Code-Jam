using System;
using System.IO;
using System.Collections.Generic;

public class Solver
{
    public static void Main()
    {
        new Plagiarism().run();
    }
}

public class Plagiarism
{
    private const int SIZE = 100 + 5;

    private int C;
    private int M, N;

    StreamReader input;
    StreamWriter output;

    List<int>[] t1 = new List<int>[SIZE];
    List<int>[] t2 = new List<int>[SIZE];

    bool[,] bGraph;
    bool[] visited;
    int[] connected = new int[SIZE];
    int a, b;

    bool result;

    public Plagiarism()
    {
        input = new StreamReader("dir/2008/APAC-Semifinal/D/D-large.in");
        output = new StreamWriter("dir/2008/APAC-Semifinal/D/D-large.out");

        C = int.Parse(input.ReadLine());
    }

    private void init()
    {
        for (int i = 1; i < SIZE; i++)
        {
            t1[i] = new List<int>();
            t2[i] = new List<int>();
        }
    }

    public void run()
    {
        init();
        for (int I = 0; I < C; I++)
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
        for (int i = 1; i < SIZE; i++)
        {
            t1[i].Clear();
            t2[i].Clear();
        }

        int v1 = 0, v2 = 0;
        N = int.Parse(input.ReadLine());
        for (int i = 1; i < N; i++)
        {
            string[] l = input.ReadLine().Split(' ');
            v1=int.Parse(l[0]);
            v2=int.Parse(l[1]);
            t1[v1].Add(v2);
            t1[v2].Add(v1);
        }

        M = int.Parse(input.ReadLine());
        for (int i = 1; i < M; i++)
        {
            string[] l = input.ReadLine().Split(' ');
            v1 = int.Parse(l[0]);
            v2 = int.Parse(l[1]);
            t2[v1].Add(v2);
            t2[v2].Add(v1);
        }
    }

    private void solve()
    {
        bool isIsomorphic = false;

        for (int i = 1; i <= N & !isIsomorphic; i++)
        {
            isIsomorphic = matchSubtree(i, 0, 1, 0);
        }

        result = isIsomorphic;
    }

    public void write(int I)
    {
        output.WriteLine("Case #{0}: {1}", I, (result ? "YES" : "NO"));
    }

    
    private bool matchSubtree(int n1, int parent1, int n2, int parent2)
    {
        int s1 = 0;
        for (int i = 0; i < t1[n1].Count; i++)
            if (t1[n1][i] != parent1)
                s1++;

        int s2 = 0;
        for (int i = 0; i < t2[n2].Count; i++)
            if (t2[n2][i] != parent2)
                s2++;

        if (s2 == 0)
            return true;

        if (s1 < s2 || s1 == 0)
            return false;

        bool[,] pairs = new bool[s1, s2];

        int x = 0, y = 0;
        for (int i = 0; i < t1[n1].Count; i++)
        {
            if (t1[n1][i] == parent1)
                continue;

            y = 0;
            for (int j = 0; j < t2[n2].Count; j++)
            {
                if (t2[n2][j] == parent2)
                    continue;

                pairs[x, y] = matchSubtree(t1[n1][i], n1, t2[n2][j], n2);
                y++;
            }
            x++;
        }

        if (maxMatching(pairs, s1, s2) == s2)
            return true;
        else return false;
    }

    private int maxMatching(bool[,] pairs, int s1, int s2)
    {
        int max = 0;
        this.bGraph = pairs;
        this.a = s1;
        this.b = s2;

        for (int i = 0; i < s2; i++)
            connected[i] = -1;

        for (int i = 0; i < s1; i++)
        {
            visited = new bool[s1];
            max += DFS(i);
        }

        return max;
    }

    private int DFS(int x)
    {
        for (int i = 0; i < b; i++)
        {
            if (bGraph[x, i])
            {
                visited[x] = true;
                if (connected[i] == -1 || (!visited[connected[i]] && DFS(connected[i]) != 0))
                {
                    connected[i] = x;
                    return 1;
                }
            }
        }
        return 0;
    }
}