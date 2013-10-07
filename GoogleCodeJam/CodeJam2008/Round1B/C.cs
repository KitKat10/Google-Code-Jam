using System;
using System.IO;

public class Solver
{
    public static void Main()
    {
        new Mousetrap().run();
    }
}

public class Mousetrap
{
    private int T;

    private int K;
    private int[] cards;

    private BinaryTree tree;
    private int[] deck;

    StreamReader input;
    StreamWriter output;

    public Mousetrap()
    {
        input = new StreamReader("dir/2008/Round1B/C/C-large.in");
        output = new StreamWriter("dir/2008/Round1B/C/C-large.out");

        this.T = int.Parse(input.ReadLine());
    }

    public void run()
    {
        for (int I = 0; I < T; I++)
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
        K = int.Parse(input.ReadLine());
        string[] ind = input.ReadLine().Split(' ');

        int n = int.Parse(ind[0]);
        cards = new int[n];

        for (int i = 1; i <= n; i++) cards[i - 1] = int.Parse(ind[i]);
    }

    private void solve()
    {
        int p = 0;
        int idx = 0;

        deck = new int[K];
        tree = new BinaryTree(K + 1);

        for (int i = 0; i <= K; i++) tree.update(1, i + 1);

        for (int i = 0; i < K; i++)
        {
            p = p + i;
            p %= (K - i);
            idx = tree.binarySearch(p + 1);
            idx = adapt(idx);
            deck[idx] = i + 1;
            tree.update(-1, idx + 1);
        }

        for (int i = 0; i < cards.Length; i++)
        {
            cards[i] = deck[cards[i] - 1];
        }

    }

    private void write(int I)
    {
        output.Write("Case #{0}:", I + 1);
        for (int i = 0; i < cards.Length; i++)
        {
            output.Write(" {0}", cards[i]);
        }
        output.WriteLine();
    }

    private int adapt(int idx)
    {
        while (deck[idx - 1] != 0) idx--;
        return idx - 1;
    }
}

public class BinaryTree
{
    private int N;
    private int[] tree;
 
    public BinaryTree(int N)
    {
        this.N = N;
        tree = new int[N + 1];
    }

    public int readCumulativeF(int idx)
    {
        int sum = 0;

        while (idx > 0)
        {
            sum += tree[idx];
            idx -= idx & (-idx);
        }

        return sum;
    }

    public void update(int v, int idx)
    {
        while (idx <= N)
        {
            tree[idx] += v;
            idx += idx & (-idx);
        }
    }

    public int binarySearch(int cf)
    {
        int l = 1; int r = N;
        int m = (l + r) / 2;
        int c = this.readCumulativeF(m);

        while (c != cf)
        {
            if (c > cf) r = m;
            else if (c < cf) l = m;
            
            m = (l + r) / 2;
            c = readCumulativeF(m);
        }

        return m;
    }
}