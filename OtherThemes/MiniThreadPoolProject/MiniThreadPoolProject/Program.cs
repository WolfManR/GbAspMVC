using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace MiniThreadPoolProject
{
    internal class Program
    {
        static void Main(string[] args)
        {
            using (IMyThreadPool threadPool = new MyThreadPool(4))
            {
                for (var i = 0; i < 5; i++)
                {
                    threadPool.QueueTask(() =>
                    {
                        Thread.Sleep(4000);
                        Console.WriteLine($"Hello from thread {Thread.CurrentThread.ManagedThreadId} with name {Thread.CurrentThread.Name}");
                    });
                }

                Thread.Sleep(10000);
                Console.WriteLine("Start disposing thread pool");
            }

            Console.WriteLine("Thread pool must be disposed");
        }
    }

    public class MyThreadPool : IMyThreadPool
    {
        private volatile Thread[] _threads;
        private readonly ConcurrentQueue<Action> _tasks;
        private readonly object _lock = new();
        private int _maxCountOfThreads;

        public MyThreadPool(int countOfThreads = 2, int maxCountOfThreads = 2)
        {
            if (countOfThreads < 1)
            {
                throw new ArgumentException("Count of initialized threads cannot be lower that 1", nameof(countOfThreads));
            }

            if (maxCountOfThreads < 1 || maxCountOfThreads < countOfThreads)
            {
                throw new ArgumentException("Maximum Count of threads cannot be lower than 1 and lower than Count of initialized threads", nameof(maxCountOfThreads));
            }

            _maxCountOfThreads = maxCountOfThreads;
            _threads = new Thread[countOfThreads];
            _tasks = new();

            for (var i = 0; i < countOfThreads; i++)
            {
                _threads[i] = new Thread(Consume) { IsBackground = true, Name = $"My Thread Pool thread No{i}" };
                _threads[i].Start();
            }
        }

        public void QueueTask(Action task)
        {
            lock (_lock)
            {
                if (_isDisposed) return;
                _tasks.Enqueue(task);
                Monitor.PulseAll(_lock);
            }
        }

        private void Consume(object arg)
        {
            while (true)
            {
                Action task;
                lock (_lock)
                {
                    while (_tasks.IsEmpty || _isDisposed || _isDisposing)
                    {
                        Monitor.Wait(_lock);
                        if (_isDisposed || _isDisposing) return;
                    }
                    
                    if (!_tasks.TryDequeue(out task)) continue;
                }
                if (_isDisposed || _isDisposing) return;
                task?.Invoke();
            }
        }

        private bool _isDisposing;
        private bool _isDisposed;

        protected virtual void Dispose(bool disposing)
        {
            if (_isDisposed) return;
            _isDisposing = true;
            if (disposing)
            {
                // TODO: dispose managed state (managed objects)
                lock (_lock)
                {
                    Monitor.PulseAll(_lock);
                }

                foreach (var thread in _threads)
                {
                    thread.Join();
                }
            }

            // TODO: free unmanaged resources (unmanaged objects) and override finalizer
            // TODO: set large fields to null
            _isDisposed = true;
        }

        // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        // ~MyThreadPool()
        // {
        //     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        //     Dispose(disposing: false);
        // }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }

    public interface IMyThreadPool : IDisposable
    {
        void QueueTask(Action task);
    }
}
