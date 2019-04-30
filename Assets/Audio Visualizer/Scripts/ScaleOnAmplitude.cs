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
                                                (AudioVisualizer.instance.amplitudeBuffer * maxScale) + startScale,
                                                (AudioVisualizer.instance.amplitudeBuffer * maxScale) + startScale,
                                                (AudioVisualizer.instance.amplitudeBuffer * maxScale) + startScale
                                               );

            Color color = new Color(
                                        customColor.r * AudioVisualizer.instance.amplitudeBuffer,
                                        customColor.g * AudioVisualizer.instance.amplitudeBuffer,
                                        customColor.b * AudioVisualizer.instance.amplitudeBuffer
                                    );

            material.SetColor("_EmissionColor", color);
        }
        else
        {
            transform.localScale = new Vector3(
                                                (AudioVisualizer.instance.amplitude * maxScale) + startScale,
                                                (AudioVisualizer.instance.amplitude * maxScale) + startScale,
                                                (AudioVisualizer.instance.amplitude * maxScale) + startScale
                                               );

            Color color = new Color(
                                        customColor.r * AudioVisualizer.instance.amplitude,
                                        customColor.g * AudioVisualizer.instance.amplitude,
                                        customColor.b * AudioVisualizer.instance.amplitude
                                    );

            material.SetColor("_EmissionColor", color);
        }
    }
}
