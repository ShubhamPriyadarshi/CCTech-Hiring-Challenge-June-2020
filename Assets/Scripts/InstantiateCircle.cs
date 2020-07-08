using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantiateCircle : MonoBehaviour
{

    Mesh m;
    MeshFilter mf;
    public Vector2 circlePos;
    public float radius;
    public float angle;
    public float clearance;
    public float lineLength;

    List<Vector3> circleVerteices = new List<Vector3>();
    List<Vector2> uvs = new List<Vector2>();
    List<int> triangles = new List<int>();
    public GameObject myPrefab;
    public GameObject linesPrefab;
    public Material [] mat;

    class Vector2D 
    {
        public double x;
        public double y;

        public Vector2D(double x, double y)
        {
            this.x = x;
            this.y = y;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<MeshRenderer>().material = mat[0];
        mf = GetComponent<MeshFilter>();
        m = new Mesh();
        mf.mesh = m;
        drawCircle();
        DrawLines();
        //GameObject circle;
        //circle = Instantiate(myPrefab, this.transform, true);
        //Mesh mesh = new Mesh(); 
        //mesh = GenerateCircleMesh();
        //circle.GetComponent<MeshFilter>().mesh = mesh;
        //circle.GetComponent<MeshRenderer>().material = mat;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void DrawLine(Vector2D start, Vector2D end, Material mat, float widthMultiplier, int sortingOrder = 2)
    {

        GameObject Line = Instantiate(linesPrefab);
        LineRenderer LineRend = Line.GetComponent<LineRenderer>();
        LineRend.positionCount = 2;
        LineRend.widthMultiplier = widthMultiplier;
        LineRend.material = mat;
        LineRend.sortingOrder = sortingOrder;
        LineRend.SetPosition(0, new Vector3(Convert.ToSingle(start.x), Convert.ToSingle(start.y)));
        LineRend.SetPosition(1, new Vector3(Convert.ToSingle(end.x), Convert.ToSingle(end.y)));

    }


    void DrawLines()
    {
        double angleRad = angle * Mathf.PI / 180f;
        double circlepoint_x = circlePos.x + ((radius) * Math.Cos(angleRad));
        double mid_x = circlePos.x + ((radius+clearance) * Math.Cos(angleRad));

        Debug.Log(Math.Cos(angleRad));
        double mid_y = circlePos.y + ((radius+clearance) * Math.Sin(angleRad));
        double circlepoint_y = circlePos.y + ((radius) * Math.Sin(angleRad));
        Vector2D circlePoint = new Vector2D(circlepoint_x, circlepoint_y);
        Vector2D lineSegmentMid = new Vector2D(mid_x, mid_y);
        Debug.Log(lineSegmentMid.x+" "+lineSegmentMid.y);
        Vector2D circlePosD = new Vector2D(circlePos.x, circlePos.y);
        DrawLine(circlePosD, circlePoint, mat[1], 0.1f);
        DrawLine(circlePoint, lineSegmentMid, mat[2], 0.1f);

        double slope = (mid_y - circlePos.y) / (mid_x - circlePos.x);
        double slopeInverse = -1 / slope;        

        //
        Vector2D[] lineSegment = GetLineCoordinates(mid_x, mid_y, mid_x, mid_y, slopeInverse, lineLength/2);
        Debug.Log(lineSegment[0].x + " " + lineSegment[0].y);
        Debug.Log(lineSegment[1].x + " " + lineSegment[1].y);
        DrawLine((lineSegment[0]), lineSegment[1], mat[3], 0.27f);
        Vector2D[] chordCoordinates = new Vector2D[2];
        Vector2D[] tempLine;
        Debug.Log("LS 1 B" + lineSegment[0].x);
        tempLine = GetLineCoordinates(lineSegment[0].x,lineSegment[0].y, circlePosD.x, circlePos.y, slope, radius); // for lower coordinate
        chordCoordinates[0] = new Vector2D(tempLine[0].x, tempLine[0].y);
        Debug.Log("LS 1" + lineSegment[0].x);
        tempLine = GetLineCoordinates(lineSegment[1].x, lineSegment[1].y, circlePosD.x, circlePos.y, slope, radius); // for upper coordinate
        chordCoordinates[1] = new Vector2D(tempLine[0].x, tempLine[0].y);
        Debug.Log(chordCoordinates[0].x + " " + chordCoordinates[0].y);
        Debug.Log(chordCoordinates[1].y + " " + chordCoordinates[1].y);

        DrawLine(lineSegment[0], chordCoordinates[0],mat[4], 0.05f);
       // DrawLine(chordCoordinates[0], circlePosD, mat[1], 0.07f);
        DrawLine(lineSegment[1], chordCoordinates[1],mat[4], 0.05f);
        //DrawLine(chordCoordinates[1], circlePosD, mat[1], 0.07f);

        DrawLine(chordCoordinates[0], chordCoordinates[1], mat[5], 0.12f, 3);//chord



    }

    Vector2D[] GetLineCoordinates(double x, double y, double a, double b, double m, double r)
    {
        Vector2D[] lineSegment = new Vector2D[2] {new Vector2D(0,0), new Vector2D(0, 0)};

        double c = y - m * x;
        Debug.Log(c);
        double d = (Math.Pow(r, 2) * (1 + Math.Pow(m, 2)) - Math.Pow((b - m * a - c), 2));
        Debug.Log(d);
        lineSegment[0].x = (a + b * m - c * m + Math.Sqrt(d)) / (1 + Math.Pow(m, 2));//lower x
        lineSegment[1].x = (a + b * m - c * m - Math.Sqrt(d)) / (1 + Math.Pow(m, 2));
        lineSegment[0].y = (c + a * m + b * Math.Pow(m, 2) + m * Math.Sqrt(d)) / (1 + Math.Pow(m, 2));// lower y
        lineSegment[1].y = (c + a * m + b * Math.Pow(m, 2) - m * Math.Sqrt(d)) / (1 + Math.Pow(m, 2));



        return lineSegment;
    }



    void drawCircle()
    {
        float val = Mathf.PI / 180f;//one degree = val radians
        //float radius = 1f;
        int deltaAngle = 2;

        Vector3 center = Vector3.zero;
        circleVerteices.Add(center);
        uvs.Add(new Vector2(0.5f, 0.5f));
        int triangleCount = 0;

        float x1 = radius * Mathf.Cos(0);
        float y1 = radius * Mathf.Sin(0);
        float z1 = 0;
        Vector3 point1 = new Vector3(x1, y1, z1);
        circleVerteices.Add(point1);
        uvs.Add(new Vector2((x1 + radius) / 2 * radius, (y1 + radius) / 2 * radius));

        for (int i = 0; i < 359; i = i + deltaAngle)
        {
            float x2 = radius * Mathf.Cos((i + deltaAngle) * val);
            float y2 = radius * Mathf.Sin((i + deltaAngle) * val);
            float z2 = 0;
            Vector3 point2 = new Vector3(x2, y2, z2);

            circleVerteices.Add(point2);

            uvs.Add(new Vector2((x2 + radius) / 2 * radius, (y2 + radius) / 2 * radius));

            triangles.Add(0);
            triangles.Add(triangleCount + 2);
            triangles.Add(triangleCount + 1);

            triangleCount++;
            point1 = point2;
        }




        m.vertices = circleVerteices.ToArray();
        m.triangles = triangles.ToArray();
        m.uv = uvs.ToArray();
        this.transform.position = circlePos;

    }
}
