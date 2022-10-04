using UnityEngine;

public class ColorWithAudioAmplitude : MonoBehaviour
{
    [SerializeField] Material material;
    Material materialInstance;
    [GradientUsage(true)] public Gradient customGradient;
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
            Color color = customGradient.Evaluate(AudioVisualizer.instance.AmplitudeBuffer);

            materialInstance.SetColor("_EmissionColor", color);
        }
        else
        {
            Color color = customGradient.Evaluate(AudioVisualizer.instance.Amplitude);

            materialInstance.SetColor("_EmissionColor", color);
        }
    }
}
