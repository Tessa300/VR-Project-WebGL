using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Net;

public class TestSQL : MonoBehaviour
{
    const String url = "http://www.beultemo.de/00_SubProjects/vr/API.php?key=VR_uniTrier";
    public string sessionID = "";

    private bool updated = false;
    private int counter = 0;

    // Start is called before the first frame update
    void Start()
    {
        WebRequest request = WebRequest.Create(url + "&get=NewSession");
        request.Method = "GET";
        WebResponse webResponse = request.GetResponse();
        sessionID = new StreamReader(webResponse.GetResponseStream()).ReadToEnd();
        Debug.Log("SessionID: " + sessionID);
    }

    // Update is called once per frame
    void Update()
    {
        //counter++;
        if (updated ) //|| counter % 1 != 0)
            return;
        //updated = true;
        Debug.Log("Enter Update");
        String reqUrl = "";
        try
        {
            sessionID = "290845";
            WebRequest request = WebRequest.Create(url + "&get=ControllerData&SessionID=" + sessionID);
            request.Method = "GET";
            request.Timeout = 1000; 
            reqUrl = request.RequestUri.AbsolutePath;

            WebResponse webResponse = request.GetResponse();
            String json = new StreamReader(webResponse.GetResponseStream()).ReadToEnd();

            if (!json.StartsWith("{"))
                return;

            double orientation_x = 0;
            double orientation_y = 0;
            double orientation_z = 0;
            foreach (string row in json.Substring(1, json.Length - 2).Split(','))
            {
                if (row.Contains("_X"))
                    orientation_x = Double.Parse(row.Substring(17, row.Length - 18).Replace('.', ','));
                if (row.Contains("_Y"))
                    orientation_y = Double.Parse(row.Substring(17, row.Length - 18).Replace('.', ','));
                if (row.Contains("_Z"))
                    orientation_z = Double.Parse(row.Substring(17, row.Length - 18).Replace('.', ','));
            }
            Debug.Log("x: " + orientation_x + " | y: " + orientation_y + " | z: " + orientation_z);
            transform.Translate(1,0,0);
            updated = false;
        }
        catch (Exception e)
        {
            Debug.Log("Fehler bei URL: " + reqUrl + Environment.NewLine + e.Message);
        }
        Debug.Log("Exit Update");
    }
}
