using System;
using System.IO;
using System.Collections.Generic;

public class Solver
{
    public static void Main()
    {
        new Portal().run();
    }
}

public class Portal
{
    private int N;

    StreamReader input;
    StreamWriter output;

    private int R, C;
    private char[,] map;

    private int start_R, start_C;
    private int end_R, end_C;

    private int[,] dist;
    private bool[,] visited;

    public Portal()
    {
        input = new StreamReader("dir/2008/Round3/B/B-large.in");
        output = new StreamWriter("dir/2008/Round3/B/B-large.out");

        N = int.Parse(input.ReadLine());
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

    private void read()
    {
        string[] dimens = input.ReadLine().Split(' ');

        this.R = int.Parse(dimens[0]);
        this.C = int.Parse(dimens[1]);

        map = new char[R + 2, C + 2];

        for (int i = 0; i < R + 2; i++)
        {
            for (int j = 0; j < C + 2; j++)
            {
                map[i, j] = '#';
            }
        }

        for (int i = 1; i <= R; i++)
        {
            string line = input.ReadLine();

            for (int j = 1; j <= C; j++)
            {
                map[i, j] = line[j - 1];
            }
        }
    }

    private void solve()
    {
        init();
        search();
    }

    private void write(int C)
    {
        int sol = dist[end_R, end_C];

        if (sol != -1)
            output.WriteLine("Case #{0}: {1}", C, sol);
        else
            output.WriteLine("Case #{0}: THE CAKE IS A LIE", C);
    }

    private void init()
    {
        dist = new int[R + 2, C + 2];
        visited = new bool[R + 2, C + 2];

        for (int i = 0; i < R + 2; i++)
        {
            for (int j = 0; j < C + 2; j++)
            {
                dist[i, j] = -1;

                if (map[i, j] == 'O')
                {
                    start_R = i;
                    start_C = j;
                }

                if (map[i, j] == 'X')
                {
                    end_R = i;
                    end_C = j;
                }
            }
        }
    }

    private void search()
    {
        List<Coordinate> openedNodes = new List<Coordinate>();
        openedNodes.Add(new Coordinate(start_R, start_C));

        dist[start_R, start_C] = 0;

        while (openedNodes.Count > 0)
        {
            int nodeWithMinDistance = idxOfMinDistNode(openedNodes);
            

            Coordinate c = openedNodes[nodeWithMinDistance];
            openedNodes.RemoveAt(nodeWithMinDistance);

            int currentDist = dist[c.row, c.column];

            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    if (i == 0 ^ j == 0)
                    {
                        int nextR = c.row + i;
                        int nextC = c.column + j;

                        if (map[nextR, nextC] == '.' || map[nextR, nextC] == 'X')
                        {
                            if (dist[nextR, nextC] == -1 || dist[nextR, nextC] > currentDist + 1)
                                dist[nextR, nextC] = currentDist + 1;

                            if (!visited[nextR, nextC])
                            {
                                Coordinate node = new Coordinate(nextR, nextC);
                                if (!openedNodes.Contains(node))
                                    openedNodes.Add(new Coordinate(nextR, nextC));
                            }
                        }
                    }
                }
            }

            //this is section for walls in point that is not surounded with walls
            List<Coordinate> walls = this.getEdges(c.row, c.column);

            int closest = 0;
            Coordinate w = walls[closest];
            int wallDist = distance(c, w);

            for (int idx = 1; idx < walls.Count; idx++)
            {
                Coordinate curr = walls[idx];
                int v = distance(c, curr);
                if (v < wallDist)
                {
                    wallDist = v;
                    closest = idx;
                }
            }

            w = walls[closest];

            

            foreach (Coordinate coord in walls)
            {
                if (!coord.Equals(w))
                {
                    if (dist[coord.row, coord.column] == -1
                        || currentDist + wallDist + 1 < dist[coord.row, coord.column])
                    {
                        dist[coord.row, coord.column] = currentDist + wallDist + 1;
                        openedNodes.Add(coord);
                    }
                }
            }

            visited[c.row, c.column] = true;
        }

    }

    private int minDistForCoordinate(Coordinate c)
    {
        return dist[c.row, c.column];
    }

    private int idxOfMinDistNode(List<Coordinate> coords)
    {
        int minDistIdx = 0;
        int minDist = minDistForCoordinate(coords[minDistIdx]);

        for (int i = 0; i < coords.Count; i++)
        {
            int v = minDistForCoordinate(coords[i]);
            if(minDist > v)
            {
                minDistIdx = i;
                minDist = v;
            }
        }

        return minDistIdx;
    }

    private List<Coordinate> getEdges(int r, int c)
    {

        List<Coordinate> walls = new List<Coordinate>();

        for (int i = -1; i <= 1; i++)
        {
            for (int j = -1; j <= 1; j++)
            {
                if (i == 0 ^ j == 0)
                {
                    int k=0;

                    int lastR = 0;
                    int lastC = 0;

                    while (map[r + i * k, c + j * k] != '#')
                    {
                        lastR = r + i * k;
                        lastC = c + j * k;
                        k++;
                    }

                    walls.Add(new Coordinate(lastR, lastC));
                }
            }
        }


        return walls;
    }

    private int distance(Coordinate c1, Coordinate c2)
    {
        return Math.Abs(c1.row - c2.row) + Math.Abs(c1.column - c2.column);
    }
}

public class Coordinate
{
    public int row;
    public int column;

    public Coordinate(int row, int column)
    {
        this.row = row;
        this.column = column;
    }

    public override bool Equals(object obj)
    {
        Coordinate c = (Coordinate)obj;
        return (c.row == this.row) && (c.column == this.column);
    }
}