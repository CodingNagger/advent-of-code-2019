using System;
using System.Diagnostics;

public static class DisplayUtils
{
    public static string DisplayValue(Stopwatch stopWatch)
    {
        TimeSpan ts = stopWatch.Elapsed;
        return String.Format("{0:00}:{1:00}:{2:00}.{3:000}",
            ts.Hours, ts.Minutes, ts.Seconds,
            ts.Milliseconds);
    }
}