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
        request.Timeout = 10000;
        WebResponse webResponse = request.GetResponse();
        sessionID = new StreamReader(webResponse.GetResponseStream()).ReadToEnd();
        Debug.Log("SessionID: " + sessionID);
    }

    // Update is called once per frame
    void Update()
    {
        //counter++;
        if (updated) //|| counter % 1 != 0)
            return;
        //updated = true;
        String reqUrl = "";
        try
        {
            sessionID = "222222";
            WebRequest request = WebRequest.Create(url + "&get=ControllerData&SessionID=" + sessionID);
            request.Method = "GET";
            request.Timeout = 800;
            reqUrl = request.RequestUri.AbsolutePath;

            WebResponse webResponse = request.GetResponse();
            String json = new StreamReader(webResponse.GetResponseStream()).ReadToEnd();

            Debug.Log("JSON: " + json);

            if (!json.StartsWith("{"))
                return;

            float x = 0;
            float y = 0;
            float z = 0;
            foreach (string row in json.Substring(1, json.Length - 2).Split(','))
            {
                if (row.Contains("X"))
                    x = float.Parse(row.Substring(5, row.Length - 6).Replace('.', ','));
                if (row.Contains("Y"))
                    y = float.Parse(row.Substring(5, row.Length - 6).Replace('.', ','));
                if (row.Contains("Z"))
                    z = float.Parse(row.Substring(5, row.Length - 6).Replace('.', ','));
            }
            Debug.Log("x: " + x + " | y: " + y + " | z: " + z);
            // x: rechts/links

            updated = false;
            move(x, y, z);
        }
        catch (Exception e)
        {
            Debug.Log("Fehler bei URL: " + reqUrl + Environment.NewLine + e.Message + Environment.NewLine + e.StackTrace);
        }
    }

    private void move(float x, float y, float z)
    {
        float interval = 0.3f;
        x = x - (x % interval);

        Vector3 dir = (new Vector3(x, 0, 0)).normalized;
        transform.Translate(dir);
    }
}
