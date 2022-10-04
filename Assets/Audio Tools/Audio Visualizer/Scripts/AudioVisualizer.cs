using UnityEngine;

public class AudioVisualizer : MonoBehaviour
{
    public static AudioVisualizer instance;
    public bool useAudioManager = false;
    public AudioSource audioSource;

    // Stereo Sampler
    float[] samplesLeft = new float[512];
    float[] samplesRight = new float[512];

    // Frequency Band 8
    float[] freqBand = new float[8];
    float[] bandBuffer = new float[8];
    float[] bufferDecrease = new float[8];
    float[] freqBandHighest = new float[8];

    //Frequency Band 64
    float[] freqBand64 = new float[64];
    float[] bandBuffer64 = new float[64];
    float[] bufferDecrease64 = new float[64];
    float[] freqBandHighest64 = new float[64];

    // Audio 8
    float[] audioBand, audioBandBuffer;

    //Audio 64
    float[] audioBand64, audioBandBuffer64;

    float amplitude, amplitudeBuffer;
    
    float amplitudeHighest;
    public float audioProfile = 5;

    public enum Channel { STEREO, LEFT, RIGHT}

    [SerializeField] Channel channel = Channel.STEREO;

    public float[] AudioBand => audioBand;

    public float[] AudioBandBuffer => audioBandBuffer;

    public float[] AudioBand64 => audioBand64;

    public float[] AudioBandBuffer64 => audioBandBuffer64;

    public float Amplitude => amplitude;

    public float AmplitudeBuffer => amplitudeBuffer;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(instance.gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        if (useAudioManager)
        {
            AudioManager.Instance.AddOnMusicChange(delegate { SetAudioSource(AudioManager.Instance.GetActiveTrack()); });
        }

        audioBand = new float[8];
        audioBandBuffer = new float[8];

        audioBand64 = new float[64];
        audioBandBuffer64 = new float[64];

        AudioProfile(audioProfile);
    }

    // Update is called once per frame
    void Update()
    {
        if (audioSource == null)
            return;

        GetSpectrumAudioSource();
        MakeFrequencyBands();
        MakeFrequencyBands64();
        BandBuffer();
        BandBuffer64();
        CreateAudioBands();
        CreateAudioBands64();
        GetAmplitude();
    }

    public void SetAudioSource(AudioSource otherAudioSource)
    {
        audioSource = otherAudioSource;
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

    void CreateAudioBands64()
    {
        for (int i = 0; i < 64; i++)
        {
            if (freqBand64[i] > freqBandHighest64[i])
            {
                freqBandHighest64[i] = freqBand64[i];
            }
            audioBand64[i] = (freqBand64[i] / freqBandHighest64[i]);
            audioBandBuffer64[i] = (bandBuffer64[i] / freqBandHighest64[i]);
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

    void BandBuffer64()
    {
        for (int i = 0; i < 64; i++)
        {
            if (freqBand64[i] > bandBuffer64[i])
            {
                bandBuffer64[i] = freqBand64[i];
                bufferDecrease64[i] = 0.005f;
            }

            if (freqBand64[i] < bandBuffer64[i])
            {
                bandBuffer64[i] -= bufferDecrease64[i];
                bufferDecrease64[i] *= 1.2f;
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

    void MakeFrequencyBands64()
    {

        int count = 0;
        int sampleCount = 1;
        int power = 0;

        for (int i = 0; i < 64; i++)
        {
            float average = 0;

            if (i == 16 || i == 32 || i == 40 || i == 48 || i == 56)
            {
                power++;
                sampleCount = (int)Mathf.Pow(2, power);

                if (power == 3)
                {
                    sampleCount -= 2;
                }
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

            freqBand64[i] = average * 80;
        }
    }
}
