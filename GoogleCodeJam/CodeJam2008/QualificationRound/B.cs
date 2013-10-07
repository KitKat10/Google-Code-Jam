using System;
using System.IO;
using System.Collections.Generic;

public class Solver
{

    public static void Main()
    {
        new TimeTable().run();
    }
}

public class TimeTable
{
    private int N;

    private int T;
    private int NA;
    private int NB;
    List<Train> trains;

    private int minA;
    private int minB;

    StreamReader input;
    StreamWriter output;


    public TimeTable()
    {
        input = new StreamReader("dir/2008/QualificationRound/B/B-large.in");
        output = new StreamWriter("dir/2008/QualificationRound/B/B-large.out");

        trains = new List<Train>();
    }

    public void run()
    {
        this.N = int.Parse(input.ReadLine());

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
        T = int.Parse(input.ReadLine());
        string[] trainNumbers = input.ReadLine().Split(' ');
        NA = int.Parse(trainNumbers[0]); NB = int.Parse(trainNumbers[1]);
        trains.Clear();

        for (int i = 0; i < NA + NB; i++)
        {
            string[] t = input.ReadLine().Split(new char[]{' ', ':'});
            Train train = new Train();
            train.start = int.Parse(t[0]) * 60 + int.Parse(t[1]);
            train.end = int.Parse(t[2]) * 60 + int.Parse(t[3]);
            train.station = (i < NA) ? 'A' : 'B';

            trains.Add(train);
        }
    }

    private void solve()
    {
        IComparer<Train> trainSorter = new Sorter();
        trains.Sort(trainSorter);
        minA = 0; minB = 0;

        while (trains.Count > 0)
        {
            int currentStart = trains[0].start;
            int currentEnd = trains[0].end;
            char currentStation = trains[0].station;

            trains.RemoveAt(0);
            addTrain(currentStation);

            for (int i = 0; i < trains.Count; i++)
            {
                if (currentStation!=trains[i].station && currentEnd + T <= trains[i].start)
                {
                    currentStart = trains[i].start;
                    currentEnd = trains[i].end;
                    currentStation = trains[i].station;

                    trains.RemoveAt(i); i--;
                }
            }
        }
    }

    private void write(int I)
    {
        output.WriteLine("Case #{0}: {1} {2}", I, minA, minB);
    }

    private void addTrain(char station)
    {
        if(station=='A') minA++;
        else minB++;
    }

    private struct Train
    {
        public int start;
        public int end;
        public char station;
    }

    private class Sorter : IComparer<Train>
    {
        public int Compare(Train x, Train y)
        {
            if (x.start < y.start) return -1;
            else if (x.start > y.start) return 1;
            else return 0;
        }
    }
}