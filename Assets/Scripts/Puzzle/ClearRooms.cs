using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection.Emit;
using Unity.VisualScripting;
using UnityEngine;

public class ClearRooms : MonoBehaviour
{
    public static ClearRooms Instance;

    public int puzzleNumber = 0;
    //몇 번째 퍼즐인지 받아오기
    public int PuzzleNumber
    {
        get { return puzzleNumber; }
        set { puzzleNumber = value; }
    }

    [Header("All")]
    public bool rightRing = false; //퍼즐이 정답 자리에 있는지
    public bool leftRing = false; //퍼즐이 정답 자리에 있는지
    public bool upClear = false; //Clear 체크해서 Rot 0으로 맞추기 upRing
    public bool bottomClear = false; //Clear 체크해서 Rot 0으로 맞추기 bottomRing
    public bool stopRing = false; // Ring On/Off
    public bool isEnd = false; //마지막 퍼즐
    
    //순차적인 결과 체크를 위한 확인 변수
    public bool ACheck = false;
    public bool BCheck = false;
    public bool AllCheck = false;

    [Header("Mode")]
    public UpMode upMode; //upRing 회전 모드 변경
    public BottomMode bottomMode; //bottomRing 회전 모드 변경
    public enum UpMode { X,Y,Z } 
    public enum BottomMode { X,Y,Z }


    [Header("Animation")]
    //애니메이션
    public Animator p1Anim;
    public Animator p2Anim;
    public GameObject p3;
    public GameObject p4;
    public Animator p4Anim;
    public GameObject imitationWall, imitationCelling;
    public GameObject p5;
    public Animator p5Anim;
    public GameEnded gameEnded;

    


    private void Awake()
    {
        if (Instance == null) { Instance = this; }
    }

    private void Start()
    {
        p1Anim = p1Anim.GetComponent<Animator>();
        p2Anim = p2Anim.GetComponent<Animator>();
        p4Anim = p4Anim.GetComponent<Animator>();
        p5Anim = p5Anim.GetComponent<Animator>();

        p3.SetActive(false);
        p4.SetActive(false);
        p5.SetActive(false);

        upMode = UpMode.Y;
        bottomMode = BottomMode.Y;
    }

   //퍼즐 BottomRing 완료 시
    public void leftCheck()
    {
        Debug.Log("LeftClear" + puzzleNumber);
        if (puzzleNumber == 0)
        {
            rightRing = true;
            leftRing = true;
            PuzzleSoundManager.instance.PlayMatchSound();
            ClearCheck();
        }
        else
        {
            leftRing = true;
            ClearCheck();
        }
    }
    //퍼즐 BottomRing 범위가 아닐 시, 결과
    public void leftNot()
    {
      bottomClear = false;
      leftRing = false;
    }
    //퍼즐 UpRing 완료 시
    public void RightCheck()
    {
        Debug.Log("RightClear" + puzzleNumber);
        if (puzzleNumber == 0)
        {
            rightRing = true;
            leftRing = true;
            PuzzleSoundManager.instance.PlayMatchSound();

            ClearCheck();
        } else
        {
            rightRing = true;
            ClearCheck();
        }
       
    }
    public void RightNot()
    {
        upClear = false;
        rightRing = false;
    }
    //All 클리어 체크
    public void ClearCheck()
    {
        if (rightRing && leftRing)
        {
            leftRing = false;
            rightRing = false;
            //퍼즐 회전값 0으로 맞추기
            upClear = true;
            bottomClear = true;
            //클리어한 후 이어지는 내용
            switch (puzzleNumber)
            {
                
                case 1:
                    PuzzleSoundManager.instance.PlayMatchSound();
                    PuzzleSoundManager.instance.PlaySwitchingRoomSound();
                    p1Anim.SetTrigger("isStart");
                    p2Anim.SetTrigger("isStart");
                    break;
                case 2:
                    PuzzleSoundManager.instance.PlayCorrectSound();
                    //세번째 퍼즐 오픈
                    p3.SetActive(true);
                    break;
                case 3:
                    PuzzleSoundManager.instance.PlayCorrectSound();
                    p4.SetActive(true);
                    p2Anim.SetTrigger("Correct");
                    p4Anim.SetTrigger("Correct");
                    imitationWall.SetActive(false);
                    imitationCelling.SetActive(false);
                    upMode = UpMode.Z;
                    break;
                case 4:
                    PuzzleSoundManager.instance.PlayMatchSound();
                    PuzzleSoundManager.instance.PlayPrismArrivedSound();
                    p5.SetActive(true);
                    upMode = UpMode.Y;
                    break;
                case 5:
                    PuzzleSoundManager.instance.PlayGoalMatchSound();
                    PuzzleSoundManager.instance.PlayPrismCorrectSound();
                    p5Anim.SetTrigger("isEnded");
                    gameEnded.GameisEnded();
                    isEnd = true;
                    stopRing = true;
                    break;

            }
            WhatController.instance.ClearVibration();
            Debug.Log(puzzleNumber + "Clear");
            
        }
    }

    
    public void RingOnOff(bool active)
    {
        
        stopRing = active;
    }

    public void NextNumber()
    {
        if (ACheck && BCheck && !isEnd)
        {
            puzzleNumber++;
            AllCheck = true;
            Debug.Log("NextNumber" + puzzleNumber);
            
            
        }
        if (!ACheck && !BCheck)
        {
            AllCheck = false;
        }
    }
}
