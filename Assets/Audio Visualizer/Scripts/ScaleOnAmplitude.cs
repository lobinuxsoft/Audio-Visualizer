using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleOnAmplitude : MonoBehaviour
{
    public float startScale = 5, maxScale = 30;
    public bool useBuffer = true;
    Material material;
    [ColorUsage(true, true)] public Color customColor = new Color(3, 1, 0, 1);

    // Start is called before the first frame update
    void Start()
    {
        material = GetComponent<MeshRenderer>().materials[0];
    }

    // Update is called once per frame
    void Update()
    {
        if (useBuffer)
        {
            transform.localScale = new Vector3(
                                                (AudioVisualizer.amplitudeBuffer * maxScale) + startScale,
                                                (AudioVisualizer.amplitudeBuffer * maxScale) + startScale,
                                                (AudioVisualizer.amplitudeBuffer * maxScale) + startScale
                                               );

            Color color = new Color(
                                        customColor.r * AudioVisualizer.amplitudeBuffer,
                                        customColor.g * AudioVisualizer.amplitudeBuffer,
                                        customColor.b * AudioVisualizer.amplitudeBuffer
                                    );

            material.SetColor("_EmissionColor", color);
        }
        else
        {
            transform.localScale = new Vector3(
                                                (AudioVisualizer.amplitude * maxScale) + startScale,
                                                (AudioVisualizer.amplitude * maxScale) + startScale,
                                                (AudioVisualizer.amplitude * maxScale) + startScale
                                               );

            Color color = new Color(
                                        customColor.r * AudioVisualizer.amplitude,
                                        customColor.r * AudioVisualizer.amplitude,
                                        customColor.r * AudioVisualizer.amplitude
                                    );

            material.SetColor("_EmissionColor", color);
        }
    }
}
