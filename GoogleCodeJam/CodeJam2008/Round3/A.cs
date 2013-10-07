using System;
using System.IO;
using System.Collections.Generic;

public class Solver
{
    public static void Main()
    {
        new PocketArea().run();
    }
}

public class PocketArea
{
    private int N;

    StreamReader input;
    StreamWriter output;

    private int L;
    private string[] steps;

    private int pockets;

    private List<int>[] verticalLines;
    private int[,] extremes;

    private const int SIZE = 6001;
    private const int MAX = 0;
    private const int MIN = 1;

    public PocketArea()
    {
        input = new StreamReader("dir/2008/Round3/A/A-large.in");
        output = new StreamWriter("dir/2008/Round3/A/A-large.out");

        this.N = int.Parse(input.ReadLine());
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

    private void read()
    {
        L = int.Parse(input.ReadLine());

        int i = 0;
        steps = new string[2 * L];
        while (i < L)
        {
            string[] part = input.ReadLine().Split(' ');
            part.CopyTo(steps, 2 * i);

            i += (part.Length / 2);
        }
    }

    private void solve()
    {
        Orientation or = Orientation.UP;
        int x = 3000;
        int y = 3000;

        resetMap();

        for(int i=0;i<L;i++)
        {
            string moves = steps[2 * i];
            int rep = int.Parse(steps[2 * i + 1]);

            for (int j = 0; j < rep; j++)
            {
                for (int k = 0; k < moves.Length; k++)
                {
                    char c = moves[k];

                    if (c == 'L')
                    {
                        if (or == Orientation.UP)
                            or = Orientation.LEFT;
                        else if (or == Orientation.RIGHT)
                            or = Orientation.UP;
                        else if (or == Orientation.DOWN)
                            or = Orientation.RIGHT;
                        else
                            or = Orientation.DOWN;
                    }
                    else if (c == 'R')
                    {
                        if (or == Orientation.UP)
                            or = Orientation.RIGHT;
                        else if (or == Orientation.RIGHT)
                            or = Orientation.DOWN;
                        else if (or == Orientation.LEFT)
                            or = Orientation.UP;
                        else
                            or = Orientation.LEFT;
                    }
                    else if (c == 'F')
                    {
                        if (or == Orientation.UP)
                            y++;
                        else if (or == Orientation.LEFT)
                        {
                            x--;
                            addLine(x, y);
                        }
                        else if (or == Orientation.RIGHT)
                        {
                            addLine(x, y);
                            x++;
                        }
                        else if(or == Orientation.DOWN)
                            y--;
                    }
                }
            }
        }

        resetExtremes();
        int A = 0;

        for (int i = 0; i < SIZE; i++)
        {
            if (verticalLines[i] != null)
            {
                verticalLines[i].Sort();
                for (int j = 0; j < verticalLines[i].Count; j = j + 2)
                {
                    A += verticalLines[i][j + 1] - verticalLines[i][j]; 
                }

                int len = verticalLines[i].Count;
                if (len > 0)
                {
                    extremes[MIN, i] = verticalLines[i][0];
                    extremes[MAX, i] = verticalLines[i][len - 1];
                }
            }
        }

        int idxOfHighPoint = findHighPoint();
        int idxOfLowPoint = findLowPoint();

        
        int level = 0;
        for (int i = 0; i <= idxOfHighPoint; i++)
        {
            if (containsLine(i))
            {
                if (extremes[MAX, i] > level)
                    level = extremes[MAX, i];
                else
                    extremes[MAX, i] = level;
            }
        }

        level = 0;
        for (int i = SIZE - 1; i >= idxOfHighPoint; i--)
        {
            if (containsLine(i))
            {
                if (extremes[MAX, i] > level)
                    level = extremes[MAX, i];
                else
                    extremes[MAX, i] = level;
            }
        }


        level = SIZE;
        for (int i = 0; i <= idxOfLowPoint; i++)
        {
            if (containsLine(i))
            {
                if (extremes[MIN, i] < level)
                    level = extremes[MIN, i];
                else
                    extremes[MIN, i] = level;
            }
        }

        level = SIZE;
        for (int i = SIZE - 1; i >= idxOfLowPoint; i--)
        {
            if (containsLine(i))
            {
                if (extremes[MIN, i] < level)
                    level = extremes[MIN, i];
                else
                    extremes[MIN, i] = level;
            }
        }

        int maxArea = 0;

        for (int i = 0; i < SIZE; i++)
        {
            maxArea += (extremes[MAX, i] - extremes[MIN, i]);
        }

        pockets = maxArea - A; 
    }

    private void write(int C)
    {
        output.WriteLine("Case #{0}: {1}", C + 1, pockets);
    }

    private void resetMap()
    {
        if (verticalLines == null)
            verticalLines = new List<int>[SIZE];

        for (int i = 0; i < SIZE; i++)
        {
            if (verticalLines[i] != null)
                verticalLines[i].Clear();
        }
    }


    /// <summary>
    /// Method ads line
    /// </summary>
    /// <param name="x">starting point of line</param>
    /// <param name="y">height</param>
    private void addLine(int x, int y)
    {
        if (verticalLines[x] == null)
            verticalLines[x] = new List<int>();

        verticalLines[x].Add(y);
    }

    private bool containsLine(int idx)
    {
        if (verticalLines[idx] == null || verticalLines[idx].Count == 0)
            return false;
        else
            return true;
    }

    private void resetExtremes()
    {
        if (extremes == null)
        {
            extremes = new int[2, SIZE];
        }

        for (int i = 0; i < SIZE; i++)
        {
            extremes[MIN, i] = 3000;
            extremes[MAX, i] = 3000;
        }
    }

    private int findHighPoint()
    {
        int idx = 0; int h = 0;
        for (int i = 0; i < SIZE; i++)
        {
            if (containsLine(i))
            {
                if (extremes[MAX, i] > h)
                {
                    h = extremes[MAX, i];
                    idx = i;
                }
            }
        }

        return idx;
    }

    private int findLowPoint()
    {
        int idx = 0; int l = SIZE;
        for (int i = 0; i < SIZE; i++)
        {
            if (containsLine(i))
            {
                if (l > extremes[MIN, i])
                {
                    l = extremes[MIN, i];
                    idx = i;
                }
            }
        }

        return idx;
    }

    private enum Orientation
    {
        UP, DOWN, LEFT, RIGHT
    }
}