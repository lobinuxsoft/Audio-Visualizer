using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioVisualizer : MonoBehaviour
{
    public AudioSource audioSource;
    float[] samplesLeft = new float[512];
    float[] samplesRight = new float[512];

    float[] freqBand = new float[8];
    float[] bandBuffer = new float[8];

    float[] bufferDecrease = new float[8];
    float[] freqBandHighest = new float[8];

    public static float[] audioBand = new float[8];
    public static float[] audioBandBuffer = new float[8];

    public static float amplitude, amplitudeBuffer;
    float amplitudeHighest;
    public float audioProfile = 5;

    public enum Channel { STEREO, LEFT, RIGHT}

    public Channel channel = Channel.STEREO;

    // Start is called before the first frame update
    void Start()
    {
        AudioProfile(audioProfile);
    }

    // Update is called once per frame
    void Update()
    {
        GetSpectrumAudioSource();
        MakeFrequencyBands();
        BandBuffer();
        CreateAudioBands();
        GetAmplitude();
    }

    void AudioProfile(float value)
    {
        for (int i = 0; i < 8; i++)
        {
            freqBandHighest[i] = value;
        }
    }

    void GetAmplitude()
    {
        float currentAmplitude = 0;
        float currentAmplitudeBuffer = 0;

        for (int i = 0; i < 8; i++)
        {
            currentAmplitude += audioBand[i];
            currentAmplitudeBuffer += audioBandBuffer[i];
        }

        if (currentAmplitude > amplitudeHighest)
        {
            amplitudeHighest = currentAmplitude;
        }

        amplitude = currentAmplitude / amplitudeHighest;
        amplitudeBuffer = currentAmplitudeBuffer / amplitudeHighest;
    }

    void CreateAudioBands()
    {
        for (int i = 0; i < 8; i++)
        {
            if (freqBand[i] > freqBandHighest[i])
            {
                freqBandHighest[i] = freqBand[i];
            }
            audioBand[i] = (freqBand[i] / freqBandHighest[i]);
            audioBandBuffer[i] = (bandBuffer[i] / freqBandHighest[i]);
        }
    }

    void GetSpectrumAudioSource()
    {
        audioSource.GetSpectrumData(samplesLeft, 0, FFTWindow.Blackman);
        audioSource.GetSpectrumData(samplesRight, 1, FFTWindow.Blackman);
    }

    void BandBuffer()
    {
        for (int i = 0; i < 8; i++)
        {
            if (freqBand[i] > bandBuffer[i])
            {
                bandBuffer[i] = freqBand[i];
                bufferDecrease[i] = 0.005f;
            }

            if (freqBand[i] < bandBuffer[i])
            {
                bandBuffer[i] -= bufferDecrease[i];
                bufferDecrease[i] *= 1.2f;
            }
        }
    }

    void MakeFrequencyBands()
    {
        /*
         Frequency Bands Range
         0 - 2 = 86 hertz
         1 - 4 = 172 hertz - 87-258
         2 - 8 = 344 hertz - 259-602
         3 - 16 = 688 hertz - 603-1290
         4 - 32 = 1376 hertz - 1291-2666
         5 - 64 = 2752 hertz - 2667-5418
         6 - 128 = 5504 hertz - 5419-10922
         7 - 256 = 11008 hertz - 10923-21930
         */

        int count = 0;

        for (int i = 0; i < 8; i++)
        {
            float average = 0;
            int sampleCount = (int)Mathf.Pow(2, i) * 2;

            if (i == 7)
            {
                sampleCount += 2;
            }

            for (int j = 0; j < sampleCount; j++)
            {
                switch (channel)
                {
                    case Channel.STEREO:
                        average += (samplesLeft[count] + samplesRight[count]) * (count + 1);
                        break;
                    case Channel.LEFT:
                        average += samplesLeft[count] * (count + 1);
                        break;
                    case Channel.RIGHT:
                        average += samplesRight[count] * (count + 1);
                        break;
                }
                
                count++;
            }

            average /= count;

            freqBand[i] = average * 10;
        }
    }
}
