using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace MiniThreadPoolProject
{
    internal class Program
    {
        static void Main()
        {
            using (IMyThreadPool threadPool = new MyThreadPool(4, 6))
            {
                for (var i = 0; i < 8; i++)
                {
                    threadPool.QueueTask(() =>
                    {
                        Thread.Sleep(4000);
                        Console.WriteLine($"Hello from thread {Thread.CurrentThread.ManagedThreadId} with name {Thread.CurrentThread.Name}");
                    });
                }

                Thread.Sleep(10000);
                Console.WriteLine($"Count of threads in thread pool: {threadPool.Count}");

                Console.WriteLine("Start disposing thread pool");
            }

            Console.WriteLine("Thread pool must be disposed");
        }
    }

    public class MyThreadPool : IMyThreadPool
    {
        private volatile Thread[] _threads;
        private readonly ConcurrentQueue<Worker> _tasks;
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

        public int Count => _threads.Length;

        public void QueueTask(Action task)
        {
            Worker worker;
            lock (_lock)
            {
                if (_isDisposed) return;
                worker = new Worker(task);
                _tasks.Enqueue(worker);
                Monitor.PulseAll(_lock);
            }

            Thread.Sleep(1);
            if(worker.Status is WorkerStatus.Handled or WorkerStatus.Executing) return;

            AppendBackgroundThread();
        }

        private void AppendBackgroundThread()
        {
            if(_threads.Length == _maxCountOfThreads) return;

            lock (_lock)
            {
                var temp = new Thread[_threads.Length + 1];
                Array.Copy(_threads, temp, _threads.Length);
                var newThread = new Thread(Consume) { IsBackground = true, Name = $"My Thread Pool thread No{temp.Length - 1}" };
                newThread.Start();
                temp[^1] = newThread;
                Interlocked.Exchange(ref _threads, temp);
            }
        }

        private void Consume(object arg)
        {
            while (true)
            {
                Worker worker;
                lock (_lock)
                {
                    while (_tasks.IsEmpty || _isDisposed || _isDisposing)
                    {
                        Monitor.Wait(_lock);
                        if (_isDisposed || _isDisposing) return;
                    }
                    
                    if (!_tasks.TryDequeue(out worker)) continue;
                }
                if (_isDisposed || _isDisposing) return;

                worker.SetAsExecuting();
                worker.Task?.Invoke();
                worker.SetAsHandled();
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

        private enum WorkerStatus { Waiting, Executing, Handled }

        private class Worker
        {
            public Worker(Action task)
            {
                Task = task;
                Status = WorkerStatus.Waiting;
            }

            public Action Task { get; }

            public WorkerStatus Status { get; private set; }

            public void SetAsHandled() => Status = WorkerStatus.Handled;

            public void SetAsExecuting() => Status = WorkerStatus.Executing;
        }
    }

    public interface IMyThreadPool : IDisposable
    {
        int Count { get; }

        void QueueTask(Action task);
    }
}
