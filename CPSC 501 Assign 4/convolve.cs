using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CPSC_501_Assign_4
{
    class Convolve
    {
        public void convolveSln(float[] x, int N, float[] h, int M, float[] y, int P)
        {
            int n; /*  Make sure the output buffer is the right size: P = N + M - 1  */
            int m = 0;
            if(P != (N+M-1))
            {
                Console.WriteLine("Output signal vector is the wrong size");
                Console.WriteLine("It is " + P + " but should be " + (N + M - 1));
                Console.WriteLine("Aborting Convolution");
                return;
            }
            //Clear the output buffer y[] to all zero values
            for(n = 0; n < P; n++)
            {
                y[n] = 0.0F; //Do the convoltion;
            }



            //Outer loop: process each input value x[n] in turn
            for (n = 0; n < N; n++)
            {
                //Inner loop: process x[n] with each sample of h[]
                for (m = 0; m < M; m++)
                {
                    y[n + m] += x[n] * h[m];
                }
            }
        }

    }
}
