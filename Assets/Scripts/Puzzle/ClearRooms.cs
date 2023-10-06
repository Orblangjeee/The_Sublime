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
    //�� ��° �������� �޾ƿ���
    public int PuzzleNumber
    {
        get { return puzzleNumber; }
        set { puzzleNumber = value; }
    }

    [Header("All")]
    public bool rightRing = false; //������ ���� �ڸ��� �ִ���
    public bool leftRing = false; //������ ���� �ڸ��� �ִ���
    public bool upClear = false; //Clear üũ�ؼ� Rot 0���� ���߱� upRing
    public bool bottomClear = false; //Clear üũ�ؼ� Rot 0���� ���߱� bottomRing
    public bool stopRing = false; // Ring On/Off
    public bool isEnd = false; //������ ����
    
    //�������� ��� üũ�� ���� Ȯ�� ����
    public bool ACheck = false;
    public bool BCheck = false;
    public bool AllCheck = false;

    [Header("Mode")]
    public UpMode upMode; //upRing ȸ�� ��� ����
    public BottomMode bottomMode; //bottomRing ȸ�� ��� ����
    public enum UpMode { X,Y,Z } 
    public enum BottomMode { X,Y,Z }


    [Header("Animation")]
    //�ִϸ��̼�
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

   //���� BottomRing �Ϸ� ��
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
    //���� BottomRing ������ �ƴ� ��, ���
    public void leftNot()
    {
      bottomClear = false;
      leftRing = false;
    }
    //���� UpRing �Ϸ� ��
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
    //All Ŭ���� üũ
    public void ClearCheck()
    {
        if (rightRing && leftRing)
        {
            leftRing = false;
            rightRing = false;
            //���� ȸ���� 0���� ���߱�
            upClear = true;
            bottomClear = true;
            //Ŭ������ �� �̾����� ����
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
                    //����° ���� ����
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
