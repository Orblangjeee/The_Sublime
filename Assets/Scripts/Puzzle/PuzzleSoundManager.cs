using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleSoundManager : MonoBehaviour
{

    public static PuzzleSoundManager instance;

    public AudioClip[] SoundList;
    public AudioSource puzzleSoundManager;
    public AudioSource roomSoundManager;

    [Header("Ring")]
    public AudioClip rollingAudio;
    public AudioSource bottomRingRollingAudio;
    public AudioSource upRingRollingAudio;
    public bool B_stopAudio = false;
    public bool U_stopAudio = false;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }


    }

    private void Start()
    {
        upRingRollingAudio.clip = rollingAudio;
        bottomRingRollingAudio.clip = rollingAudio;
    }

    public void PlayCorrectSound()
    {
        if (SoundList != null && SoundList.Length > 0) //배열이 비어있지 않을 경우
        {
            puzzleSoundManager.clip = SoundList[0];
            puzzleSoundManager.Stop();
            puzzleSoundManager.Play();

        }
    }
    public void PlayMatchSound()
    {
        if (SoundList != null && SoundList.Length > 0) //배열이 비어있지 않을 경우
        {
            puzzleSoundManager.clip = SoundList[1];
            puzzleSoundManager.Stop();
            puzzleSoundManager.Play();

        }
    }

    public void PlayGoalMatchSound()
    {
        if (SoundList != null && SoundList.Length > 0) //배열이 비어있지 않을 경우
        {
            puzzleSoundManager.clip = SoundList[0];
            puzzleSoundManager.Stop();
            puzzleSoundManager.Play();

        }
    }

    public void PlaySwitchingRoomSound()
    {
        if (SoundList != null && SoundList.Length > 0) //배열이 비어있지 않을 경우
        {
            roomSoundManager.clip = SoundList[2];
            roomSoundManager.Stop();
            roomSoundManager.Play();

        }
    }

    public void PlayPrismArrivedSound()
    {
        if (SoundList != null && SoundList.Length > 0) //배열이 비어있지 않을 경우
        {
            roomSoundManager.clip = SoundList[4];
            roomSoundManager.Stop();
            roomSoundManager.Play();

        }
    }

    public void PlayPrismCorrectSound()
    {
        if (SoundList != null && SoundList.Length > 0) //배열이 비어있지 않을 경우
        {
            roomSoundManager.clip = SoundList[5];
            roomSoundManager.Stop();
            roomSoundManager.Play();

        }
    }

    public void PlayEndSceneSound()
    {
        if (SoundList != null && SoundList.Length > 0) //배열이 비어있지 않을 경우
        {
            puzzleSoundManager.clip = SoundList[3];
            puzzleSoundManager.Stop();
            puzzleSoundManager.Play();

        }
    }


    //--------------------Ring------------------------------------
    public IEnumerator B_FadeOut(AudioSource audioSource, float FadeTime)
    {
        Debug.Log("Fade");
        float startVolume = 1f;

        while (audioSource.volume > 0)
        {
            audioSource.volume -= startVolume * Time.deltaTime / FadeTime;

            yield return B_stopAudio = false;

        }

        audioSource.Stop();
        audioSource.volume = 1f;
    }

    public IEnumerator U_FadeOut(AudioSource audioSource, float FadeTime)
    {
        Debug.Log("Fade");
        float startVolume = 1f;

        while (audioSource.volume > 0)
        {
            audioSource.volume -= startVolume * Time.deltaTime / FadeTime;

            yield return U_stopAudio = false;

        }

        audioSource.Stop();
        audioSource.volume = 1f;
    }

    public void B_AudioFadeOut()
    {
        if (bottomRingRollingAudio.isPlaying && B_stopAudio)
        {
            StartCoroutine(B_FadeOut(bottomRingRollingAudio, 1f));

        }

    }

    public void U_AudioFadeOut()
    {
        if (upRingRollingAudio.isPlaying && U_stopAudio)
        {
            StartCoroutine(U_FadeOut(upRingRollingAudio, 1f));
        }


    }

    public void B_AudioRingPlay()
    {
        if (!bottomRingRollingAudio.isPlaying && !B_stopAudio)
        {

            bottomRingRollingAudio.Play();
            B_stopAudio = true;
        }
    }

    public void U_AudioRingPlay()
    {
        if (!upRingRollingAudio.isPlaying && !U_stopAudio)
        {

            upRingRollingAudio.Play();
            U_stopAudio = true;
        }
    }
}
