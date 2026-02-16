using System;

namespace InfrastructureCheckers
{
    // !!! >>> LLM generated
    /* Static class containing heavy CPU-bound methods */
    public static class CpuHeavy
    {
        /* O(N²) CPU work: sum over nested loops */
        public static double WorkQuadratic(int n)
        {
            double sum = 0;
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    sum += i * j * 0.0001; // arbitrary computation
                }
            }
            return sum;
        }

        /* O(N³) CPU work: triple nested loops */
        public static double WorkCubic(int n)
        {
            double sum = 0;
            for (int i = 0; i < n; i++)
            for (int j = 0; j < n; j++)
            for (int k = 0; k < n; k++)
                sum += i + j + k;
            return sum;
        }

        /* O(N⁴) CPU work: quadruple nested loops */
        public static double WorkQuartic(int n)
        {
            double sum = 0;
            for (int i = 0; i < n; i++)
            for (int j = 0; j < n; j++)
            for (int k = 0; k < n; k++)
            for (int l = 0; l < n; l++)
                sum += (i + j + k + l) * 0.0001; // arbitrary computation
            return sum;
        }
    }
}
