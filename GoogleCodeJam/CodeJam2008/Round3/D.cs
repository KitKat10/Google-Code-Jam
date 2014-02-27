using System;
using System.Collections.Generic;
using System.IO;

public class Solver
{
    public static void Main()
    {
        new EndlessKnight().run();
    }
}

public class EndlessKnight
{
    private int N;

    StreamReader input;
    StreamWriter output;

    private int H, W, R;

    private Coordinate[] stones;
    private CoordComparer _sc = new CoordComparer();

    private const int MOD = 10007;

    private int paths;

    private int[] factorials = new int[MOD];
    private int[] modInverses = new int[MOD];

    public EndlessKnight()
    {
        input = new StreamReader("dir/2008/Round3/D/D-large.in");
        output = new StreamWriter("dir/2008/Round3/D/D-large.out");

        this.N = int.Parse(input.ReadLine());
        stones = new Coordinate[10];
    }

    private void read()
    {
        string[] args = input.ReadLine().Split(' ');
        H = int.Parse(args[0]);
        W = int.Parse(args[1]);
        R = int.Parse(args[2]);

        for (int i = 0; i < R; i++)
        {
            args = input.ReadLine().Split(' ');
            stones[i].y = int.Parse(args[0]) - 1;
            stones[i].x = int.Parse(args[1]) - 1;
        }
    }

    public void run()
    {
        preCompute();
        for (int I = 0; I < N; I++)
        {
            read();
            solve();
            write(I + 1);
        }

        input.Close();
        output.Close();
    }

    //dynamic programming for small input
    private void solve_small()
    {
        int[,] dyn = new int[H, W];
        dyn[0, 0] = 1;

        for (int i = 0; i < R; i++)
        {
            dyn[stones[i].x, stones[i].y] = -1;
        }

        for (int i = 0; i < H; i++)
        {
            for (int j = 0; j < W; j++)
            {
                for (int k = 0; k <= 1; k++)
                {
                    int x = i - 1 - k;
                    int y = j - 2 + k;
                    if (!(x < 0 || x >= H || y < 0 || y >= W) && dyn[x, y] != -1 && dyn[i, j] != -1)
                    {
                        dyn[i, j] += dyn[x, y];
                        dyn[i, j] %= MOD;
                    }
                }

            }
        }

        this.paths = dyn[H - 1, W - 1];
    }

    //solves large input
    private void solve()
    {
        Array.Sort(stones, 0, R, _sc);

        int stoneConf = (R == 0) ? 0 : 1 << R;

        int h = H - 1, w = W - 1;
        this.paths = 0;
        if (!pathDecomposition(ref h, ref w))
            return;

        int ans = choose(h + w, h);

        Coordinate pos = new Coordinate();
        for (int C = 1; C < stoneConf; C++)
        {
            int currentPath = 1;
            pos.x = 0; pos.y = 0;
            for (int j = 0; j < R; j++)
            {
                if ((C & (1 << j)) != 0)
                {
                    int m = stones[j].x - pos.x;
                    int n = stones[j].y - pos.y;

                    if (pathDecomposition(ref m, ref n))
                    {
                        currentPath *= choose(m + n, n);
                        currentPath = modulo(currentPath);
                        pos.x = stones[j].x; pos.y = stones[j].y;
                    }
                    else
                    {
                        currentPath = 0;
                        break;
                    }
                }
            }

            if (currentPath != 0)
            {
                int lm = W - pos.x - 1;
                int ln = H - pos.y - 1;

                if (pathDecomposition(ref lm, ref ln))
                {
                    currentPath *= choose(lm + ln, ln);
                    currentPath = modulo(currentPath);
                }
                else
                    currentPath = 0;
            }

            ans += calculateParity(C) * currentPath;
        }

        this.paths = modulo(ans);
    }

    private void write(int C)
    {
        output.WriteLine("Case #{0}: {1}", C, paths);
    }

    private int calculateParity(int conf)
    {
        int p = 1;
        for (int i = 0; i < R; i++)
        {
            int r = (1 << i) & conf;
            if (r != 0)
                p *= -1;
        }
        return p;
    }

    private bool pathDecomposition(ref int m, ref int n)
    {
        int s = m - n;
        int a = (s + m);
        int b = (n - s);
        if (a < 0 || b < 0)
            return false;
        else if (a % 3 != 0 || b % 3 != 0)
            return false;
        else
        {
            a /= 3;
            b /= 3;
            m = a;
            n = b;
            return true;
        }
    }

    private int modulo(int x)
    {
        x %= MOD;
        if (x < 0)
            x += MOD;
        return x;
    }

    private void preCompute()
    {
        factorials[0] = 1;
        for (int i = 1; i < MOD; i++)
        {
            factorials[i] = (i * factorials[i - 1]) % MOD;
            modInverses[i] = modInverse(i);
        }
    }

    private int modInverse(int x)
    {
        if (x == 1)
            return 1;

        int m = MOD;
        int q = x;
        int r = m % q;

        int u0 = 0, u1 = 1, u2 = 0;

        while (q != 1)
        {
            u2 = u0 - (m / q) * u1;
            u0 = u1;
            u1 = u2;

            m = q;
            q = r;
            r = m % q;
        }

        return modulo(u2);
    }

    private int choose(int n, int k)
    {
        int result = 1;
        int m = k > n - k ? k : n - k;
        int mk = n - m;

        int f1 = primeFactorInFactorial(n);
        int f2 = primeFactorInFactorial(m);
        int f3 = primeFactorInFactorial(mk);

        if (f1 > f2 + f3)
            return 0;

        result *= factorialWithoutPrimeMod(n);
        result %= MOD;

        result *= modInverses[factorialWithoutPrimeMod(m)];
        result %= MOD;

        result *= modInverses[factorialWithoutPrimeMod(mk)];
        result %= MOD;

        return result;
    }

    //Legendre's theorem
    private int primeFactorInFactorial(int n)
    {
        int p = 0;
        while (n > 0)
        {
            n /= MOD;
            p += n;
        }
        return p;
    }

    //Wilson's theorem
    private int factorialWithoutPrimeMod(int n)
    {
        if (n < MOD)
            return factorials[n];
        else
        {
            int m = n / MOD;
            int result = ((factorials[n % MOD]) * factorialWithoutPrimeMod(m)) % MOD;
            if (m % 2 == 1)
                result = (MOD - result) % MOD;
            return result;
        }
    }
}

struct Coordinate
{
    public int x;
    public int y;
}

class CoordComparer : IComparer<Coordinate>
{
    public int Compare(Coordinate f, Coordinate s)
    {
        if (f.x > s.x)
            return 1;
        else if (f.x < s.x)
            return -1;
        else
        {
            return f.y - s.y;
        }
    }
}