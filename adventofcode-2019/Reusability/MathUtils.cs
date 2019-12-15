using System;
public static class MathUtils
{
    public static long GCD(long a, long b)
    {
        a = Math.Abs(a);
        b = Math.Abs(b);

        for (; ; )
        {
            long remainder = a % b;
            if (remainder == 0) return b;
            a = b;
            b = remainder;
        };
    }

    public static long LCM(long a, long b)
    {
        return a * b / GCD(a, b);
    }
}