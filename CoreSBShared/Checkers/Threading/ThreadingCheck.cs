using System;
using System.Threading;
using System.Threading.Tasks;

namespace CoreSBShared.Universal.Checkers.Threading
{
    public class FireAndForgetWrong
    {
        public Task DoWorkCorrect()
        {
            return Task.Run(() => DoWork());
        }

        public void DoWorkWrong()
        {
            Task.Run(() => DoWork());
        }

        void DoWork()
        {
            Thread.Sleep(1000);
            throw new Exception("Boom!");
        }

        public async Task CallerCorrect()
        {
            var cl = new FireAndForgetWrong();
            try
            {
                await cl.DoWorkCorrect();
            }
            catch (Exception e)
            {
                
            }
        }

        public void CallerWrong()
        {
            var cl = new FireAndForgetWrong();
            
            try
            {
                cl.DoWorkWrong();
            }
            catch (Exception e)
            {
                
            }
        }
    }

    class WrongVolatile
    {
        static volatile bool _ready;
        static int _data;

        static void Main()
        {
            var reader = new Thread(Reader);
            var writer = new Thread(Writer);

            reader.Start();
            writer.Start();

            reader.Join();
            writer.Join();
        }

        static void Writer()
        {
            Thread.Sleep(100);
            _data = 42;
            _ready = true;
            Console.WriteLine("Writer published data");
        }

        static void Reader()
        {
            while (!_ready) { }

            Console.WriteLine("Reader sees data = " + _data);
        }
    }

    class LockBarier
    {
        static readonly object _lock = new();
        static bool _ready;
        static int _data;

        static void Main()
        {
            var reader = new Thread(Reader);
            var writer = new Thread(Writer);

            reader.Start();
            writer.Start();

            reader.Join();
            writer.Join();
        }

        static void Writer()
        {
            Thread.Sleep(100);
            lock (_lock)
            {
                _data = 42;
                _ready = true;
            }
        }

        static void Reader()
        {
            while (true)
            {
                lock (_lock)
                {
                    if (_ready)
                    {
                        Console.WriteLine("Reader sees data = " + _data);
                        return;
                    }
                }
            }
        }
    }

    class InterlockedBarier
    {
        static int _data;
        static int _ready; // 0 = false, 1 = true

        static void Main()
        {
            var reader = new Thread(Reader);
            var writer = new Thread(Writer);

            reader.Start();
            writer.Start();

            reader.Join();
            writer.Join();
        }

        static void Writer()
        {
            Thread.Sleep(100);
            _data = 42;
            Interlocked.Exchange(ref _ready, 1);
        }

        static void Reader()
        {
            while (Interlocked.CompareExchange(ref _ready, 1, 1) == 0) { }

            Console.WriteLine("Reader sees data = " + _data);
        }
    }

    class clearVolatile
    {
        private static volatile bool _stopRequested = false;

        static void Main()
        {
            Thread worker = new Thread(WorkerLoop);
            worker.Start();

            Console.WriteLine("Worker started. Press Enter to stop...");
            Console.ReadLine();

            _stopRequested = true;   // publish stop signal
            worker.Join();

            Console.WriteLine("Worker stopped cleanly.");
        }

        static void WorkerLoop()
        {
            Console.WriteLine("Worker running on thread " + Thread.CurrentThread.ManagedThreadId);

            while (!_stopRequested)
            {
                // Simulate work
                Thread.Sleep(200);
                Console.Write(".");
            }

            Console.WriteLine("\nStop signal received.");
        }
    }

}
