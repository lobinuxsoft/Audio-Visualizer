using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParamCube : MonoBehaviour
{
    public int band;
    public float startScale, scaleMultiplier;
    public bool useBuffer = true;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (useBuffer)
        {
            transform.localScale = new Vector3(
                                            transform.localScale.x,
                                            (AudioVisualizer.bandBuffer[band] * scaleMultiplier) + startScale,
                                            transform.localScale.z
                                           );
        }
        else
        {
            transform.localScale = new Vector3(
                                            transform.localScale.x,
                                            (AudioVisualizer.freqBand[band] * scaleMultiplier) + startScale,
                                            transform.localScale.z
                                           );
        }
           
    }
}
