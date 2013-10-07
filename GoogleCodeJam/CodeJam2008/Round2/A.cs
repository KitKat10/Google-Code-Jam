using System;
using System.IO;

public class Solver
{
    public static void Main()
    {
        new BooleanTree().run();
    }
}

public class BooleanTree
{
    private int N;

    StreamReader input;
    StreamWriter output;

    private int V;
    private int M;
    private Node[] tree;

    private int[,] dyn;

    public BooleanTree()
    {
        input = new StreamReader("dir/2008/Round2/A/A-large.in");
        output = new StreamWriter("dir/2008/Round2/A/A-large.out");

        this.N = int.Parse(input.ReadLine());
    }

    private void read()
    {
        string[] treeParams = input.ReadLine().Split(' ');
        M = int.Parse(treeParams[0]);
        V = int.Parse(treeParams[1]);

        tree = new Node[M + 1];

        for (int i = 1; i <= (M - 1) / 2; i++)
        {
            Node n = new Node();
            string[] p = input.ReadLine().Split(' ');
            n.value = int.Parse(p[0]);
            n.isChangable = (int.Parse(p[1]) == 1);
            tree[i] = n;
        }

        for (int i = (M + 1) / 2; i <= M; i++)
        {
            Node n = new Node();
            n.isLeaf = true;
            n.value = int.Parse(input.ReadLine());
            tree[i] = n;
        }
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

    private void solve()
    {
        dyn = new int[2, M + 1];
        travel(1);
    }

    private void write(int c)
    {
        if (dyn[V, 1] == -1)
            output.WriteLine("Case #{0}: IMPOSSIBLE", c + 1);
        else
            output.WriteLine("Case #{0}: {1}", c + 1, dyn[V, 1]);
    }

    private void travel(int idx)
    {
        if (idx > M) return;
        travel(2 * idx);
        travel(2 * idx + 1);

        int v, A, B, C, D, MIN0 = 0, MIN1 = 0;

        if (tree[idx].isLeaf)
        {
            v = tree[idx].value;
            dyn[v, idx] = 0;

            if (v == 0)
                v = 1;
            else
                v = 0;

            dyn[v, idx] = -1;
        }
        else
        {
            A = dyn[0, 2 * idx]; B = dyn[0, 2 * idx + 1]; 
            C = dyn[1, 2 * idx]; D = dyn[1, 2 * idx + 1];

            if (tree[idx].value == 0 && tree[idx].isChangable)
            {
                MIN1 = minChanges(C, D);
                int c = minChanges(A, B);
                MIN0 = (c == -1) ? -1 : c + 1;
                if (A != -1 && B != -1) MIN0 = Math.Min(MIN0, A + B);
            }
            else if (tree[idx].value == 0 && !tree[idx].isChangable)
            {
                MIN1 = minChanges(C, D);
                if (A == -1 || B == -1) MIN0 = -1;
                else MIN0 = A + B;
            }
            else if (tree[idx].value == 1 && tree[idx].isChangable)
            {
                MIN0 = minChanges(A, B);
                int c = minChanges(C, D);
                MIN1 = (c == -1) ? -1 : c + 1;
                if (C != -1 && D != -1) MIN1 = Math.Min(MIN1, C + D);
            }
            else
            {
                MIN0 = minChanges(A, B);
                if (C == -1 || D == -1) MIN1 = -1;
                else MIN1 = C + D;
            }
            dyn[0, idx] = MIN0;
            dyn[1, idx] = MIN1;
        }

        
    }

    private int minChanges(int x, int y)
    {
        if (x != -1 && y != -1)
            return Math.Min(x, y);
        else if (x == -1)
            return y;
        else
            return x;
    }
}

public struct Node
{
    public int value;
    public bool isChangable;
    public bool isLeaf;
}