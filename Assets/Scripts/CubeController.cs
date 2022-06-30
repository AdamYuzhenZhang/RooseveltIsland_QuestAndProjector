using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeController : MonoBehaviour
{
    public string nameController = "Cube Controller";
    public string tagOfTheMQTTReceiver="";
    public MQTTReceiver _eventSender;

    public GameObject cube;
    public GameObject face1;
    public GameObject face2;
    public GameObject face3;
    public GameObject face4;
    public GameObject face5;
    public GameObject face6;

    public Material defaultMat;
    public Material touchMat;
    public Material forceMat;

    void Start()
    {
        _eventSender=GameObject.FindGameObjectsWithTag(tagOfTheMQTTReceiver)[0].gameObject.GetComponent<MQTTReceiver>();
        _eventSender.OnMessageArrived += OnMessageArrivedHandler;
    }

    private void OnMessageArrivedHandler(string newMsg)
    {
        string[] messages = newMsg.Split();
        
        ParseAndTouch(messages);
        ParseAndRotate(messages);

        //Debug.Log(face1_touch);
        //Debug.Log(x);
    }

    private void ParseAndTouch(string[] messages)
    {
        bool face1_touch = (messages[0] == "1");
        bool face1_force = (messages[1] == "1");
        bool face2_touch = (messages[3] == "1");
        bool face2_force = (messages[2] == "1");
        bool face3_touch = (messages[5] == "1");
        bool face3_force = (messages[6] == "1");
        bool face4_touch = (messages[7] == "1");
        bool face4_force = (messages[8] == "1");
        bool face5_touch = (messages[9] == "1");
        bool face5_force = (messages[10] == "1");
        bool face6_touch = (messages[11] == "1");
        bool face6_force = (messages[10] == "1");
        
        //Touch(face1, face1_touch, face1_force);
        Touch(face2, face2_touch, face2_force);
        //Touch(face3, face3_touch, face3_force);
        //Touch(face4, face4_touch, face4_force);
        //Touch(face5, face5_touch, face5_force);
        Touch(face6, face6_touch, face6_force);
    }
    private void Touch(GameObject face, bool touch, bool force)
    {
        if (force)
        {
            face.GetComponent<MeshRenderer>().material = forceMat;
            Debug.Log("force touch");
        }
        else
        {
            if (touch)
            {
                face.GetComponent<MeshRenderer>().material = touchMat;
                Debug.Log("normal touch");
            }
            else
            {
                face.GetComponent<MeshRenderer>().material = defaultMat;
            }
        }
    }

    private void ParseAndRotate(string[] messages)
    {
        float x = float.Parse(messages[12]);
        float y = float.Parse(messages[13]);
        float z = float.Parse(messages[14]);

        cube.transform.localRotation = Quaternion.Euler(x + 90f, 0.0f, -y-90f);
    }
}
