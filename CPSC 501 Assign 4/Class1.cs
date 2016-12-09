using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace CPSC_501_Assign_4
{
    //Taken from wavfile.c

    class wavfile_manipulation
    {
        public static int WAVFILE_SAMPLES_PER_SECOND = 44100;
        private static class Wavfile_header
        {
            public static String riff_tag;
            public static int riff_length;
            public static String wave_tag;
            public static String fmt_tag;
            public static int fmt_length;
            public static short auido_format;
            public static short num_channels;
            public static int sample_rate;
            public static int byte_rate;
            public static short block_align;
            public static short bits_per_sample;
            public static String data_tag;
            public static int data_length;
        }

        public class wavfile_open
        {
 //           Wavfile_header header = new Wavfile_header();
            private string file_name;

            public wavfile_open(string file_name)
            {
                this.file_name = file_name;
            }

            public StreamReader open()
            {
                int samples_per_second = WAVFILE_SAMPLES_PER_SECOND;
                int bits_per_sample = 16;

                Wavfile_header.riff_tag = "RIFF";
                Wavfile_header.wave_tag = "WAVE";
                Wavfile_header.fmt_tag = "fmt ";
                Wavfile_header.data_tag = "data";

                Wavfile_header.riff_length = 0;
                Wavfile_header.fmt_length = 16;
                Wavfile_header.auido_format = 1;
                Wavfile_header.num_channels = 1;
                Wavfile_header.sample_rate = samples_per_second;
                Wavfile_header.byte_rate = samples_per_second * (bits_per_sample / 8);
                Wavfile_header.block_align = Convert.ToInt16(bits_per_sample / 8);
                Wavfile_header.bits_per_sample = Convert.ToInt16(bits_per_sample);
                Wavfile_header.data_length = 0;

                try
                {
                    StreamReader reader = new StreamReader(this.file_name);
                }
                catch(FileNotFoundException e)
                {
                    throw new FileNotFoundException("File not found!");
                }



                return null;




            }

        }

    }
}
