using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text.RegularExpressions;

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
    public Text OutputRight;



    public void Submit() 
    {
        if (input[0].text != "" && input[1].text != "" && input[2].text != "" && input[3].text != "" && input[4].text != "")
        {
            string[] coord;
            float coordX, coordY;
            circlePosRaw = input[0].text.ToString();
            circlePosRaw = circlePosRaw.Replace(",", " ").Trim();
            circlePosRaw = Regex.Replace(circlePosRaw, @"\s+", " ");
            coord = circlePosRaw.Split(' ');

            coordX = Convert.ToSingle(coord[0]);
            coordY = (coord.Length == 2) ? (Convert.ToSingle(coord[1])) : 0;

            circlePos = new Vector2(coordX, coordY);

            radius = input[1].text.ToString();
            angle = input[2].text.ToString();
            clearance = input[3].text.ToString();
            lineLength = input[4].text.ToString();

            InstantiateCircle IC = circleGenerator.GetComponent<InstantiateCircle>();
            IC.OnSubmit(circlePos, Convert.ToSingle(radius), Convert.ToSingle(angle), Convert.ToSingle(clearance), Convert.ToDouble(lineLength));
        }
        else
        {

            OutputRight.text = "Error: Please check and correct the inputs and submit again.";
        
        }




    }
}
