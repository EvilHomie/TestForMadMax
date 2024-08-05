using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TESTSOUNDENGINE : MonoBehaviour
{

    [SerializeField] double soundDuration;
    [SerializeField] double goalTime;
    [SerializeField] AudioSource[] audioSources;
    [SerializeField] AudioSource SpeedUpAS;
    [SerializeField] int audioToggle;
    [SerializeField] AudioClip currentClip;

    [SerializeField] AudioClip StartClip;
    [SerializeField] AudioClip ContinueClip;
    [SerializeField] AudioClip SpeedUpClip;


    private void Start()
    {
        //currentClip = StartClip;
        //SetCurrentClip(StartClip);
        goalTime = AudioSettings.dspTime + 0.5f;

        //PlaySound();
    }

    private void PlaySound()
    {
        throw new NotImplementedException();
    }

    private void Update()
    {
        if (AudioSettings.dspTime > goalTime - 1f)
        {
            PlayScheduledClip();
            SetCurrentClip(ContinueClip);
        }
        //if (Input.GetKeyDown(KeyCode.W))
        //{
        //    SpeedUpAS.Play();
        //}
        //if (Input.GetKeyUp(KeyCode.W))
        //{
        //    SpeedUpAS.Stop();
        //}

    }

    private void PlayScheduledClip()
    {
        audioSources[audioToggle].clip = currentClip;
        audioSources[audioToggle].PlayScheduled(goalTime);

        soundDuration = (double)currentClip.samples/currentClip.frequency;
        goalTime += soundDuration;
        audioToggle = 1 - audioToggle;
    }

    public void SetCurrentClip(AudioClip audioClip)
    {
        currentClip = audioClip;   
    }




    //[SerializeField] AudioSource AudioSource1;
    //[SerializeField] AudioSource AudioSource2;
    //[SerializeField] AudioClip StartClip;
    //[SerializeField] AudioClip ContinueClip;

    //double initLatency = .1d;


    //private void Start()
    //{
    //    double playTime = initLatency;
    //    AudioSource1.PlayDelayed((float)playTime);
    //    playTime += (double)AudioSource1.clip.length;
    //    AudioSource2.PlayDelayed((float)playTime);

    //}
    ////    IEnumerator EngineSound()
    ////{
    ////    AudioSource.PlayOneShot(StartClip);
    ////    yield return new WaitForSeconds(StartClip.length);
    ////    AudioSource.PlayOneShot(ContinueClip);
    ////}
}
