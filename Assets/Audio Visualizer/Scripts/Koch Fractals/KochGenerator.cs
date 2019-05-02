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

    [SerializeField] Color gizmosColor = Color.white;

    protected enum InitiatorAxis
    {
        XAxis,
        YAxis,
        ZAxis
    }

    [SerializeField] protected InitiatorAxis axis = new InitiatorAxis();

    protected enum Initiator
    {
        Triangle,
        Square,
        Pentagon,
        Hexagon,
        Heptagon,
        Octagon
    }

    [SerializeField] protected Initiator initiator = new Initiator();

    [SerializeField] protected AnimationCurve generator;
    protected Keyframe[] keys;
    protected int generationCount;

    protected int initiatorPointAmount;
    Vector3[] initiatorPoint;
    Vector3 rotateVector;
    Vector3 rotateAxis;
    float initialRotation;
    [SerializeField] protected float initiatorSize;

    protected Vector3[] _positions;
    protected Vector3[] _targetPositions;
    List<LineSegment> lineSegments;

    private void Awake()
    {
        GetInitiatorPoints();

        //Assing lists and arrays
        _positions = new Vector3[initiatorPointAmount + 1];
        _targetPositions = new Vector3[initiatorPointAmount + 1];
        lineSegments = new List<LineSegment>();
        keys = generator.keys;

        rotateVector = Quaternion.AngleAxis(initialRotation, rotateAxis) * rotateVector;

        for (int i = 0; i < initiatorPointAmount; i++)
        {
            _positions[i] = rotateVector * initiatorSize;
            rotateVector = Quaternion.AngleAxis(360 / initiatorPointAmount, rotateAxis) * rotateVector;
        }

        _positions[initiatorPointAmount] = _positions[0];
        _targetPositions = _positions;
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

            for (int j = 1; j < keys.Length - 1; j++)
            {
                float moveAmount = lineSegments[i].Length * keys[j].time;
                float heightAmount = (lineSegments[i].Length * keys[j].value) * generatorMultiplier;
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

        generationCount++;
    }

    private void OnDrawGizmos()
    {
        GetInitiatorPoints();
        initiatorPoint = new Vector3[initiatorPointAmount];

        rotateVector = Quaternion.AngleAxis(initialRotation, rotateAxis) * rotateVector;

        for (int i = 0; i < initiatorPointAmount; i++)
        {
            initiatorPoint[i] = rotateVector * initiatorSize;
            rotateVector = Quaternion.AngleAxis(360 / initiatorPointAmount, rotateAxis) * rotateVector;
        }
        for (int i = 0; i < initiatorPointAmount; i++)
        {
            Gizmos.color = gizmosColor;
            Matrix4x4 rotationMatrix = Matrix4x4.TRS(transform.position, transform.rotation, transform.lossyScale);
            Gizmos.matrix = rotationMatrix;

            if (i < initiatorPointAmount - 1)
            {
                Gizmos.DrawSphere(initiatorPoint[i], .25f);
                Gizmos.DrawLine(initiatorPoint[i], initiatorPoint[i + 1]);
            }
            else
            {
                Gizmos.DrawSphere(initiatorPoint[i], .25f);
                Gizmos.DrawLine(initiatorPoint[i], initiatorPoint[0]);
            }
        }

    }

    void GetInitiatorPoints()
    {
        switch (initiator)
        {
            case Initiator.Triangle:
                initiatorPointAmount = 3;
                initialRotation = 0;
                break;
            case Initiator.Square:
                initiatorPointAmount = 4;
                initialRotation = 45;
                break;
            case Initiator.Pentagon:
                initiatorPointAmount = 5;
                initialRotation = 36;
                break;
            case Initiator.Hexagon:
                initiatorPointAmount = 6;
                initialRotation = 30;
                break;
            case Initiator.Heptagon:
                initiatorPointAmount = 7;
                initialRotation = 25.71428f;
                break;
            case Initiator.Octagon:
                initiatorPointAmount = 8;
                initialRotation = 22.5f;
                break;
            default:
                initiatorPointAmount = 3;
                initialRotation = 0;
                break;
        }

        switch (axis)
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
