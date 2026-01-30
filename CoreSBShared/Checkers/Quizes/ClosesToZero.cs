using System;
using System.Collections.Generic;

namespace CoreSBShared.Universal.Checkers.Quizes
{
    public class ClosesToZero
    {
        public static float ClosestToZero(IEnumerable<float> temps)
        {
            float? temp = null;
            foreach (var t in temps)
            {
                if (temp == null)
                    temp = t;

                if (t == 0)
                {
                    temp = t;
                    return (float)temp;
                }
                
                if (Math.Abs(t) < Math.Abs((float)temp) || 
                    (Math.Abs(t) == Math.Abs((float)temp) && t > 0))
                    temp = t;
            }
            
            return temp ?? 0.0F;
         
        }
    }
}
