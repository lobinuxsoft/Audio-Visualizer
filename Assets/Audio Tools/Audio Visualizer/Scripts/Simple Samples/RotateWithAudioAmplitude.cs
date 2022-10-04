using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateWithAudioAmplitude : MonoBehaviour
{
    //public AudioVisualizer audioVisualizer;
    public bool useBuffer = false;
    public Vector3 rotateAxis, rotateSpeed;

    // Update is called once per frame
    void Update()
    {
        if (useBuffer)
        {
            this.transform.Rotate(
                                rotateAxis.x * rotateSpeed.x * Time.deltaTime * AudioVisualizer.instance.AmplitudeBuffer,
                                rotateAxis.y * rotateSpeed.y * Time.deltaTime * AudioVisualizer.instance.AmplitudeBuffer,
                                rotateAxis.z * rotateSpeed.z * Time.deltaTime * AudioVisualizer.instance.AmplitudeBuffer,
                                Space.Self
                              );
        }
        else
        {
            this.transform.Rotate(
                                rotateAxis.x * rotateSpeed.x * Time.deltaTime * AudioVisualizer.instance.Amplitude,
                                rotateAxis.y * rotateSpeed.y * Time.deltaTime * AudioVisualizer.instance.Amplitude,
                                rotateAxis.z * rotateSpeed.z * Time.deltaTime * AudioVisualizer.instance.Amplitude,
                                Space.Self
                              );
        }
        
    }
}
