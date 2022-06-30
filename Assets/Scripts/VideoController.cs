using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;

public class VideoController : MonoBehaviour
{
    public MQTTReceiver _eventSender;
    public string tagOfTheMQTTReceiver = "GameController";
    public VideoPlayer videoPlayer;
    public GameObject playerCamera;

    private int[] starts = new int[8]{0, 49, 64, 135, 153, 162, 210, 330};
    private int[] ends = new int[8]{47, 62, 132, 144, 160, 210, 330, 356};
    private int[] durations = new int[8];
    
    //private int[] starts = new int[13]{0, 49, 64, 135, 153, 162, 210, 330, 368, 400, 510, 562, 572};
    //private int[] ends = new int[13]{47, 62, 132, 144, 160, 210, 330, 356, 400, 510, 558, 569, 593};
    //private int[] durations = new int[13];

    private int currentVideoPt = 0;
    
    private int[] rotations = new int[8]{130, 220, -50, 40, 220, -50, -50, -50};
    //private int[] rotations = new int[13]{130, 220, -50, 40, 220, -50, -50, -50, 130, 130, 130, 40, 130};

    private float targetRatio;

    public Text text_message;
    public Text text_speed;
    public Text text_ratio;

    public ScreenBlocker screenBlocker;
    
    // Start is called before the first frame update
    void Start()
    {
        _eventSender=this.gameObject.GetComponent<MQTTReceiver>();
        _eventSender.OnMessageArrived += OnMessageArrivedHandler;

        for (int i = 0; i < durations.Length; i++)
        {
            durations[i] = ends[i] - starts[i];
        }

        videoPlayer.playbackSpeed = 0.1f;
    }

    private void Update()
    {
        // stop when needed
        if (targetRatio <= currentRatio(currentVideoPt))
        {
            //videoPlayer.playbackSpeed = 0;
        }
        
        //Debug.Log(currentRatio(currentVideoPt));
    }
    
    
    

    private void OnMessageArrivedHandler(string newMsg)
    {
        Debug.Log("Message Arrived");
        string[] messages = newMsg.Split();
        Debug.Log(newMsg);
        text_message.text = newMsg;
        ParseAndPlay(messages);
    }

    private void ParseAndPlay(string[] messages)
    {
        int currentPt = int.Parse(messages[0]);
        // update pt if necessary
        //if (currentPt != currentVideoPt)
        //{
        //    goToPoint(currentPt);
        //}

        float distanceRatio = float.Parse(messages[1]);
        if (distanceRatio > 0.95)
        {
            screenBlocker.BlockTheView();
        }
        if (currentPt != currentVideoPt)
        {
            goToPoint(currentVideoPt);
        }
        targetRatio = distanceRatio;
        float deltaRatio = float.Parse(messages[2]);
        float deltaTime = float.Parse(messages[3]);

        float currentVideoRatio = currentRatio(currentPt);
        // set speed
        videoPlayer.playbackSpeed = targetSpeed(currentPt, distanceRatio, currentVideoRatio);
        text_speed.text = videoPlayer.playbackSpeed.ToString();
        text_ratio.text = currentVideoRatio.ToString();

    }

    private void goToPoint(int targetPt)
    {
        // change video section
        videoPlayer.time = starts[targetPt+1];
        currentVideoPt = targetPt+1;
        // change orientation
        playerCamera.transform.rotation = Quaternion.Euler(0, rotations[targetPt+1], 0);
    }

    private float targetSpeed(int currentPt, float distanceRatio, float videoRatio)
    {
        float diff = distanceRatio - videoRatio;
        diff = Mathf.Clamp(diff, 0, 1);
        return Mathf.Clamp(durations[currentPt] * diff, 0f, 2f);
    }

    private float currentRatio(int currentPt)
    {
        float currentVideoTime = (float)videoPlayer.time;
        float ratio = (currentVideoTime - starts[currentPt]) / durations[currentPt];
        return Mathf.Clamp(ratio, 0, 1);
    }
}
