using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;


public class SendButton : MonoBehaviour
{
    int it;
    public InputField inputField;
    public Text currText;
    GameObject IO;
    public GameObject Buildings;
    public GameObject defaultOptions1;
    public GameObject defaultOptions2;
    public GameObject defaultOptions3;
    public GameObject circleChords;

    public GameObject resetButton;
    public GameObject OutputLeft;
    public GameObject SimulationMode;
    public GameObject circleChordsCanvas;
    public InputField[] input;


    void Start()
    {
        it = 0;
        IO = GameObject.Find("IO");

    }

    public void RetrieveCmd()
    {
        List<string> cmd = new List<string>();
        string temp = inputField.text.ToString();
        inputField.text = "";
        cmd.Add(temp);
        
        //Debug.Log(cmd[0]);
        switch (ConsoleInputs.option)
        {
            case (1):
                
                int numOfVertices;
                switch (it)
                {

                    case (0):
                        

                        temp = temp.Replace("], [", " ").Replace("],", " ").Replace("[", "").Replace("]", "").Trim();
                        temp = Regex.Replace(temp, @"\s+", " ");
                        string[] coordinates = temp.Split(' ');
                        numOfVertices = coordinates.Length;
                        ConsoleInputs.PolygonData.numOfVertices = numOfVertices;
                      
                        if (numOfVertices>=3)
                        {
                            currText.text = "Enter the coordinates of the point ( X and Y separated by a comma)";
                            ConsoleInputs.PolygonData.polygonDataRaw = temp.Split(' ');
                            defaultOptions1.SetActive(false);
                        }
                        else
                        {
                            currText.text = "Error: Wrong number of coordinates ( enter atleast three coordinates ) or wrong format, try again ( Tip: Use the format given in default options )";
                            it--;
                        }
                      

                        break;


                    case (1):
                        string[] coord;
                        float coordX, coordY;
                        if (temp != "")
                        {
                            temp = temp.Replace("[", "").Replace("]", "");
                            coord = temp.Split(',');
                            coordX = Convert.ToSingle(coord[0]);
                            coordY = Convert.ToSingle(coord[1]);
                            ConsoleInputs.PolygonData.pointCoordinates = new Vector2(coordX, coordY);
                            //Debug.Log(ConsoleInputs.PolygonData.pointCoordinates);
                        }
                        else if (temp == "")
                            ConsoleInputs.PolygonData.pointCoordinates = new Vector2(1, 1);
                        SimulationMode.SetActive(true);
                        IO.SetActive(false);
                        resetButton.SetActive(true);
                        ConsoleInputs.DrawPolygon();
                        break;
                }
                break;
            case (2):
                int numOfBuildings;
                switch (it) {

                    case (0):
                        if (temp != "")
                        {
                            temp = temp.Replace("]], [[", " ").Replace("]],", " ").Replace("],[", " ").Replace("[", "").Replace("]", "").Trim();
                            temp = Regex.Replace(temp, @"\s+", " ");
                            string[] coordinates = temp.Split(' ');
                            if (coordinates.Length % 4 == 0 && coordinates.Length >=4) 
                            {
                                defaultOptions2.SetActive(false);
                                numOfBuildings = coordinates.Length / 4;
                                ConsoleInputs.BuildingData.numOfBuildings = numOfBuildings;
                                ConsoleInputs.BuildingData.buildingDataRaw = coordinates;
                                currText.text = "Enter the coordinates of the light source ( X and Y separated by a comma) ( Default [1,1] )";
                            }
                            else
                            {
                                currText.text = "Wrong number of coordinates or wrong format, please try again. ( Tip: Use the format given in default options )";
                                it--;
                            }
                        }
                        else
                        {
                            currText.text = "Wrong number of coordinates or wrong format, please try again. ( Tip: Use the format given in default options )";
                            it--;
                        }
                        break;
                        
                        


                    case (1):
                        string[] coord;
                        float coordX, coordY;
                        if (temp != "")
                        {
                            temp = temp.Replace("[", "").Replace("]", "");
                            coord = temp.Split(',');
                            coordX = Convert.ToSingle(coord[0]);
                            coordY = Convert.ToSingle(coord[1]);
                            ConsoleInputs.BuildingData.sunCoordinates = new Vector2(coordX, coordY);
                        }
                        else
                        {
                            ConsoleInputs.BuildingData.sunCoordinates = new Vector2(1, 1);
                        }
                        SimulationMode.SetActive(true);
                       // Debug.Log(ConsoleInputs.BuildingData.sunCoordinates);
                        IO.SetActive(false);
                        resetButton.SetActive(true);
                        ConsoleInputs.DrawBuilding();
                        break;



                        //case(3):


                } break;
            case (3):
                temp = temp.Replace("], [", " ").Replace("],", " ").Replace("[", "").Replace("]", "").Trim();
                temp = Regex.Replace(temp, @"\s+", " ");
                string[] CircleDataRaw = temp.Split(',');
                float X, Y;
                X = Convert.ToSingle(CircleDataRaw[0]);
                Y = Convert.ToSingle(CircleDataRaw[1]);
                InstantiateCircle.circlePos = new Vector2(X, Y);
                InstantiateCircle.radius = Convert.ToSingle(CircleDataRaw[2]);
                InstantiateCircle.angle = Convert.ToSingle(CircleDataRaw[3]);
                InstantiateCircle.clearance = Convert.ToSingle(CircleDataRaw[4]);
                InstantiateCircle.lineLength = Convert.ToDouble(CircleDataRaw[5]);


                if (CircleDataRaw.Length == 6)
                {
                    it++;
                    input[0].text = X + ", " + Y;
                    input[1].text = CircleDataRaw[2];
                    input[2].text = CircleDataRaw[3];
                    input[3].text = CircleDataRaw[4];
                    input[4].text = CircleDataRaw[5];
                    SimulationMode.SetActive(true);
                    IO.SetActive(false);
                    OutputLeft.SetActive(false);
                    resetButton.SetActive(true);
                    circleChords.SetActive(true);
                    circleChordsCanvas.SetActive(true);

                }
                else
                {
                    currText.text = "Error: Wrong format, use the format from the given in the default option";
                    it--;
                }
                break;


        } it++;
    }
}

