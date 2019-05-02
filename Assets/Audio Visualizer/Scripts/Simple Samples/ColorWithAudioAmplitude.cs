using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorWithAudioAmplitude : MonoBehaviour
{
    [SerializeField] Material material;
    Material materialInstance;
    [ColorUsage(true, true)] public Color customColor = new Color(3, 1, 0, 1);
    public bool useBuffer = true;
    MeshRenderer meshRenderer;

    // Start is called before the first frame update
    void Start()
    {
        materialInstance = new Material(material);
        meshRenderer = GetComponent<MeshRenderer>();
        meshRenderer.sharedMaterial = materialInstance;
    }

    // Update is called once per frame
    void Update()
    {
        if (useBuffer)
        {
            Color color = new Color(
                                        customColor.r * AudioVisualizer.instance.amplitudeBuffer,
                                        customColor.g * AudioVisualizer.instance.amplitudeBuffer,
                                        customColor.b * AudioVisualizer.instance.amplitudeBuffer
                                    );

            materialInstance.SetColor("_EmissionColor", color);
        }
        else
        {
            Color color = new Color(
                                        customColor.r * AudioVisualizer.instance.amplitude,
                                        customColor.g * AudioVisualizer.instance.amplitude,
                                        customColor.b * AudioVisualizer.instance.amplitude
                                    );

            materialInstance.SetColor("_EmissionColor", color);
        }
    }
}
