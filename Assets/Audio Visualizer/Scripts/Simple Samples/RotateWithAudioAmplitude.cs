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
                                rotateAxis.x * rotateSpeed.x * Time.deltaTime * AudioVisualizer.instance.amplitudeBuffer,
                                rotateAxis.y * rotateSpeed.y * Time.deltaTime * AudioVisualizer.instance.amplitudeBuffer,
                                rotateAxis.z * rotateSpeed.z * Time.deltaTime * AudioVisualizer.instance.amplitudeBuffer,
                                Space.Self
                              );
        }
        else
        {
            this.transform.Rotate(
                                rotateAxis.x * rotateSpeed.x * Time.deltaTime * AudioVisualizer.instance.amplitude,
                                rotateAxis.y * rotateSpeed.y * Time.deltaTime * AudioVisualizer.instance.amplitude,
                                rotateAxis.z * rotateSpeed.z * Time.deltaTime * AudioVisualizer.instance.amplitude,
                                Space.Self
                              );
        }
        
    }
}
