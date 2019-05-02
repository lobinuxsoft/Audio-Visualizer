using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateFromAudio : MonoBehaviour
{
    public AudioVisualizer audioVisualizer;
    public Vector3 rotateAxis, rotateSpeed;

    // Update is called once per frame
    void Update()
    {
        this.transform.Rotate(
                                rotateAxis.x * rotateSpeed.x * Time.deltaTime * audioVisualizer.amplitudeBuffer,
                                rotateAxis.y * rotateSpeed.y * Time.deltaTime * audioVisualizer.amplitudeBuffer,
                                rotateAxis.z * rotateSpeed.z * Time.deltaTime * audioVisualizer.amplitudeBuffer,
                                Space.World
                              );
    }
}
