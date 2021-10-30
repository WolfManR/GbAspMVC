using System;

namespace MiniThreadPoolProject
{
    public interface IMyThreadPool : IDisposable
    {
        int Count { get; }

        IMultiThreadWorkCallback QueueTask(Action task);
    }
}