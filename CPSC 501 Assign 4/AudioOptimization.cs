using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace CPSC_501_Assign_4
{



    class AudioOptimization
    {
        private class Wavfile_header
        {
            public char[] riff_tag = new char[4];
            public int riff_length;
            public char[] wave_tag = new char[4];
            public char[] fmt_tag = new char[4];
            public int fmt_length;
            public short auido_format;
            public short num_channels;
            public int sample_rate;
            public int byte_rate;
            public short block_align;
            public short bits_per_sample;
            public char[] data_tag = new char[4];
            public int data_length;
        }


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
            char[] preValuesData = new char[24];
            char[] preValuesEnv = new char[24];
            Wavfile_header header = new Wavfile_header();
            String convertToString;
            int[] size_amount = new int[4];

            try
            {
                audioData1 = new StreamReader("FluteDry.wav");
            }
            catch(FileNotFoundException e)
            {
                Console.WriteLine(/*args[0] + */"FluteDry not found, exiting");
                Console.Read();
                return;
            }
            try
            {
                audioData2 = new StreamReader("BIG HALL E001 M2S.wav");
            }
            catch(FileNotFoundException e)
            {
                Console.WriteLine(/*args[1] + */"BIG HALL not found, exiting");
                Console.Read();
                return;
            }


            for(int i = 0; i < header.riff_tag.Length; i++)
            {
                header.riff_tag[i] = Convert.ToChar(audioData1.Read());
            }
            convertToString = new string(header.riff_tag);

            if(!convertToString.Equals("RIFF"))
            {
                Console.WriteLine("Didn't find RIFF, Found " + header.riff_tag.ToString());
                Console.WriteLine("Press any key to exit");
                Console.Read();
                return;
            }

            for(int i = 0; i < size_amount.Length; i++)
            {
                size_amount[i] = audioData1.Read();
            }

            header.riff_length = (size_amount[0] * 0xFFFFFF) + (size_amount[1] * 0xFFFF) + (size_amount[2] * 0xFF) + size_amount[3];

            Console.Write("Size_amount is ");
            for(int i = 0; i < size_amount.Length; i++)
            {
                Console.Write(size_amount[i] + " ");
            }
            Console.Write("\n riff_length is ");
            Console.WriteLine(header.riff_length);
            Console.Read();


            for(int i = 0; i < preValuesData.Length; i++)
            {
                preValuesData[i] = Convert.ToChar(audioData1.Read());
            }


            //Get the length of the file
            while (!audioData1.EndOfStream)
            {
                audioData1.Read();
                soundLength++;
            }

            audioData1.Close();
            audioData1 = new StreamReader("FluteDry.wav");

            //As the values read are characters, the length needs to be halved
            newDryValues = new float[(soundLength / 2) + 1];
            length = (soundLength / 2) + 1;

            for(int i = 0; i < preValuesData.Length; i++)
            {
                audioData1.Read();
            }

            while (!audioData1.EndOfStream)
            {
                //Get the data and convert it
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
            

            for (int i = 0; i < preValuesEnv.Length; i++)
            {
                preValuesEnv[i] = Convert.ToChar(audioData2.Read());
            }


            while(!audioData2.EndOfStream)
            {
                audioData2.Read();
                environmentLength++;
            }
            audioData2.Close();

            audioData2 = new StreamReader("BIG HALL E001 M2S.wav");
            areaData = new float[(environmentLength / 2) + 1];
            length = environmentLength / 2 + 1;

            index = 0;
            while (!audioData2.EndOfStream)
            {
                digit = Convert.ToUInt16(audioData2.Read());
                digit *= CHAR_SIZE;
                if (!audioData2.EndOfStream)
                {
                    digit += Convert.ToUInt16(audioData2.Read());
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

            for(int i = 0; i < preValuesData.Length; i++)
            {
                output.Write(preValuesData[i]);
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
