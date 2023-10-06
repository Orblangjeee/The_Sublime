using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class WhatController : MonoBehaviour
{
    public static WhatController instance;
    public XRBaseController[] controller;
    public XRBaseController lastController;

    [Header("Ring Haptic")]
  
    public float ringhapticIntencity = 0.2f;
    public float ringhapticTime = 0.1f; //진동 시간
    public float compareValue = 0.5f; //회전 비교 최솟값

    [Header("Anim Haptic")]
    
    public float animHapticIntencity = 0.8f;
    public float animHapticTime = 0.045f;
    public float animWaitTime = 0.06f;
    public bool isAnim = false;

    [Header("Clear Haptic")]

    public float clearHapticIntencity = 1f;
    public float clearHapticTime = 0.03f;
    public float clearWaitTime = 0.24f;
    public ClearMode clearMode;

    public enum ClearMode { Both, One }

    [Header("Radius")]
    public float grabRadius = 0.2f; //그랩 시 Collider Radius
    public float dropRadius = 0.2f; //놓을 시 Collider Radius

    public bool isClear = true;
    private void Awake()
    {
        if (instance == null) { instance = this; }
    }

    private void Start()
    {
        lastController = null;
        isAnim = false;
    }


    public IEnumerator ClearModeChange()
    {
        
        if (clearMode == ClearMode.One)
        {
            
            while (isClear)
            {
                
                yield return new WaitForSeconds(clearWaitTime);
                lastController.SendHapticImpulse(clearHapticIntencity, clearHapticTime);
                yield return new WaitForSeconds(clearWaitTime);
                lastController.SendHapticImpulse(clearHapticIntencity, clearHapticTime);
                yield return new WaitForSeconds(clearWaitTime);
                isClear = false;
            }
            
        }

        if (clearMode == ClearMode.Both)
        {
            for (int i = 0; i < 2; i++)
            {
                controller[i].SendHapticImpulse(clearHapticIntencity, clearHapticTime);
                yield return new WaitForSeconds(clearWaitTime);
            }
        }
    }

    public void ClearVibration()
    {
        isClear = true;
        StartCoroutine(ClearModeChange());
    }


    public IEnumerator AnimVibCoroutine()
    {
        while (isAnim)
        {
            for (int i = 0; i < controller.Length; i++)
            {
                controller[i].SendHapticImpulse(animHapticIntencity, animHapticTime);

            }
            yield return new WaitForSeconds(animWaitTime);
            Debug.Log("Vib");
        }

    }


    public void AnimVibration()
    {
        StartCoroutine(nameof(AnimVibCoroutine));
    }

    public void StopCoroutine()
    {
        StopAllCoroutines();
        Debug.Log("Stop");
    }
}
