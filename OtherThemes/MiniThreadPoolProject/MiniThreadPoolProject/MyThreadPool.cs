using System;
using System.Collections.Concurrent;
using System.Threading;

namespace MiniThreadPoolProject
{
    public class MyThreadPool : IMyThreadPool
    {
        private Thread[] _threads;
        private readonly ConcurrentQueue<Worker> _tasks;
        private readonly object _lock = new();
        private readonly int _maxCountOfThreads;

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

        public IMultiThreadWorkCallback QueueTask(Action task)
        {
            MultiThreadWorkCallback callback;
            lock (_lock)
            {
                if (_isDisposed) return null;
                callback = new MultiThreadWorkCallback();
                var worker = new Worker(task, callback);
                _tasks.Enqueue(worker);
                Monitor.PulseAll(_lock);
            }
            
            if (_tasks.Count >= _threads.Length)
            {
                AppendBackgroundThread();
            }

            return callback;
        }

        private void AppendBackgroundThread()
        {
            if (_threads.Length == _maxCountOfThreads) return;

            lock (_lock)
            {
                if (_threads.Length == _maxCountOfThreads) return;
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

                worker.ExecuteTask();
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
            public Worker(Action task, MultiThreadWorkCallback callback)
            {
                Task = task;
                Callback = callback;
                Status = WorkerStatus.Waiting;
            }

            public Action Task { get; }

            public MultiThreadWorkCallback Callback { get; private set; }

            public WorkerStatus Status { get; private set; }

            private void SetAsHandled()
            {
                Status = WorkerStatus.Handled;
                Callback.IsTaskHandled = true;
            }

            private void SetAsFailed(Exception exception)
            {
                Callback.Exception = exception;
                Callback.IsFailed = true;
                Callback.IsTaskHandled = true;
                Status = WorkerStatus.Handled;
            }

            public void ExecuteTask()
            {
                Status = WorkerStatus.Executing;
                try
                {
                    Task?.Invoke();
                    SetAsHandled();
                }
                catch (Exception ex)
                {
                    SetAsFailed(ex);
                }
            }
        }
    }
}