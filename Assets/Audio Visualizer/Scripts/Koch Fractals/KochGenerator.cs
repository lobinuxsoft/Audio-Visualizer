using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KochGenerator : MonoBehaviour
{
    public struct LineSegment
    {
        public Vector3 StartPosition { get; set; }
        public Vector3 EndPosition { get; set; }
        public Vector3 Direction { get; set; }
        public float Length { get; set; }
    }

    protected enum InitiatorAxis
    {
        XAxis,
        YAxis,
        ZAxis
    }

    [SerializeField] protected InitiatorAxis _axis = new InitiatorAxis();

    protected enum Initiator
    {
        Triangle,
        Square,
        Pentagon,
        Hexagon,
        Heptagon,
        Octagon
    }

    [SerializeField] protected Initiator _initiator = new Initiator();

    [SerializeField] protected AnimationCurve _generator;

    [System.Serializable]
    public struct StartGen
    {
        public bool outwards;
        public float scale;
    }

    public StartGen[] _startGens;

    protected Keyframe[] _keys;

    [SerializeField] protected bool _useBezierCurve;
    [SerializeField] [Range(8, 24)] protected int _bezierVertexCount;

    protected int _generationCount;

    protected int _initiatorPointAmount;
    Vector3[] initiatorPoint;
    Vector3 rotateVector;
    Vector3 rotateAxis;
    float initialRotation;
    [SerializeField] protected float _initiatorSize;

    protected Vector3[] _positions;
    protected Vector3[] _targetPositions;
    protected Vector3[] _bezierPosition;
    List<LineSegment> lineSegments;

    protected Vector3[] BezierCurve(Vector3[] points, int vertexCount)
    {
        var pointList = new List<Vector3>();
        for (int i = 0; i < points.Length; i += 2)
        {
            if (i+2 <= points.Length - 1)
            {
                for (float ratio = 0; ratio <= 1f; ratio += 1.0f / vertexCount)
                {
                    var tangentLineVertex1 = Vector3.Lerp(points[i], points[i + 1], ratio);
                    var tangentLineVertex2 = Vector3.Lerp(points[i + 1], points[i + 2], ratio);
                    var bezierpoint = Vector3.Lerp(tangentLineVertex1, tangentLineVertex2, ratio);
                    pointList.Add(bezierpoint);
                }
            }
        }
        return pointList.ToArray();
    }

    private void Awake()
    {
        GetInitiatorPoints();

        //Assing lists and arrays
        _positions = new Vector3[_initiatorPointAmount + 1];
        _targetPositions = new Vector3[_initiatorPointAmount + 1];
        lineSegments = new List<LineSegment>();
        _keys = _generator.keys;

        rotateVector = Quaternion.AngleAxis(initialRotation, rotateAxis) * rotateVector;

        for (int i = 0; i < _initiatorPointAmount; i++)
        {
            _positions[i] = rotateVector * _initiatorSize;
            rotateVector = Quaternion.AngleAxis(360 / _initiatorPointAmount, rotateAxis) * rotateVector;
        }

        _positions[_initiatorPointAmount] = _positions[0];
        _targetPositions = _positions;

        for (int i = 0; i < _startGens.Length; i++)
        {
            KochGenerate(_targetPositions, _startGens[i].outwards, _startGens[i].scale);
        }
    }

    protected void KochGenerate(Vector3[] positions, bool outwards, float generatorMultiplier)
    {
        //Creating line segments
        lineSegments.Clear();
        for (int i = 0; i < positions.Length - 1; i++)
        {
            LineSegment line = new LineSegment();
            line.StartPosition = positions[i];

            if (i == positions.Length - 1)
            {
                line.EndPosition = positions[0];
            }
            else
            {
                line.EndPosition = positions[i + 1];
            }

            line.Direction = (line.EndPosition - line.StartPosition).normalized;
            line.Length = Vector3.Distance(line.EndPosition, line.StartPosition);
            lineSegments.Add(line);
        }

        //Add the line segment points to a point array
        List<Vector3> newPos = new List<Vector3>();
        List<Vector3> targetPos = new List<Vector3>();

        for (int i = 0; i < lineSegments.Count; i++)
        {
            newPos.Add(lineSegments[i].StartPosition);
            targetPos.Add(lineSegments[i].StartPosition);

            for (int j = 1; j < _keys.Length - 1; j++)
            {
                float moveAmount = lineSegments[i].Length * _keys[j].time;
                float heightAmount = (lineSegments[i].Length * _keys[j].value) * generatorMultiplier;
                Vector3 movePos = lineSegments[i].StartPosition + (lineSegments[i].Direction * moveAmount);
                Vector3 Dir;

                if (outwards)
                {
                    Dir = Quaternion.AngleAxis(-90, rotateAxis) * lineSegments[i].Direction;
                }
                else
                {
                    Dir = Quaternion.AngleAxis(90, rotateAxis) * lineSegments[i].Direction;
                }
                newPos.Add(movePos);
                targetPos.Add(movePos + (Dir * heightAmount));
            }
        }

        newPos.Add(lineSegments[0].StartPosition);
        targetPos.Add(lineSegments[0].StartPosition);
        _positions = new Vector3[newPos.Count];
        _targetPositions = new Vector3[targetPos.Count];
        _positions = newPos.ToArray();
        _targetPositions = targetPos.ToArray();
        _bezierPosition = BezierCurve(_targetPositions, _bezierVertexCount);
        _generationCount++;
    }

    public float _lengthOfSide;

    private void OnDrawGizmos()
    {
        GetInitiatorPoints();
        initiatorPoint = new Vector3[_initiatorPointAmount];

        rotateVector = Quaternion.AngleAxis(initialRotation, rotateAxis) * rotateVector;

        for (int i = 0; i < _initiatorPointAmount; i++)
        {
            initiatorPoint[i] = rotateVector * _initiatorSize;
            rotateVector = Quaternion.AngleAxis(360 / _initiatorPointAmount, rotateAxis) * rotateVector;
        }
        for (int i = 0; i < _initiatorPointAmount; i++)
        {
            Gizmos.color = Color.white;
            Matrix4x4 rotationMatrix = Matrix4x4.TRS(transform.position, transform.rotation, transform.lossyScale);
            Gizmos.matrix = rotationMatrix;

            if (i < _initiatorPointAmount - 1)
            {
                Gizmos.DrawSphere(initiatorPoint[i], .1f);
                Gizmos.DrawLine(initiatorPoint[i], initiatorPoint[i + 1]);
            }
            else
            {
                Gizmos.DrawSphere(initiatorPoint[i], .1f);
                Gizmos.DrawLine(initiatorPoint[i], initiatorPoint[0]);
            }
        }

        _lengthOfSide = Vector3.Distance(initiatorPoint[0], initiatorPoint[1]) * .5f;
    }

    void GetInitiatorPoints()
    {
        switch (_initiator)
        {
            case Initiator.Triangle:
                _initiatorPointAmount = 3;
                initialRotation = 0;
                break;
            case Initiator.Square:
                _initiatorPointAmount = 4;
                initialRotation = 45;
                break;
            case Initiator.Pentagon:
                _initiatorPointAmount = 5;
                initialRotation = 36;
                break;
            case Initiator.Hexagon:
                _initiatorPointAmount = 6;
                initialRotation = 30;
                break;
            case Initiator.Heptagon:
                _initiatorPointAmount = 7;
                initialRotation = 25.71428f;
                break;
            case Initiator.Octagon:
                _initiatorPointAmount = 8;
                initialRotation = 22.5f;
                break;
            default:
                _initiatorPointAmount = 3;
                initialRotation = 0;
                break;
        }

        switch (_axis)
        {
            case InitiatorAxis.XAxis:
                rotateVector = Vector3.right;
                rotateAxis = Vector3.forward;
                break;
            case InitiatorAxis.YAxis:
                rotateVector = Vector3.up;
                rotateAxis = Vector3.right;
                break;
            case InitiatorAxis.ZAxis:
                rotateVector = Vector3.forward;
                rotateAxis = Vector3.up;
                break;
            default:
                rotateVector = Vector3.up;
                rotateAxis = Vector3.right;
                break;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
