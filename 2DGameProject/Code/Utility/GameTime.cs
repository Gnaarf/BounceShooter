using System;
using System.Diagnostics;

public class GameTime
{
    private Stopwatch watch;
    /// <summary>
    /// Time passed since GameStart
    /// </summary>
    public TimeSpan totalTime { get; private set; }
    /// <summary>
    /// Time passed since last Update
    /// </summary>
    public TimeSpan ellapsedTime { get; private set; }
    
    public GameTime()
    {
        watch = new Stopwatch();
        totalTime = TimeSpan.FromSeconds(0);
        ellapsedTime = TimeSpan.FromSeconds(0);
    }
    public void Start()
    {
        watch.Start();
    }
    public void Stop()
    {
        watch.Reset();
        totalTime = TimeSpan.FromSeconds(0);
        ellapsedTime = TimeSpan.FromSeconds(0);
    }
    public void Update()
    {
        ellapsedTime = watch.Elapsed - totalTime;
        totalTime = watch.Elapsed;
    }
}

