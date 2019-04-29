using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Instantiate512Cubes : MonoBehaviour
{
    public GameObject sampleCubePrefab;
    public float maxScale = 1000f;
    GameObject[] sampleCubes = new GameObject[8];
    [GradientUsage(true)] public Gradient customGradient;
    Material[] materials = new Material[8];
    Color[] colors = new Color[8];

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < 8; i++)
        {
            GameObject go = Instantiate(sampleCubePrefab, this.transform.position, Quaternion.identity, this.transform);
            go.name = "Sample Cube ID: " + i;
            this.transform.eulerAngles = new Vector3(0, -45f * i, 0);
            
            go.transform.position = Vector3.forward * 50;
            go.transform.position += this.transform.position;
            sampleCubes[i] = go;
            materials[i] = go.GetComponent<MeshRenderer>().materials[0];
            colors[i] = customGradient.Evaluate((float)i / 8);
            materials[i].SetColor("_Color", colors[i]);
        }
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < 8; i++)
        {
            sampleCubes[i].transform.localScale = new Vector3(10, (AudioVisualizer.audioBand[i] * maxScale) + 2, 10);
            Color color = colors[i] * AudioVisualizer.audioBand[i];
            materials[i].SetColor("_EmissionColor", color * 2);
        }
    }
}
