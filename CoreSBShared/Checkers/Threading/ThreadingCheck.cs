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
    
    // volatile
    // lock barier
    // interlocked 
    // clear voaltile
    
}
