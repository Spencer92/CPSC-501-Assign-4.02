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
            StreamReader audioData1;
            StreamReader audioData2;
            StreamWriter output;
            int soundLength = 0;
            int environmentLength = 0;
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
            char value;
            byte byteValue;


            try
            {
                audioData1 = new StreamReader(args[0]);
            }
            catch(FileNotFoundException e)
            {
                Console.WriteLine(args[0] + " not found, exiting");
                return;
            }
            try
            {
                audioData2 = new StreamReader(args[1]);
            }
            catch(FileNotFoundException e)
            {
                Console.WriteLine(args[1] + " not found, exiting");
                return;
            }






            //Get the length of the file
            while (!audioData1.EndOfStream)
            {
                audioData1.Read();
                soundLength++;
            }

            audioData1.Close();
            audioData1 = new StreamReader(args[0]);

            //As the values read are characters, the length needs to be halved
            newDryValues = new float[(soundLength / 2) + 1];
            length = (soundLength / 2) + 1;

            while (!audioData1.EndOfStream)
            {
                //Get the data and 
                digit = Convert.ToUInt16(audioData1.Read());
                digit *= CHAR_SIZE;
                if (!audioData1.EndOfStream)
                {
                    digit += Convert.ToUInt16(audioData1.Read());
                }

                shortDigit = (float)digit;

                if (index < length)
                {
                    newDryValues[index] = shortDigit / NEGATIVE_FLOAT;
                    index++;
                }
            }



            audioData1.Close();
            audioData2 = new StreamReader(args[1]);

            while(!audioData1.EndOfStream)
            {
                audioData2.Read();
                environmentLength++;
            }
            audioData2.Close();

            audioData2 = new StreamReader(args[1]);
            areaData = new float[(environmentLength / 2) + 1];
            length = environmentLength / 2 + 1;

            index = 0;
            while (!audioData1.EndOfStream)
            {
                digit = Convert.ToUInt16(audioData1.Read());
                digit *= CHAR_SIZE;
                if (!audioData1.EndOfStream)
                {
                    digit += Convert.ToUInt16(audioData1.Read());
                }

                shortDigit = (float)digit;

                if (index < length)
                {
                    thing = shortDigit / NEGATIVE_FLOAT;
                    areaData[index] = shortDigit / NEGATIVE_FLOAT;
                    index++;
                }
            }
            audioData2.Close();

            convolvedData = new float[areaData.Length + newDryValues.Length - 1];


            convolver = new Convolve();
            convolver.convolveSln(newDryValues, newDryValues.Length, areaData, areaData.Length, convolvedData, convolvedData.Length);

            convertedData = new short[convolvedData.Length];
            length = convertedData.Length;

            output = new StreamWriter("output.wav");


            //It could be that the audio data is greater than one, if so, it
            //needs to be reduced
            for(int i = 0; i < convolvedData.Length; i++)
            {
                if(convolvedData[i] > largest_value)
                {
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

            for(int i = 0; i < length; i++)
            {
                byteValue = Convert.ToByte(convertedData[i] / CHAR_SIZE);
                value = (char) byteValue;

                output.Write(value);
                byteValue = Convert.ToByte(convertedData[i]%256);
                value = (char)byteValue;

                output.Write(value);

            }

            Console.Write("End of program, enter key to exit");
            output.Close();
            Console.Read();
      

        }
    }
}
