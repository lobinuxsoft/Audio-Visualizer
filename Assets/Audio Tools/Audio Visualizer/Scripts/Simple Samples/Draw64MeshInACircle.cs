using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Draw64MeshInACircle : MonoBehaviour
{
    public Mesh mesh;
    public Material material;
    public Vector3 defaultMeshScale = Vector3.one;
    public float maxScale = 1000f;
    public float maxRadius = 50f;
    public bool useBuffer = true;

    Vector3[] positions = new Vector3[64];
    Matrix4x4[] matrixs = new Matrix4x4[64];

    [GradientUsage(true)] public Gradient customGradient;
    public float colorMultiplier = 2f;
    Material[] materials = new Material[64];
    Color[] colors = new Color[64];


    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < 64; i++)
        {
            materials[i] = new Material(material);
            colors[i] = customGradient.Evaluate((float)i / 64);
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
                positions[i] = PosInCircle(this.transform, maxRadius, (float)i / 64);

                Color color = colors[i] * AudioVisualizer.instance.AudioBandBuffer64[i];
                materials[i].SetColor("_EmissionColor", color * colorMultiplier);
                matrixs[i] = Matrix4x4.TRS(positions[i], this.transform.rotation, new Vector3(defaultMeshScale.x, (AudioVisualizer.instance.AudioBandBuffer64[i] * maxScale) + defaultMeshScale.y, defaultMeshScale.z));
                Graphics.DrawMesh(mesh,matrixs[i], materials[i], 0);

            }
            else
            {
                positions[i] = PosInCircle(this.transform, maxRadius, (float)i / 64);

                Color color = colors[i] * AudioVisualizer.instance.AudioBand64[i];
                materials[i].SetColor("_EmissionColor", color * colorMultiplier);
                matrixs[i] = Matrix4x4.TRS(positions[i], this.transform.rotation, new Vector3(defaultMeshScale.x, (AudioVisualizer.instance.AudioBand64[i] * maxScale) + defaultMeshScale.y, defaultMeshScale.z));
                Graphics.DrawMesh(mesh, matrixs[i], materials[i], 0);
            }
        }
    }

    Vector3 PosInCircle(Transform center, float radius, float anglePercent)
    {
        float ang = anglePercent * 360;
        Vector3 pos;
        pos = center.right * radius * Mathf.Sin(ang * Mathf.Deg2Rad);
        pos += center.forward * radius * Mathf.Cos(ang * Mathf.Deg2Rad);
        pos += center.position;
        return pos;
    }
}
