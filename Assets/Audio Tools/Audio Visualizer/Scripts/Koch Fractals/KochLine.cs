using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class KochLine : KochGenerator
{
    LineRenderer lineRenderer;
    Vector3[] lerpPositions;
    //public float generateMultiplier;
    float[] lerpAudio;

    [Header("Audio")]
    public AudioVisualizer audioVisualizer;
    public bool useBuffer = false;
    public int[] audioBand;
    public Material material;
    [ColorUsage(true,true)] public Color color;
    Material materialInstance;
    public int audioBandMaterial;
    public float emissionMultiplier;

    // Start is called before the first frame update
    void Start()
    {
        if (!audioVisualizer)
        {
            audioVisualizer = AudioVisualizer.instance;
        }

        lerpAudio = new float[_initiatorPointAmount];
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.enabled = true;
        lineRenderer.useWorldSpace = false;
        lineRenderer.loop = true;
        lineRenderer.positionCount = _positions.Length;
        lineRenderer.SetPositions(_positions);
        lerpPositions = new Vector3[_positions.Length];

        //Apply material
        materialInstance = new Material(material);
        lineRenderer.material = materialInstance;
    }

    // Update is called once per frame
    void Update()
    {
        if (!audioVisualizer)
        {
            audioVisualizer = AudioVisualizer.instance;
        }

        if (useBuffer)
        {
            if(materialInstance != null)
                materialInstance.SetColor("_EmissionColor", color * audioVisualizer.audioBandBuffer[audioBandMaterial] * emissionMultiplier);
        }
        else
        {
            if (materialInstance != null)
                materialInstance.SetColor("_EmissionColor", color * audioVisualizer.audioBand[audioBandMaterial] * emissionMultiplier);
        }
        

        if (_generationCount != 0)
        {
            int count = 0;
            for (int i = 0; i < _initiatorPointAmount; i++)
            {
                if (useBuffer)
                {
                    lerpAudio[i] = audioVisualizer.audioBandBuffer[audioBand[i]];
                }
                else
                {
                    lerpAudio[i] = audioVisualizer.audioBand[audioBand[i]];
                }
                
                for (int j = 0; j < (_positions.Length - 1) / _initiatorPointAmount; j++)
                {
                    lerpPositions[count] = Vector3.Lerp(_positions[count], _targetPositions[count], lerpAudio[i]);
                    count++;
                }
            }
            lerpPositions[count] = Vector3.Lerp(_positions[count], _targetPositions[count], lerpAudio[_initiatorPointAmount - 1]);

            if (_useBezierCurve)
            {
                _bezierPosition = BezierCurve(lerpPositions, _bezierVertexCount);
                lineRenderer.positionCount = _bezierPosition.Length;
                lineRenderer.SetPositions(_bezierPosition);
            }
            else
            {
                lineRenderer.positionCount = lerpPositions.Length;
                lineRenderer.SetPositions(lerpPositions);
            }
            
        }
    }
}
