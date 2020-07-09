using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InstantiateCircle : MonoBehaviour
{

    
    GameObject circle;
    public static Vector2 circlePos;
    public static float radius;
    public static float angle;
    public static float clearance;
    public static double lineLength;

    
    public GameObject circlePrefab;
    public GameObject linesPrefab;
    public GameObject P, Q;
    public Text outputText;
    public Material [] mat;

    public InputField[] input;

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
    void OnEnable()
    {
        if (lineLength == 2 * radius)
            lineLength -= 1E-14;
        circle = Instantiate(circlePrefab, this.transform, true);
        circle.GetComponent<MeshRenderer>().material = mat[0];
        Mesh m;
        MeshFilter mf;
        mf = circle.GetComponent<MeshFilter>();
        m = new Mesh();
        mf.mesh = m;
        drawCircle(m);
        circle.transform.position = circlePos;
        GetChordCoordinates();
    }

    public void OnSubmit(Vector2 c, float r, float a, float cl, double ll)
    {

        circlePos = c;
        radius = r;
        angle = a;
        clearance = cl;
        lineLength = ll;
        if (lineLength == 2 * radius)
            lineLength -= 1E-14;

        GameObject[] OldLines = GameObject.FindGameObjectsWithTag("Lines");
        for (int i = 0; i < OldLines.Length; i++)
            Destroy(OldLines[i]);
        Destroy(circle);
        circle = Instantiate(circlePrefab, this.transform, true);
        circle.GetComponent<MeshRenderer>().material = mat[0];
        Mesh m;
        MeshFilter mf;
        mf = circle.GetComponent<MeshFilter>();
        m = new Mesh();
        mf.mesh = m;
        drawCircle(m);
        circle.transform.position = circlePos;
        GetChordCoordinates();

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


    void GetChordCoordinates()
    {
        bool intersects = true;

        if (angle == 0)
            angle = 360;

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
        
        
        if ((angle < 90 && angle >0)||angle>=270)
        {
            tempLine = GetLineCoordinates(lineSegment[0].x, lineSegment[0].y, circlePosD.x, circlePos.y, slope, radius); // for lower coordinate
            chordCoordinates[0] = new Vector2D(tempLine[0].x, tempLine[0].y);
            tempLine = GetLineCoordinates(lineSegment[1].x, lineSegment[1].y, circlePosD.x, circlePos.y, slope, radius); // for upper coordinate
            chordCoordinates[1] = new Vector2D(tempLine[0].x, tempLine[0].y);
            if (chordCoordinates[0].x == 0 && chordCoordinates[0].y == 0 && chordCoordinates[1].x == 0 && chordCoordinates[1].y == 0)
            {
                intersects = false;
            }
        }
        else if (angle>=90 && angle<270)
        {
            tempLine = GetLineCoordinates(lineSegment[0].x, lineSegment[0].y, circlePosD.x, circlePos.y, slope, radius); // for lower coordinate
            chordCoordinates[0] = new Vector2D(tempLine[1].x, tempLine[1].y);
            tempLine = GetLineCoordinates(lineSegment[1].x, lineSegment[1].y, circlePosD.x, circlePos.y, slope, radius); // for upper coordinate
            chordCoordinates[1] = new Vector2D(tempLine[1].x, tempLine[1].y);
            Debug.Log("CHORDS DEBUG:            "+chordCoordinates[0].x +" "+ chordCoordinates[0].y + " " + chordCoordinates[1].x + " " + chordCoordinates[1].y);
            if (chordCoordinates[0].x == 0 && chordCoordinates[0].y == 0 && chordCoordinates[1].x ==0  && chordCoordinates[1].y == 0)
            {
                intersects = false;
            }

        }
        Debug.Log(chordCoordinates[0].x + " " + chordCoordinates[0].y);
        Debug.Log(chordCoordinates[1].x + " " + chordCoordinates[1].y);
        if (intersects)
        {
            P.SetActive(true);
            Q.SetActive(true);
            P.transform.position = new Vector3(Convert.ToSingle(chordCoordinates[0].x), Convert.ToSingle(chordCoordinates[0].y), 0);
            Q.transform.position = new Vector3(Convert.ToSingle(chordCoordinates[1].x), Convert.ToSingle(chordCoordinates[1].y), 0);
            DrawLine(lineSegment[0], chordCoordinates[0], mat[4], 0.05f);
            // DrawLine(chordCoordinates[0], circlePosD, mat[1], 0.07f);
            DrawLine(lineSegment[1], chordCoordinates[1], mat[4], 0.05f);
            //DrawLine(chordCoordinates[1], circlePosD, mat[1], 0.07f);

            DrawLine(chordCoordinates[0], chordCoordinates[1], mat[5], 0.12f, 3);//chord
            outputText.text = "Output: Values of P:"+ "( "+Math.Round(P.transform.position.x,3)+", "+ Math.Round(P.transform.position.y,3)+" )" + "& Q:"+ "("+ Math.Round(Q.transform.position.x,3)+", "+ Math.Round(Q.transform.position.y,3)+")" ;
        }
        else
        {
            P.SetActive(false);
            Q.SetActive(false);
            outputText.text=  "Output: Lines do not interesect the circle";
        
        }


    }

    Vector2D[] GetLineCoordinates(double x, double y, double a, double b, double m, double r)
    {
        Vector2D[] lineSegment = new Vector2D[2] {new Vector2D(0,0), new Vector2D(0, 0)};

        double c = y - m * x;
        Debug.Log(c);
        double d = (Math.Pow(r, 2) * (1 + Math.Pow(m, 2)) - Math.Pow((b - m * a - c), 2));
        Debug.Log("VALUE OF D: "+d);
        if (d < 0)
            return lineSegment;
        lineSegment[0].x = (a + b * m - c * m + Math.Sqrt(d)) / (1 + Math.Pow(m, 2));//lower x
        lineSegment[1].x = (a + b * m - c * m - Math.Sqrt(d)) / (1 + Math.Pow(m, 2));
        lineSegment[0].y = (c + a * m + b * Math.Pow(m, 2) + m * Math.Sqrt(d)) / (1 + Math.Pow(m, 2));// lower y
        lineSegment[1].y = (c + a * m + b * Math.Pow(m, 2) - m * Math.Sqrt(d)) / (1 + Math.Pow(m, 2));



        return lineSegment;
    }

    void drawCircle(Mesh m)
    {
        List<Vector3> circleVertices = new List<Vector3>();
        List<Vector2> uvs = new List<Vector2>();
        List<int> triangles = new List<int>();
        float val = Mathf.PI / 180f;//one degree = val radians
        //float radius = 1f;
        int deltaAngle = 2;

        Vector3 center = Vector3.zero;
        circleVertices.Add(center);
        uvs.Add(new Vector2(0.5f, 0.5f));
        int triangleCount = 0;

        float x1 = radius * Mathf.Cos(0);
        float y1 = radius * Mathf.Sin(0);
        float z1 = 0;
        Vector3 point1 = new Vector3(x1, y1, z1);
        circleVertices.Add(point1);
        uvs.Add(new Vector2((x1 + radius) / 2 * radius, (y1 + radius) / 2 * radius));

        for (int i = 0; i < 359; i = i + deltaAngle)
        {
            float x2 = radius * Mathf.Cos((i + deltaAngle) * val);
            float y2 = radius * Mathf.Sin((i + deltaAngle) * val);
            float z2 = 0;
            Vector3 point2 = new Vector3(x2, y2, z2);

            circleVertices.Add(point2);

            uvs.Add(new Vector2((x2 + radius) / 2 * radius, (y2 + radius) / 2 * radius));

            triangles.Add(0);
            triangles.Add(triangleCount + 2);
            triangles.Add(triangleCount + 1);

            triangleCount++;
            point1 = point2;
        }




        m.vertices = circleVertices.ToArray();
        m.triangles = triangles.ToArray();
        m.uv = uvs.ToArray();
        

    }
}
