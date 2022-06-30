using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class AudioController : MonoBehaviour
{
    public bool audioEnabled = true;
    public AudioClip[] audios;
    private bool[] audioPlayed = new[] {false, false, false, false, false, false, false, false, false};

    private AudioSource audioSource;

    private VideoPlayer videoPlayer;

    private VideoController videoController;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        videoController = GetComponent<VideoController>();
        videoPlayer = videoController.videoPlayer;
        
    }

    private void Update()
    {
        double time = videoPlayer.time;
        if (!audioPlayed[0] && time > 2)
        {
            playAudio(0);
            audioPlayed[0] = true;
        }
        if (!audioPlayed[1] && time > 45)
        {
            playAudio(1);
            audioPlayed[1] = true;
        }
        if (!audioPlayed[2] && time > 90)
        {
            playAudio(2);
            audioPlayed[2] = true;
        }
        if (!audioPlayed[3] && time > 128)
        {
            playAudio(3);
            audioPlayed[3] = true;
        }
        if (!audioPlayed[4] && time > 140)
        {
            playAudio(4);
            audioPlayed[4] = true;
        }
        if (!audioPlayed[5] && time > 165)
        {
            playAudio(5);
            audioPlayed[5] = true;
        }
        if (!audioPlayed[6] && time > 210)
        {
            playAudio(6);
            audioPlayed[6] = true;
        }
        if (!audioPlayed[7] && time > 238)
        {
            playAudio(7);
            audioPlayed[7] = true;
        }
        if (!audioPlayed[8] && time > 307)
        {
            playAudio(8);
            audioPlayed[8] = true;
        }
    }

    public void playAudio(int num)
    {
        if (audioEnabled)
        {
            AudioClip audio = audios[num];
            audioSource.clip = audio;
            audioSource.Play();
        }
    }
}
