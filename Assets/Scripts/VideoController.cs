using System.Collections;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class VideoController : MonoBehaviour
{
    public MQTTReceiver _eventSender;
    //public string tagOfTheMQTTReceiver = "GameController";
    public VideoPlayer videoPlayer;
    public VideoPlayer fireFighterPlayer;
    public GameObject fireFigher;
    public GameObject playerCamera;

    public int[] starts = new int[]{0, 49, 64, 135, 153, 162};
    public int[] ends = new int[]{47, 62, 132, 144, 160, 356};
    private int[] durations;
    public float[] endingPortion = new float[]{0.96f, 0.95f, 0.98f, 0.92f, 0.92f, 0.995f};
    
    //public int[] starts = new int[]{0, 49, 64, 135, 153, 162, 210, 330};
    //public int[] ends = new int[]{47, 62, 132, 144, 160, 210, 330, 356};
    //private int[] starts = new int[13]{0, 49, 64, 135, 153, 162, 210, 330, 368, 400, 510, 562, 572};
    //private int[] ends = new int[13]{47, 62, 132, 144, 160, 210, 330, 356, 400, 510, 558, 569, 593};
    //private int[] durations = new int[13];

    private int currentVideoPt = 0;
    
    private int[] rotations = new int[]{130, 220, -50, 40, 220, -50};
    //private int[] rotations = new int[13]{130, 220, -50, 40, 220, -50, -50, -50, 130, 130, 130, 40, 130};

    private float targetRatio;

    public Text text_message;
    public Text text_speed;
    public Text text_ratio;

    public HeadBlocker headBlocker;
    public GameObject startScene;
    public GameObject VideoPlayerObject;
    private long currentFrame;
    
    // Start is called before the first frame update
    void Start()
    {
        endingPortion = new float[]{0.95f, 0.93f, 0.97f, 0.9f, 0.9f, 0.995f};
        _eventSender=this.gameObject.GetComponent<MQTTReceiver>();
        _eventSender.OnMessageArrived += OnMessageArrivedHandler;
        durations = new int[starts.Length];
        for (int i = 0; i < durations.Length; i++)
        {
            durations[i] = ends[i] - starts[i];
        }
        
        videoPlayer.time = starts[0];
        videoPlayer.playbackSpeed = 0f;
    }

    private void FixedUpdate()
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

        Debug.Log(currentPt);
        Debug.Log(endingPortion);
        Debug.Log(endingPortion[currentPt]);

        float distanceRatio = float.Parse(messages[1]);
        if (distanceRatio > endingPortion[currentPt])
        {
            headBlocker.BlockTheView();
        }
        if (currentPt != currentVideoPt && currentVideoPt <= 5)
        {
            goToPoint(currentVideoPt);
        }
        targetRatio = distanceRatio;
        float num3 = float.Parse(messages[2]);
        float num4 = float.Parse(messages[3]);

        float currentVideoRatio = currentRatio(currentPt);

        if (num3 == -10 && num4 == -10)
        {
            // restart app
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            
        }
        if (num3 == -50 && num4 == -50)
        {
            // Start video
            startScene.SetActive(false);
            videoPlayer.Play();
            headBlocker.BlockTheView();
        }

        if (num3 == -5 && num4 == -5)
        {
            // Firefighter start
            // headBlocker.BlockTheView();
            // should block the view first then invoke the video scene
            headBlocker.StopBlocking();
            currentFrame = videoPlayer.frame;
            VideoPlayerObject.SetActive(false);
            fireFigher.SetActive(true);
            StartCoroutine(PlayFireFighter(fireFighterPlayer));
        }
        if (num3 == -6 && num4 == -6)
        {
            // Firefighter end
            headBlocker.StopBlocking();
            fireFigher.SetActive(false);
            fireFighterPlayer.frame = 0;
            VideoPlayerObject.SetActive(true);
            videoPlayer.frame = currentFrame + 1;
            videoPlayer.Play();
        }
        if (num3 == -7 && num4 == -7)
        {
            // block the view 
            headBlocker.ManuallyBlocked();
        }
        if (num3 == -8 && num4 == -8)
        {
            // unblock the view
            headBlocker.StopBlocking();
        }

        // set speed
        videoPlayer.playbackSpeed = targetSpeed(currentPt, distanceRatio, currentVideoRatio);
        text_speed.text = videoPlayer.playbackSpeed.ToString();
        text_ratio.text = currentVideoRatio.ToString();

    }

    private void goToPoint(int targetPt)
    {
        headBlocker.BlockTheView();
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

    IEnumerator PlayFireFighter(VideoPlayer videoPlayer)
    {
        yield return new WaitForSeconds(0.5f);
        videoPlayer.Play();
    }
}
