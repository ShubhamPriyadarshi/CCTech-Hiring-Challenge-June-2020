using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SubmitButton : MonoBehaviour
{ 

    public InputField[] input;
    string circlePosRaw;
    Vector2 circlePos;
    string radius;
    string angle;
    string clearance;
    string lineLength;
    public GameObject circleGenerator;



    public void Submit() 
    {

        string[] coord;
        float coordX, coordY;
        circlePosRaw =  input[0].text.ToString();
        coord = circlePosRaw.Split(',');
        coordX = Convert.ToSingle(coord[0]);
        coordY = Convert.ToSingle(coord[1]);
        circlePos = new Vector2(coordX, coordY);

        radius = input[1].text.ToString();
        angle = input[2].text.ToString();
        clearance = input[3].text.ToString();
        lineLength = input[4].text.ToString();

        InstantiateCircle IC = circleGenerator.GetComponent<InstantiateCircle>();
        IC.OnSubmit(circlePos, Convert.ToSingle(radius), Convert.ToSingle(angle), Convert.ToSingle(clearance), Convert.ToDouble(lineLength));




    }
}
