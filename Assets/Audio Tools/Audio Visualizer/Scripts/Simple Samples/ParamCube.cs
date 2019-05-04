using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParamCube : MonoBehaviour
{
    public int band;
    public float startScale, scaleMultiplier;
    public bool useBuffer = true;
    Material material;
    [ColorUsage(true, true)] public Color customColor = new Color(1f, 1f, 1f);

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
                                            transform.localScale.x,
                                            (AudioVisualizer.instance.audioBandBuffer[band] * scaleMultiplier) + startScale,
                                            transform.localScale.z
                                           );

            Color color = new Color(
                                        customColor.r * AudioVisualizer.instance.audioBandBuffer[band],
                                        customColor.g * AudioVisualizer.instance.audioBandBuffer[band],
                                        customColor.b * AudioVisualizer.instance.audioBandBuffer[band]
                                    );

            material.SetColor("_EmissionColor", color);
        }
        else
        {
            transform.localScale = new Vector3(
                                            transform.localScale.x,
                                            (AudioVisualizer.instance.audioBand[band] * scaleMultiplier) + startScale,
                                            transform.localScale.z
                                           );

            Color color = new Color(
                                        customColor.r * AudioVisualizer.instance.audioBand[band],
                                        customColor.g * AudioVisualizer.instance.audioBand[band],
                                        customColor.b * AudioVisualizer.instance.audioBand[band]
                                    );
            material.SetColor("_EmissionColor", color);
        }
           
    }
}
