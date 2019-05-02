using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class KochLine : KochGenerator
{
    LineRenderer lineRenderer;
    [Range(0f, 1f)] public float lerpAmount;
    Vector3[] lerpPositions;
    public float multiplier;

    // Start is called before the first frame update
    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.enabled = true;
        lineRenderer.useWorldSpace = false;
        lineRenderer.loop = true;
        lineRenderer.positionCount = _positions.Length;
        lineRenderer.SetPositions(_positions);
    }

    // Update is called once per frame
    void Update()
    {
        if (generationCount != 0)
        {
            for (int i = 0; i < _positions.Length; i++)
            {
                lerpPositions[i] = Vector3.Lerp(_positions[i], _targetPositions[i], lerpAmount);
            }

            lineRenderer.SetPositions(lerpPositions);
        }

        if (Input.GetKeyUp(KeyCode.O))
        {
            KochGenerate(_targetPositions, true, multiplier);
            lerpPositions = new Vector3[_positions.Length];
            lineRenderer.positionCount = _positions.Length;
            lineRenderer.SetPositions(_positions);
            lerpAmount = 0;
        }

        if (Input.GetKeyUp(KeyCode.I))
        {
            KochGenerate(_targetPositions, false, multiplier);
            lerpPositions = new Vector3[_positions.Length];
            lineRenderer.positionCount = _positions.Length;
            lineRenderer.SetPositions(_positions);
            lerpAmount = 0;
        }
    }
}
