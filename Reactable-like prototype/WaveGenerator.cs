using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace WpfApplication2
{
    /// <summary>
    /// Possible example waves to generate
    /// </summary>
    public enum WaveExampleType
    {
        ExampleSineWave = 0,
        ExampleSquareWave = 1,
        ExampleSawtoothWave = 2,
        ExampleTriangleWave = 3,
        ExampleWhiteNoise = 4
    }

    class WaveGenerator
    {
        // Header, Format, Data chunks
        WaveHeader header;
        WaveFormatChunk format;
        WaveDataChunk data;

        const double MAX_AMPLITUDE_16BIT = 32760;

        /// <summary>
        /// Initializes the object and generates a wave.
        /// </summary>
        /// <param name="type">The type of wave to generate</param>
        public WaveGenerator(WaveExampleType type, int frequency, double volume)
        {
            // Init chunks
            header = new WaveHeader();
            format = new WaveFormatChunk();
            data = new WaveDataChunk();

            // Number of samples = sample rate * channels * bytes per sample
            uint numSamples = format.dwSamplesPerSec * format.wChannels;

            // Initialize the 16-bit array
            data.shortArray = new short[numSamples];

            // Calculate data chunk size in bytes and store it in the data chunk
            data.dwChunkSize = (uint)(data.shortArray.Length * (format.wBitsPerSample / 8));

            // Max amplitude for 16-bit audio
            int amplitude = (int)(MAX_AMPLITUDE_16BIT * volume);

            // Create a double version of the frequency for easier math
            double freq = (double)frequency;

            // The "angle" used in the function, adjusted for the number of channels and sample rate.
            // This value is like the period of the wave.
            double t = (Math.PI * 2 * freq) / (format.dwSamplesPerSec * format.wChannels);

            // Used to generate some of the linear waveforms
            int samplesPerWavelength = 0;
            short ampStep = 0;
            short tempSample = 0;

            // Fill the data array with sample data
            switch (type)
            {
                case WaveExampleType.ExampleSineWave:                                                            

                    for (int i = 0; i < numSamples - 1; i++)
                    {
                        // Fill with a simple sine wave at max amplitude
                        for (int channel = 0; channel < format.wChannels; channel++)
                        {
                            data.shortArray[i + channel] = Convert.ToInt16(amplitude * Math.Sin(t * i));
                        }                        
                    }                    

                    break;

                case WaveExampleType.ExampleSquareWave:

                    for (int i = 0; i < numSamples - 1; i++)
                    {                        
                        for (int channel = 0; channel < format.wChannels; channel++)
                        {
                            //data.shortArray[i] = Convert.ToInt16(amplitude * Math.Sign(Math.Sin(t * i)));
                            data.shortArray[i] = Convert.ToInt16(amplitude * Math.Sign(Math.Sin(t * i)));
                        }
                    }
                    break;

                case WaveExampleType.ExampleSawtoothWave:
                    
                    // Determine the number of samples per wavelength
                    samplesPerWavelength = Convert.ToInt32(format.dwSamplesPerSec / (frequency / format.wChannels));

                    // Determine the amplitude step for consecutive samples
                    ampStep = Convert.ToInt16((amplitude * 2) / samplesPerWavelength);

                    // Temporary sample value, added to as we go through the loop
                    tempSample = (short)-amplitude;

                    // Total number of samples written so we know when to stop
                    int totalSamplesWritten = 0;

                    while (totalSamplesWritten < numSamples)
                    {
                        tempSample = (short)-amplitude;

                        for (uint i = 0; i < samplesPerWavelength && totalSamplesWritten < numSamples; i++)
                        {
                            for (int channel = 0; channel < format.wChannels; channel++)
                            {
                                tempSample += ampStep;
                                data.shortArray[totalSamplesWritten] = tempSample;

                                totalSamplesWritten++;
                            }
                        }                        
                    }
                    
                    break;

                case WaveExampleType.ExampleTriangleWave:

                    // Determine the number of samples per wavelength
                    samplesPerWavelength = Convert.ToInt32(format.dwSamplesPerSec / (frequency / format.wChannels));

                    // Determine the amplitude step for consecutive samples                  
                    ampStep = Convert.ToInt16((amplitude * 2) / samplesPerWavelength);

                    // Temporary sample value, added to as we go through the loop
                    tempSample = (short)-amplitude;

                    for (int i = 0; i < numSamples - 1; i++)
                    {
                        for (int channel = 0; channel < format.wChannels; channel++)
                        {
                            // Negate ampstep whenever it hits the amplitude boundary
                            if (Math.Abs(tempSample) > amplitude)
                                ampStep = (short)-ampStep;

                            tempSample += ampStep;
                            data.shortArray[i + channel] = tempSample;
                        }
                    }                    

                    break;

                case WaveExampleType.ExampleWhiteNoise:

                    // White noise is just a bunch of random samples.
                    Random rnd = new Random();
                    short randomValue = 0;

                    // No need for a nested loop since it's all random anyway
                    for (int i = 0; i < numSamples; i++)
                    {
                        randomValue = Convert.ToInt16(rnd.Next(-amplitude, amplitude));
                        data.shortArray[i] = randomValue;
                    }

                    break;
            }          
        }

        /// <summary>
        /// Saves the current wave data to the specified file.
        /// </summary>
        /// <param name="filePath"></param>
        public void Save(string filePath)
        {
            // Create a file (it always overwrites)
            FileStream fileStream = new FileStream(filePath, FileMode.Create);                                              

            // Use BinaryWriter to write the bytes to the file
            BinaryWriter writer = new BinaryWriter(fileStream);

            // Write the header
            writer.Write(header.sGroupID.ToCharArray());
            writer.Write(header.dwFileLength);
            writer.Write(header.sRiffType.ToCharArray());

            // Write the format chunk
            writer.Write(format.sChunkID.ToCharArray());
            writer.Write(format.dwChunkSize);
            writer.Write(format.wFormatTag);
            writer.Write(format.wChannels);
            writer.Write(format.dwSamplesPerSec);
            writer.Write(format.dwAvgBytesPerSec);
            writer.Write(format.wBlockAlign);
            writer.Write(format.wBitsPerSample);

            // Write the data chunk
            writer.Write(data.sChunkID.ToCharArray());
            writer.Write(data.dwChunkSize);
            foreach (short dataPoint in data.shortArray)
            {
                writer.Write(dataPoint);
            }

            writer.Seek(4, SeekOrigin.Begin);
            uint filesize = (uint)writer.BaseStream.Length;
            writer.Write(filesize - 8);
            
            // Clean up
            writer.Close();
            fileStream.Close();            
        }
    }
    
}
