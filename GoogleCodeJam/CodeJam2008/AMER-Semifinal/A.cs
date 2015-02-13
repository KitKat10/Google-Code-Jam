using System;
using System.IO;
using System.Collections.Generic;

public class Solver
{
    public static void Main()
    {
        new MixingBowls().run();
    }
}

public class MixingBowls
{
    int C;
    int N;

    StreamReader input;
    StreamWriter output;

    Node root;
    Dictionary<string, Node> map;
    int sol;

    public MixingBowls()
    {
        input = new StreamReader("dir/2008/AMER-Semifinal/A/A-large.in");
        output = new StreamWriter("dir/2008/AMER-Semifinal/A/A-large.out");

        C = int.Parse(input.ReadLine());

        map = new Dictionary<string, Node>();
        root = new Node();
    }

    private void read()
    {
        N = int.Parse(input.ReadLine());

        map.Clear();
        root.size = 0;

        for (int i = 0; i < N; i++)
        {
            string[] p = input.ReadLine().Split(' ');
            if (i == 0)
            {
                root.name = p[0];
                map[p[0]] = root;
                int M = int.Parse(p[1]) + 2;
                for (int j = 2; j < M; j++)
                {
                    if (char.IsUpper(p[j][0]))
                    {
                        Node n = new Node();
                        n.name = p[j];
                        map[p[j]] = n;
                        root.children[root.size++] = n;
                    }
                }
            }
            else
            {
                Node n = findNode(p[0]);
                int M = int.Parse(p[1]) + 2;
                for (int j = 2; j < M; j++)
                {
                    if (char.IsUpper(p[j][0]))
                    {
                        Node ch = findNode(p[j]);
                        n.children[n.size++] = ch;
                    }
                }
            }
        }
    }

    private Node findNode(string name)
    {
        Node node = null;
        if (map.ContainsKey(name))
            node = map[name];
        else
        {
            node = new Node();
            node.name = name;
            map[name] = node;
        }
        return node;
    }

    private int getNumberOfBowls(Node n)
    {
        int numOfChild = n.size;
        if (numOfChild == 0)
            return 1;

        int[] bowls = new int[numOfChild];

        for(int i=0;i<numOfChild;i++)
        {
            bowls[i] = getNumberOfBowls(n.children[i]);
        }

        Array.Sort(bowls);

        int max = 0;
        for(int i=1;i<=numOfChild;i++)
        {
            int nec = i + bowls[numOfChild-i] - 1;
            if (nec > max)
                max = nec;
        }

        return
            Math.Max(max, numOfChild + 1);
    }

    private void solve()
    {
        sol = getNumberOfBowls(root);
    }

    private void write(int I)
    {
        output.WriteLine("Case #{0}: {1}", I, sol);
    }

    public void run()
    {
        for (int I = 0; I < C; I++)
        {
            read();
            solve();
            write(I + 1);
        }

        input.Close();
        output.Close();
    }
}

class Node
{
    public string name;
    public int size = 0;
    public Node[] children = new Node[10];
}