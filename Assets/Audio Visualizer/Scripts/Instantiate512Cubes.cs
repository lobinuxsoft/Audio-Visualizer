using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Instantiate512Cubes : MonoBehaviour
{
    public GameObject sampleCubePrefab;
    public float maxScale = 1000f;
    GameObject[] sampleCubes = new GameObject[512];

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < 512; i++)
        {
            GameObject go = Instantiate(sampleCubePrefab, this.transform.position, Quaternion.identity, this.transform);
            go.name = "Sample Cube ID: " + i;
            this.transform.eulerAngles = new Vector3(0, -0.703125f * i, 0);
            go.transform.position = Vector3.forward * 100;
            sampleCubes[i] = go;
        }
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < 512; i++)
        {
            sampleCubes[i].transform.localScale = new Vector3(10, (AudioVisualizer.samples[i] * maxScale) + 2, 10);
        }
    }
}
