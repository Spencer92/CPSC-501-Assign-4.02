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
            Console.ReadLine();
            for(int i = 0; i < x.Length; i++)
            {
                Console.Write(x[i]);
            }

            Console.ReadLine();
            for(int i = 0; i < h.Length; i++)
            {
                Console.Write(h[i]);
            }


            //Outer loop: process each input value x[n] in turn
            Console.WriteLine("Length of y: " + y.Length);
            Console.WriteLine("Length of n + m" + (n + m));
//           float stuff = h[0];
//            stuff = x[0];

//            try
//            {
                for (n = 0; n < N; n++)
                {
                //Inner loop: process x[n] with each sample of h[]
                for (m = 0; m < M; m++)
                {
                        y[n + m]
                            += x[n]
                            * h[m];
                    }
                }
//            }
/*            catch(IndexOutOfRangeException e)
            {
                Console.WriteLine("y is " + y.Length);
                Console.WriteLine("n + m is " + (m + n));
                Console.WriteLine("x is " + x.Length);
                Console.WriteLine("n is " + n);
                Console.WriteLine("h is " + h.Length);
                Console.WriteLine("m is " + m);
                Console.WriteLine("P is " + P);
                Console.Read();
            }*/
        }

    }
}
