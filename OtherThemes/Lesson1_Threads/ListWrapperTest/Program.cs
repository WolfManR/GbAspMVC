using System;
using System.Threading;

namespace ListWrapperTest
{
    internal class Program
    {
        private static readonly Random Random = new(648);

        private const int AddTimes = 100;
        private const int Times = 10;

        private const int PrintDelay = 1000;
        private const int AddDelay = 1000;
        private const int RemoveDelay = 1000;

        static void Main(string[] args)
        {
            ListWrapper<int> wrapper = new();
            
            for (var i = 0; i < 5; i++)
            {
                LaunchThread(new DTO(AddTimes, AddDelay, wrapper), ThreadType.increment, i);
                LaunchThread(new DTO(Times, RemoveDelay, wrapper), ThreadType.decrement, i);
                LaunchThread(new DTO(Times, PrintDelay, wrapper), ThreadType.print, i);
            }
        }

        private static void LaunchThread(DTO launchDTO, ThreadType threadType, int number)
        {
            Thread thread = null;
            switch (threadType)
            {
                case ThreadType.increment:
                    thread = new Thread(dto =>
                    {
                        if (dto is DTO d) Increment(d.Times, d.Delay, d.ListWrapper);
                    })
                    { 
                        Name = $"Increment {number}"
                    };
                    break;
                case ThreadType.decrement:
                    thread = new Thread(dto =>
                    {
                        if (dto is DTO d) Decrement(d.Times, d.Delay, d.ListWrapper);
                    })
                    {
                        Name = $"Decrement {number}"
                    };
                    break;
                case ThreadType.print:
                    thread = new Thread(dto =>
                    {
                        if (dto is DTO d) Print(d.Times, d.Delay, d.ListWrapper);
                    })
                    {
                        Name = $"Print {number}"
                    };
                    break;
            }
            thread?.Start(launchDTO);
        }

        private static void Increment(int maxTimes, int delay, ListWrapper<int> wrapper)
        {
            for (var i = 0; i < maxTimes; i++)
            {
                wrapper.Add(Random.Next(1000));
                Thread.Sleep(delay);
            }
        }

        private static void Decrement(int maxTimes, int delay, ListWrapper<int> wrapper)
        {
            for (var i = 0; i < maxTimes; i++)
            {
                wrapper.Remove(wrapper.GetLast());
                Thread.Sleep(delay);
            }
        }

        private static void Print(int maxTimes, int delay, ListWrapper<int> wrapper)
        {
            for (var i = 0; i < maxTimes; i++)
            {
                Console.WriteLine(string.Join(", ", wrapper.GetData()));
                Thread.Sleep(delay);
            }
        }

        private enum ThreadType{ increment, decrement, print }

        private readonly struct DTO
        {
            public DTO(int times, int delay, ListWrapper<int> listWrapper)
            {
                Times = times;
                Delay = delay;
                ListWrapper = listWrapper;
            }

            public int Times { get; }
            public int Delay { get; }
            public ListWrapper<int> ListWrapper { get; }
        }
    }
}
