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
            public short audio_format;
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
            BinaryReader audioData1;
            BinaryReader audioData2;
            BinaryWriter output;
            int soundLength = 0;
            int environmentLength = 0;
            float[] newDryValues;
            float[] areaData;
            float[] convolvedData;
            short[] convertedData;
            ushort first_part_of_digit;
            ushort second_part_of_digit;
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
            int audioData1Length;
            int audioData2Length;
            byte[] byteDataAudio;
            byte[] byteDataEnv;
            short actualNumber;
            byte[] convolvedDataByte;

            try
            {
                audioData1 = new BinaryReader(File.OpenRead("FluteDry.wav"));
            }
            catch (FileNotFoundException e)
            {
                Console.WriteLine(/*args[0] + */"FluteDry not found, exiting");
                Console.Read();
                return;
            }
            try
            {
                audioData2 = new BinaryReader(File.OpenRead("BIG HALL E001 M2S.wav"));
            }
            catch (FileNotFoundException e)
            {
                Console.WriteLine(/*args[1] + */"BIG HALL not found, exiting");
                Console.Read();
                return;
            }

            audioData1Length = (int)audioData1.BaseStream.Length;
            audioData2Length = (int)audioData2.BaseStream.Length;

            for (int i = 0; i < header.riff_tag.Length; i++)
            {
                header.riff_tag[i] = Convert.ToChar(audioData1.Read());
            }
            convertToString = new string(header.riff_tag);

            if (!convertToString.Equals("RIFF"))
            {
                Console.WriteLine("Didn't find RIFF, Found " + header.riff_tag.ToString());
                Console.WriteLine("Press any key to exit");
                Console.Read();
                return;
            }

            for (int i = 0; i < size_amount.Length; i++)
            {
                size_amount[i] = Convert.ToChar(audioData1.ReadByte());
                Console.WriteLine("What it is: " + size_amount[i]);
            }

            Console.WriteLine(size_amount[0]);
            Console.WriteLine(size_amount[1] * 0x100);
            Console.WriteLine(size_amount[2] * 0x10000);
            Console.WriteLine(size_amount[3] * 0x1000000);

            header.riff_length = (size_amount[0]) + (size_amount[1] * 0x100) + (size_amount[2] * 0x10000) + (size_amount[3] * 0x1000000);

            Console.Write("Size_amount is ");
            for (int i = 0; i < size_amount.Length; i++)
            {
                Console.Write(size_amount[i] + " ");
            }
            Console.Write("\n riff_length is ");
            Console.WriteLine(header.riff_length);


            for (int i = 0; i < header.wave_tag.Length; i++)
            {
                header.wave_tag[i] = Convert.ToChar(audioData1.ReadByte());
            }

            for (int i = 0; i < header.wave_tag.Length; i++)
            {
                Console.WriteLine(header.wave_tag[i]);
            }

            for (int i = 0; i < header.fmt_tag.Length; i++)
            {
                header.fmt_tag[i] = Convert.ToChar(audioData1.ReadByte());
            }

            for (int i = 0; i < size_amount.Length; i++)
            {
                size_amount[i] = audioData1.ReadByte();
            }

            header.fmt_length = size_amount[0] + (size_amount[1] * 0x100) + (size_amount[2] * 0x10000) + (size_amount[3] * 0x1000000);

            Console.WriteLine("fmt_length: " + header.fmt_length);


            for (int i = 0; i < size_amount.Length; i++)
            {
                size_amount[i] = audioData1.ReadByte();
            }

            header.audio_format = Convert.ToInt16(size_amount[0] + (size_amount[1] * 0x100));
            header.num_channels = Convert.ToInt16(size_amount[2] + (size_amount[3] * 0x100));

            Console.WriteLine("audio_format: " + header.audio_format);
            Console.WriteLine("num_channels: " + header.num_channels);

            for (int i = 0; i < size_amount.Length; i++)
            {
                size_amount[i] = audioData1.ReadByte();
            }

            header.sample_rate = size_amount[0] + (size_amount[1] * 0x100) + (size_amount[2] * 0x10000) + (size_amount[3] * 0x1000000);

            for (int i = 0; i < size_amount.Length; i++)
            {
                size_amount[i] = audioData1.ReadByte();
            }
            header.byte_rate = size_amount[0] + (size_amount[1] * 0x100) + (size_amount[2] * 0x10000) + (size_amount[3] * 0x1000000);

            Console.WriteLine(header.sample_rate);
            Console.WriteLine(header.byte_rate);

            for (int i = 0; i < size_amount.Length; i++)
            {
                size_amount[i] = audioData1.ReadByte();
            }

            for (int i = 0; i < size_amount.Length; i++)
            {
                Console.WriteLine(size_amount[i]);
            }

            header.block_align = Convert.ToInt16(size_amount[0] + (size_amount[1] * 0x100));
            header.bits_per_sample = Convert.ToInt16(size_amount[2] + (size_amount[3] * 0x100));

            Console.WriteLine("block_align: " + header.block_align);
            Console.WriteLine("bits_per_sample: " + header.bits_per_sample);

            for (int i = 0; i < header.data_tag.Length; i++)
            {
                header.data_tag[i] = Convert.ToChar(audioData1.ReadByte());
                Console.Write(header.data_tag[i]);
            }

            for (int i = 0; i < size_amount.Length; i++)
            {
                size_amount[i] = audioData1.ReadByte();
            }

            header.data_length = (size_amount[0]) + (size_amount[1] * 0x100) + (size_amount[2] * 0x10000) + (size_amount[3] * 0x1000000);

            Console.WriteLine("\n" + header.data_length);

            output = new BinaryWriter(File.OpenWrite("output.wav"));

            for (int i = 0; i < header.riff_tag.Length; i++)
            {
                output.Write(header.riff_tag[i]);
            }



            size_amount[0] = header.riff_length / 0x1000000;
            size_amount[1] = (header.riff_length / 0x10000) & 0xFF;
            size_amount[2] = (header.riff_length / 0x100) & 0xFF;
            size_amount[3] = header.riff_length & 0xFF;

            for (int i = 3; i >= 0; i--)
            {
                output.Write(Convert.ToByte(size_amount[i]));
            }

            for (int i = 0; i < header.wave_tag.Length; i++)
            {
                output.Write(Convert.ToChar(header.wave_tag[i]));
            }

            for (int i = 0; i < header.fmt_tag.Length; i++)
            {
                output.Write(Convert.ToChar(header.fmt_tag[i]));
            }

            size_amount[0] = header.fmt_length / 0x1000000;
            size_amount[1] = (header.fmt_length / 0x10000) & 0xFF;
            size_amount[2] = (header.fmt_length / 0x100) & 0xFF;
            size_amount[3] = header.fmt_length & 0xFF;

            for (int i = 3; i >= 0; i--)
            {
                output.Write(Convert.ToByte(size_amount[i]));
            }


            size_amount[0] = header.audio_format / 0x100;
            size_amount[1] = header.audio_format & 0xFF;
            size_amount[2] = header.num_channels / 0x100;
            size_amount[3] = header.num_channels & 0xFF;

            output.Write(Convert.ToByte(size_amount[1]));
            output.Write(Convert.ToByte(size_amount[0]));
            output.Write(Convert.ToByte(size_amount[3]));
            output.Write(Convert.ToByte(size_amount[2]));

            size_amount[0] = header.sample_rate / 0x1000000;
            size_amount[1] = (header.sample_rate / 0x10000) & 0xFF;
            size_amount[2] = (header.sample_rate / 0x100) & 0xFF;
            size_amount[3] = header.sample_rate & 0xFF;

            for (int i = 3; i >= 0; i--)
            {
                output.Write(Convert.ToByte(size_amount[i]));
            }

            size_amount[0] = header.byte_rate / 0x1000000;
            size_amount[1] = (header.byte_rate / 0x10000) & 0xFF;
            size_amount[2] = (header.byte_rate / 0x100) & 0xFF;
            size_amount[3] = header.byte_rate & 0xFF;

            for (int i = 3; i >= 0; i--)
            {
                output.Write(Convert.ToByte(size_amount[i]));
            }

            size_amount[0] = header.block_align / 0x100;
            size_amount[1] = header.block_align & 0xFF;
            size_amount[2] = header.bits_per_sample / 0x100;
            size_amount[3] = header.bits_per_sample & 0xFF;

            output.Write(Convert.ToByte(size_amount[1]));
            output.Write(Convert.ToByte(size_amount[0]));
            output.Write(Convert.ToByte(size_amount[3]));
            output.Write(Convert.ToByte(size_amount[2]));

            for (int i = 0; i < header.data_tag.Length; i++)
            {
                output.Write(Convert.ToChar(header.data_tag[i]));
            }

            size_amount[0] = header.data_length / 0x1000000;
            size_amount[1] = (header.data_length / 0x10000) & 0xFF;
            size_amount[2] = (header.data_length / 0x100) & 0xFF;
            size_amount[3] = header.data_length & 0xFF;

            for (int i = 3; i >= 0; i--)
            {
                output.Write(Convert.ToByte(size_amount[i]));
            }


            for (int i = 0; i < 44; i++)
            {
                audioData2.ReadByte();
            }


            byteDataAudio = new byte[audioData1Length - 44];

            for (int i = 0; i < audioData1Length - 44; i++)
            {
                byteDataAudio[i] = audioData1.ReadByte();
            }

            //As float = 2 bytes
            newDryValues = new float[(byteDataAudio.Length / 2) + 1];
            index = 0;
            for (int i = 0; i < byteDataAudio.Length; i++)
            {
                first_part_of_digit = Convert.ToUInt16(byteDataAudio[i]);
                i++;
                second_part_of_digit = 0;
                if (i < byteDataAudio.Length)
                {
                    second_part_of_digit = Convert.ToUInt16(byteDataAudio[i]);
                    second_part_of_digit *= 0x100;
                }
                first_part_of_digit += second_part_of_digit;
                if (index < newDryValues.Length)
                {
                    actualNumber = (short)first_part_of_digit;
                    newDryValues[index] = (float)actualNumber / 0x8000;
                    index++;
                }
                else
                {
                    break; //Can't fill any longer
                }
            }

            for (int i = 0; i < newDryValues.Length; i++)
            {
                Console.WriteLine(newDryValues[i]);
            }

            byteDataEnv = new byte[audioData2Length - 44];

            for (int i = 0; i < audioData2Length - 44; i++)
            {
                byteDataEnv[i] = audioData2.ReadByte();
            }

            areaData = new float[(byteDataEnv.Length / 2) + 1];

            index = 0;
            for (int i = 0; i < byteDataEnv.Length; i++)
            {
                first_part_of_digit = Convert.ToUInt16(byteDataAudio[i]);
                i++;
                second_part_of_digit = 0;
                if (i < byteDataEnv.Length)
                {
                    second_part_of_digit = Convert.ToUInt16(byteDataAudio[i]);
                    second_part_of_digit *= 0x100;
                }
                first_part_of_digit += second_part_of_digit;
                if (index < areaData.Length)
                {
                    actualNumber = (short)first_part_of_digit;
                    areaData[index] = (float)actualNumber / 0x8000;
                    index++;
                }
                else
                {
                    break; //Can't fill any longer
                }
            }



            /*            for(int i = 0; i < byteData.Length; i++)
                        {
                            output.Write(Convert.ToByte(byteData[i]));
                        }*/

            convolvedData = new float[newDryValues.Length + areaData.Length - 1];

            convolver = new Convolve();
            convolver.convolveSln(newDryValues, newDryValues.Length, areaData, areaData.Length, convolvedData, convolvedData.Length);

            for (int i = 0; i < convolvedData.Length; i++)
            {
                Console.WriteLine(convolvedData[i]);
            }

            convertedData = new short[convolvedData.Length];

            for (int i = 0; i < convolvedData.Length; i++)
            {
                if (convolvedData[i] > largest_value)
                {
                    largest_value = convolvedData[i];
                }
            }

            for (int i = 0; i < convolvedData.Length; i++)
            {
                convolvedData[i] /= largest_value;
            }

            for (int i = 0; i < convolvedData.Length; i++)
            {
                convertedData[i] = Convert.ToInt16(convolvedData[i] * 0x7FFF);
            }

            for (int i = 0; i < convertedData.Length; i++)
            {
                Console.WriteLine(convertedData[i]);
            }

            convolvedDataByte = new Byte[(convertedData.Length * 2) + 1];

            index = 0;
            for (int i = 0; i < convolvedDataByte.Length; i++)
            {
                if (index < convertedData.Length)
                {
                    convolvedDataByte[i] = Convert.ToByte(convertedData[index] & 0xFF);
                }
                else
                {
                    break;
                }
                i++;
                if (i < convolvedDataByte.Length)
                {
                    if (index < convertedData.Length)
                    {
                        Console.WriteLine("{0:X}", convertedData[index]);
                        Console.WriteLine("{0:X}", (convertedData[index] / 0x100) & 0xFF);

                        convolvedDataByte[i] = Convert.ToByte((convertedData[index] / 0x100) & 0xFF);
                        index++;
                    }
                    else
                    {
                        break;
                    }
                }

            }

            for (int i = 0; i < convolvedDataByte.Length; i++)
            {
                output.Write(convolvedDataByte[i]);
            }

            Console.WriteLine("Done");

            output.Close();
            audioData1.Close();
            audioData2.Close();
            Console.Read();
            return;

      

        }
    }
}
