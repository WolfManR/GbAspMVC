using System;

namespace MiniThreadPoolProject
{
    public class MultiThreadWorkCallback : IMultiThreadWorkCallback
    {
        public bool IsTaskHandled { get; set; }
        public bool IsFailed { get; set; }
        public Exception Exception { get; set; }
    }
}