using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateFromAudio : MonoBehaviour
{
    public AudioVisualizer audioVisualizer;
    public bool useBuffer = false;
    public Vector3 rotateAxis, rotateSpeed;

    // Update is called once per frame
    void Update()
    {
        if (useBuffer)
        {
            this.transform.Rotate(
                                rotateAxis.x * rotateSpeed.x * Time.deltaTime * audioVisualizer.amplitudeBuffer,
                                rotateAxis.y * rotateSpeed.y * Time.deltaTime * audioVisualizer.amplitudeBuffer,
                                rotateAxis.z * rotateSpeed.z * Time.deltaTime * audioVisualizer.amplitudeBuffer,
                                Space.World
                              );
        }
        else
        {
            this.transform.Rotate(
                                rotateAxis.x * rotateSpeed.x * Time.deltaTime * audioVisualizer.amplitude,
                                rotateAxis.y * rotateSpeed.y * Time.deltaTime * audioVisualizer.amplitude,
                                rotateAxis.z * rotateSpeed.z * Time.deltaTime * audioVisualizer.amplitude,
                                Space.World
                              );
        }
        
    }
}
