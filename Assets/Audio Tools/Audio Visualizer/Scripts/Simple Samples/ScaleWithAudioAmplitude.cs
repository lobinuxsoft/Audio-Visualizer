using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleWithAudioAmplitude : MonoBehaviour
{
    public float startScale = 5, maxScale = 30;
    public bool useBuffer = true;

    // Update is called once per frame
    void Update()
    {
        if (useBuffer)
        {
            transform.localScale = new Vector3(
                                                (AudioVisualizer.instance.AmplitudeBuffer * maxScale) + startScale,
                                                (AudioVisualizer.instance.AmplitudeBuffer * maxScale) + startScale,
                                                (AudioVisualizer.instance.AmplitudeBuffer * maxScale) + startScale
                                               );
        }
        else
        {
            transform.localScale = new Vector3(
                                                (AudioVisualizer.instance.Amplitude * maxScale) + startScale,
                                                (AudioVisualizer.instance.Amplitude * maxScale) + startScale,
                                                (AudioVisualizer.instance.Amplitude * maxScale) + startScale
                                               );
        }
    }
}
