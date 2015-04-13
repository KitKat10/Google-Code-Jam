using System;
using System.IO;
using System.Collections.Generic;

public class Solver
{
    public static void Main()
    {
        new B().run();
    }
}

public class B
{
    int T;

    StreamReader input;
    StreamWriter output;

    int N;
    Dictionary<string, List<Offer>> offersDict = new Dictionary<string, List<Offer>>();
    int minimum;

    OfferComparer oc = new OfferComparer();

    public B()
    {
        input = new StreamReader("dir/2008/EMEA-Semifinal/B/B-large.in");
        output = new StreamWriter("dir/2008/EMEA-Semifinal/B/B-large.out");

        T = int.Parse(input.ReadLine());
    }

    private void read()
    {
        N = int.Parse(input.ReadLine());
        offersDict.Clear();
        for (int i = 0; i < N; i++)
        {
            string[] l = input.ReadLine().Split(' ');
            Offer o;
            o.color = l[0];
            o.start = int.Parse(l[1]);
            o.end = int.Parse(l[2]);

            if(!offersDict.ContainsKey(o.color))
            {
                List<Offer> list = new List<Offer>();
                list.Add(o);
                offersDict[o.color] = list;
            }
            else
            {
                offersDict[o.color].Add(o);
            }
        }
    }

    private void solve()
    {
        int colCount = offersDict.Count;
        string[] colors = new string[offersDict.Count];
        offersDict.Keys.CopyTo(colors, 0);

        minimum = -1;
        if (colCount < 3)
        {
            List<Offer> off = new List<Offer>();

            foreach(string s in offersDict.Keys)
            {
                off.AddRange(offersDict[s]);
            }
            off.Sort(oc);
            minimum = minimal(off);
        }
        else
        {
            List<Offer> colorOffers = null;
            for (int i = 0; i < colors.Length - 2; i++)
                for (int j = i + 1; j < colors.Length - 1; j++)
                    for (int k = j + 1; k < colors.Length; k++)
                    {
                        colorOffers = new List<Offer>(offersDict[colors[i]]);
                        colorOffers.AddRange(offersDict[colors[j]]);
                        colorOffers.AddRange(offersDict[colors[k]]);
                        colorOffers.Sort(oc);
                        int m = minimal(colorOffers);
                        minimum = (m != -1 && (minimum > m || minimum == -1)) ? m : minimum;
                    }
        }
    }

    private int minimal(List<Offer> offerSubset)
    {
        int max = 0;
        int count = 0;

        if (offerSubset.Count == 0 || offerSubset[0].start != 1)
            return -1;

        int i = 0;
        while (i < offerSubset.Count && max < 10000)
        {
            int j=i;
            int currentMax = 0;
            while(j<offerSubset.Count && offerSubset[j].start <= max + 1 && max < 10000)
            {
                if (offerSubset[j].end > currentMax)
                    currentMax = offerSubset[j].end;
                j++;
            }

            if (i == j)
                return -1;
            max = currentMax;
            count++;
            i = j;
        }

        if (max == 10000)
            return count;
        else return -1;
    }

    public void write(int I)
    {
        if (minimum == -1)
            output.WriteLine("Case #{0}: IMPOSSIBLE", I);
        else
            output.WriteLine("Case #{0}: {1}", I, minimum);
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
}

struct Offer
{
    public string color;
    public int start;
    public int end;
}

class OfferComparer : IComparer<Offer>
{
    public int Compare(Offer x, Offer y)
    {
        if (x.start < y.start)
            return -1;
        else if (x.start > y.start)
            return 1;
        else
            return y.end - x.end;
    }
}
