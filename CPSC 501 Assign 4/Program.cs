using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace CPSC_501_Assign_4
{

    class Program
    {
        static int BYTE = 16;
        static ushort CHAR_SIZE = 256;
        static uint NEGATIVE_FLOAT = 32768;
        static uint POSITIVE_FLOAT = 32767;


        static void Main(string[] args)
        {
            StreamReader audioData = new StreamReader("FluteDry.wav");
            StreamWriter output;
            int soundLength = 0;
            int environmentLength = 0;
            int stuff;
            float[] newDryValues;
            float[] areaData;
            float[] convolvedData;
            short[] convertedData;
            ushort digit;
            float shortDigit;
            int index = 0;
            int length;
            float thing;
            Convolve convolver;
            float largest_value = 1;

            while (!audioData.EndOfStream)
            {
                audioData.Read();
                soundLength++;
            }

            audioData.Close();
            audioData = new StreamReader("FluteDry.wav");

            newDryValues = new float[(soundLength / 2) + 1];
            length = (soundLength / 2) + 1;

            while (!audioData.EndOfStream)
            {
                digit = Convert.ToUInt16(audioData.Read());
                digit *= CHAR_SIZE;
                if (!audioData.EndOfStream)
                {
                    digit += Convert.ToUInt16(audioData.Read());
                }

                shortDigit = (float)digit;

                if (index < length)
                {
                    newDryValues[index] = shortDigit / NEGATIVE_FLOAT;
                    Console.WriteLine(newDryValues[index]);
                    index++;
                }
            }
//            Console.ReadLine();


            audioData.Close();
            audioData = new StreamReader("BIG HALL E001 M2S.wav");

            for (int i = 0; i < newDryValues.Length; i++)
            {
                Console.WriteLine(newDryValues[i]);
            }

            while(!audioData.EndOfStream)
            {
                audioData.Read();
                environmentLength++;
            }
            audioData.Close();

            audioData = new StreamReader("BIG HALL E001 M2S.wav");
            areaData = new float[(environmentLength / 2) + 1];
            length = environmentLength / 2 + 1;

            index = 0;
            while (!audioData.EndOfStream)
            {
                digit = Convert.ToUInt16(audioData.Read());
                digit *= CHAR_SIZE;
                if (!audioData.EndOfStream)
                {
                    digit += Convert.ToUInt16(audioData.Read());
                }

                shortDigit = (float)digit;

                if (index < length)
                {
                    thing = shortDigit / NEGATIVE_FLOAT;
                    areaData[index] = shortDigit / NEGATIVE_FLOAT;
                    Console.Write(areaData[index]);
                    index++;
                }
            }
            audioData.Close();

            convolvedData = new float[areaData.Length + newDryValues.Length - 1];
            Console.WriteLine("Right before convolve");

            /*            for(int i = 0; i < newDryValues.Length; i++)
                        {
                            Console.Write(newDryValues[i]);
                        }*/

//            Console.ReadLine();
            for(int i = 0; i < areaData.Length; i++)
            {
                Console.WriteLine(areaData[i]);
            }

            convolver = new Convolve();
            convolver.convolveSln(newDryValues, newDryValues.Length, areaData, areaData.Length, convolvedData, convolvedData.Length);

            convertedData = new short[convolvedData.Length];
            length = convertedData.Length;

            output = new StreamWriter("output.wav");


            for(int i = 0; i < convolvedData.Length; i++)
            {
                if(convolvedData[i] > largest_value)
                {
                    Console.WriteLine("Too big: ");
                    largest_value = convolvedData[i];
                }
            }

            for(int i = 0; i < convolvedData.Length; i++)
            {
                convolvedData[i] /= largest_value;
            }

            for(int i = 0; i < convertedData.Length; i++)
            {
                convertedData[i] = Convert.ToInt16(convolvedData[i] * POSITIVE_FLOAT);
            }
            char value;
            byte byteValue;
            for(int i = 0; i < length; i++)
            {
                byteValue = Convert.ToByte(convertedData[i] / CHAR_SIZE);
                value = (char) byteValue;
                Console.WriteLine(value);
                output.Write(value);
                byteValue = Convert.ToByte(convertedData[i]%256);
                value = (char)byteValue;
                Console.WriteLine(value);
                output.Write(value);

            }

            Console.Write("End of program");
            output.Close();
            Console.Read();
      

        }
    }
}
