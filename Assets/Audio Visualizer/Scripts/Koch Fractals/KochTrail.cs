using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KochTrail : KochGenerator
{
    public class TrailObject
    {
        public GameObject GO { get; set; }
        public TrailRenderer Trail { get; set; }
        public int CurrentTargetNum { get; set; }
        public Vector3 TargetPosition { get; set; }
        public Color EmissionColor { get; set; }
    }

    [HideInInspector] public List<TrailObject> trails;

    [Header("Trail Properties")]
    public GameObject trailPrefab;
    public AnimationCurve trailWidthCurve;
    [Range(0,8)] public int trailEndCapVertices;
    public Material trailMaterial;
    public Gradient trailColor;

    [Header("Audio")]
    public AudioVisualizer audioVisualizer;
    public bool useBuffer;
    public int[] audioBand;
    public Vector2 speedMinMax, widthMinMax, trailTimeMinMax;
    public float colorMultiplier;

    float lerpPosSpeed;
    float distanceSnap;
    Color startColor, endColor;

    // Start is called before the first frame update
    void Start()
    {
        Initialize();
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
        AudioBehaviour();
    }

    void Initialize()
    {
        if (!audioVisualizer)
        {
            audioVisualizer = AudioVisualizer.instance;
        }

        startColor = new Color(0, 0, 0, 0);
        endColor = new Color(0, 0, 0, 1);

        trails = new List<TrailObject>();

        for (int i = 0; i < _initiatorPointAmount; i++)
        {
            GameObject trailInstance = Instantiate(trailPrefab, transform.position, Quaternion.identity, this.transform);
            TrailObject trailObjectInstance = new TrailObject();
            trailObjectInstance.GO = trailInstance;
            trailObjectInstance.Trail = trailInstance.GetComponent<TrailRenderer>();
            trailObjectInstance.Trail.material = new Material(trailMaterial);
            trailObjectInstance.EmissionColor = trailColor.Evaluate(i * (1.0f / _initiatorPointAmount));
            trailObjectInstance.Trail.numCapVertices = trailEndCapVertices;
            trailObjectInstance.Trail.widthCurve = trailWidthCurve;

            Vector3 instantiatePosition;

            if (_generationCount > 0)
            {
                int step;
                if (_useBezierCurve)
                {
                    step = _bezierPosition.Length / _initiatorPointAmount;
                    instantiatePosition = _bezierPosition[i * step];
                    trailObjectInstance.CurrentTargetNum = (i * step) + 1;
                    trailObjectInstance.TargetPosition = _bezierPosition[trailObjectInstance.CurrentTargetNum];
                }
                else
                {
                    step = _positions.Length / _initiatorPointAmount;
                    instantiatePosition = _positions[i * step];
                    trailObjectInstance.CurrentTargetNum = (i * step) + 1;
                    trailObjectInstance.TargetPosition = _positions[trailObjectInstance.CurrentTargetNum];
                }
            }
            else
            {
                instantiatePosition = _positions[i];
                trailObjectInstance.CurrentTargetNum = i + 1;
                trailObjectInstance.TargetPosition = _positions[trailObjectInstance.CurrentTargetNum];
            }

            trailObjectInstance.GO.transform.localPosition = instantiatePosition;
            trails.Add(trailObjectInstance);
        }
    }

    void Movement()
    {
        lerpPosSpeed = Mathf.Lerp(speedMinMax.x, speedMinMax.y, audioVisualizer.amplitude);
        for (int i = 0; i < trails.Count; i++)
        {
            
            distanceSnap = Vector3.Distance(trails[i].GO.transform.localPosition, trails[i].TargetPosition);

            if (distanceSnap < 0.05f)
            {
                trails[i].GO.transform.localPosition = trails[i].TargetPosition;
                if (_useBezierCurve && _generationCount > 0)
                {
                    if (trails[i].CurrentTargetNum < _bezierPosition.Length - 1)
                    {
                        trails[i].CurrentTargetNum += 1;
                    }
                    else
                    {
                        trails[i].CurrentTargetNum = 1;
                    }
                    trails[i].TargetPosition = _bezierPosition[trails[i].CurrentTargetNum];
                }
                else
                {
                    if (trails[i].CurrentTargetNum < _positions.Length - 1)
                    {
                        trails[i].CurrentTargetNum += 1;
                    }
                    else
                    {
                        trails[i].CurrentTargetNum = 1;
                    }
                    trails[i].TargetPosition = _targetPositions[trails[i].CurrentTargetNum];
                }
            }

            trails[i].GO.transform.localPosition = Vector3.MoveTowards(trails[i].GO.transform.localPosition, trails[i].TargetPosition, Time.deltaTime * lerpPosSpeed);
        }
    }

    void AudioBehaviour()
    {
        for (int i = 0; i < _initiatorPointAmount; i++)
        {
            Color colorLerp;
            float widthLerp, timeLerp;
            if (useBuffer)
            {
                colorLerp = Color.Lerp(startColor, trails[i].EmissionColor * colorMultiplier, audioVisualizer.audioBandBuffer[audioBand[i]]);
                trails[i].Trail.material.SetColor("_EmissionColor", colorLerp);
                colorLerp = Color.Lerp(startColor, endColor, audioVisualizer.audioBandBuffer[audioBand[i]]);
                trails[i].Trail.material.SetColor("_Color", colorLerp);

                widthLerp = Mathf.Lerp(widthMinMax.x, widthMinMax.y, audioVisualizer.audioBandBuffer[audioBand[i]]);
                trails[i].Trail.widthMultiplier = widthLerp;

                timeLerp = Mathf.Lerp(trailTimeMinMax.x, trailTimeMinMax.y, audioVisualizer.audioBandBuffer[audioBand[i]]);
                trails[i].Trail.time = timeLerp;
            }
            else
            {
                colorLerp = Color.Lerp(startColor, trails[i].EmissionColor * colorMultiplier, audioVisualizer.audioBand[audioBand[i]]);
                trails[i].Trail.material.SetColor("_EmissionColor", colorLerp);
                colorLerp = Color.Lerp(startColor, endColor, audioVisualizer.audioBand[audioBand[i]]);
                trails[i].Trail.material.SetColor("_Color", colorLerp);

                widthLerp = Mathf.Lerp(widthMinMax.x, widthMinMax.y, audioVisualizer.audioBand[audioBand[i]]);
                trails[i].Trail.widthMultiplier = widthLerp;

                timeLerp = Mathf.Lerp(trailTimeMinMax.x, trailTimeMinMax.y, audioVisualizer.audioBand[audioBand[i]]);
                trails[i].Trail.time = timeLerp;
            }
        }
    }
}
