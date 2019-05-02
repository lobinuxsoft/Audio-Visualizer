using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Instantiate64Cubes : MonoBehaviour
{
    public GameObject sampleCubePrefab;
    public Vector3 defaultObjectScale = Vector3.one;
    public float maxScale = 1000f;
    public float maxRadius = 50f;
    public bool useBuffer = true;
    GameObject[] sampleCubes = new GameObject[64];
    [GradientUsage(true)] public Gradient customGradient;
    public float colorMultiplier = 2f;
    Material[] materials = new Material[64];
    Color[] colors = new Color[64];


    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < 64; i++)
        {
            GameObject go = Instantiate(sampleCubePrefab, this.transform.position, Quaternion.identity, this.transform);
            go.name = "Sample Cube ID: " + i;
            this.transform.eulerAngles = new Vector3(0, -5.625f * i, 0);
            
            go.transform.position = Vector3.forward * maxRadius;
            go.transform.position += this.transform.position;
            sampleCubes[i] = go;
            materials[i] = go.GetComponent<MeshRenderer>().materials[0];
            colors[i] = customGradient.Evaluate((float)i / 64);
            //materials[i].SetColor("_Color", colors[i]);
            materials[i].SetColor("_Color", Color.black);
        }
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < 64; i++)
        {
            if (useBuffer)
            {
                sampleCubes[i].transform.localScale = new Vector3(defaultObjectScale.x, (AudioVisualizer.instance.audioBandBuffer64[i] * maxScale) + defaultObjectScale.y, defaultObjectScale.z);
                Color color = colors[i] * AudioVisualizer.instance.audioBandBuffer64[i];
                materials[i].SetColor("_EmissionColor", color * colorMultiplier);
            }
            else
            {
                sampleCubes[i].transform.localScale = new Vector3(defaultObjectScale.x, (AudioVisualizer.instance.audioBand64[i] * maxScale) + defaultObjectScale.y, defaultObjectScale.z);
                Color color = colors[i] * AudioVisualizer.instance.audioBand64[i];
                materials[i].SetColor("_EmissionColor", color * colorMultiplier);
            }
        }
    }
}
