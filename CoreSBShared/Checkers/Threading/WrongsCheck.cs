using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreSBShared.Universal.Checkers.Threading;

public class WrongsCheck
{
    public static async Task WrongFireAndForget()
    {
        var fg = new FireAndForgetWrong();
        fg.CallerWrong();
        await fg.CallerCorrect();
        
    }
    
    public static void BlockedCall()
    {
        var item = SomeAsync().Result;
        var fix = SomeAsync().GetAwaiter().GetResult();
    }

    public static async Task<int> SomeAsync()
    {
        var items = new List<int>()
        {
            1,
            2,
            3,
            4,
            5
        };

        var rnd = new Random();
        var rands = Enumerable
            .Range(1, 15)
            .Select(s => rnd.Next(1, 5))
            .ToList();

        var q = rands.Select(s =>
        {
            var v = s.ToString() + "a";
            return new {val = v, idx = s};
        });

        var q1 = q.ToList();
        var q2 = q.ToList();

        await Task.Delay(1);
        return 1;
    }
}
