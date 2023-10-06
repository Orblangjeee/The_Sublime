using System.Collections.Generic;
using System.Net;
using UnityEngine;
using UnityEngine.ProBuilder.MeshOperations;
using UnityEngine.XR.Interaction.Toolkit;

[System.Serializable]
public class PuzzleObj_B
{
    public Transform[] puzzles; //followPuzzles ȸ����
    public float minVlaue; //�� ���񸶴� �ο��� ȸ���� min
    public float maxVlaue; //�� ���񸶴� �ο��� ȸ���� max
}
public class BottomTorusRotator : MonoBehaviour
{
    [Header("Torus")]
    public Transform bottomRing; // ���� Torus ������Ʈ
    public float speed = 1f; //Torus ȸ�� �ӵ�
    public float lerpRingSpeed = 0.1f; //�׷�(������) Lerp �ӵ�

    private Quaternion initialTorusRotation; // �ʱ� Torus ȸ����
    private Vector3 initialGrabVector; // ��Ʈ�ѷ� ���� ������ Torus �߽��� ����
    private bool isGrabbing = false; // ���� Torus�� ��� �ִ��� ����
    private float rotationAngle; //Torus ���� ȸ����


    [Header("LocalRotateOBJ")]
    public Transform[] bottomPuzzles; //���� �迭
    public float lerpObjSpeed = 0.1f; //������Ʈ lerp �ӵ�
    public float puzzleSpeed = 1f; //���� ȸ�� �ӵ�
    public float minClearRange = 3f; //�ּ� ���� Ŭ���� ����
    public float maxClearRange = 357f; //�ִ� ���� Ŭ���� ����
    public PuzzleObj_B[] followPuzzles; //puzzle�� ���� ���� ������Ʈ��
    
    private List<Quaternion> initialPuzzleRotation = new List<Quaternion>(); //���� �ʱ� ȸ����
    private Vector3 currentPuzzleRot; //���� ���� ȸ����
    private float clearRot; //���� ���� ȸ����

    [Header("GrabOBJ")]
    public Animator tutorialUI;
    public GameObject prism;
    private bool firstGrab = false;
    private Camera mainCamera;


    private XRGrabInteractable grabInteractable; // XR Interaction Toolkit���� ��Ʈ�ѷ����� ��ȣ�ۿ��� ó��
    private XRBaseController controller; //��ƽ ȣ���� ���� ��Ʈ�ѷ� 
    private SphereCollider sphereCollider; //��Ʈ�ѷ� radius ����
    private GameObject controllerOnOff; //OnOff�� ��Ʈ�ѷ� ������Ʈ
    void Start()
    {
       

        mainCamera = Camera.main;
        // �ʱ� Torus ȸ���� ����
        initialTorusRotation = bottomRing.rotation;
        //�ʱ� ���� ȸ���� ����
        
        for (int i = 0; i < bottomPuzzles.Length; i++)
        {
            if (bottomPuzzles[i] == null)
            {
                initialPuzzleRotation.Add(Quaternion.Euler(0,0,0));

            }
            else
            {
                initialPuzzleRotation.Add(bottomPuzzles[i].localRotation);
            }
             
            
        }

        minClearRange = followPuzzles[ClearRooms.Instance.puzzleNumber].minVlaue;
        maxClearRange = followPuzzles[ClearRooms.Instance.puzzleNumber].maxVlaue;


        // XRGrabInteractable ������Ʈ�� ������ ����
        grabInteractable = GetComponent<XRGrabInteractable>();

        // ��Ʈ�ѷ��� Torus�� ���� �� ȣ��� �̺�Ʈ ���
        grabInteractable.onSelectEntered.AddListener(OnGrab);
        // ��Ʈ�ѷ��� Torus���� ���� �� ȣ��� �̺�Ʈ ���
        grabInteractable.onSelectExited.AddListener(OnRelease);

        //������ ��Ȱ��ȭ
        prism.SetActive(false);
  
        //�ִϸ��̼����� ���� puzzle4 �ʱ� ȸ���� ����
        initialPuzzleRotation[4] = Quaternion.Euler(0, 180, 0);
        Debug.Log(initialPuzzleRotation[4].eulerAngles);
    }

    // ��Ʈ�ѷ��� Torus�� ����� �� ȣ��� �Լ�
    private void OnGrab(XRBaseInteractor interactor)
    {
        if (!ClearRooms.Instance.stopRing)
        {
            isGrabbing = true;

            // ��Ʈ�ѷ� ���� ������ Torus �߽��� ���� ���
            initialGrabVector = bottomRing.position - grabInteractable.selectingInteractor.transform.position;

            // �ʱ� Torus ȸ���� ����
            initialTorusRotation = bottomRing.rotation;

            if (bottomPuzzles[ClearRooms.Instance.puzzleNumber] != null && ClearRooms.Instance.puzzleNumber <= bottomPuzzles.Length)
            {
                //�ʱ� ���� ȸ���� ����
                initialPuzzleRotation[ClearRooms.Instance.puzzleNumber] = bottomPuzzles[ClearRooms.Instance.puzzleNumber].localRotation;
                
            }

            //������ ��ġ�� �׷��� ��ġ�� ����
            prism.transform.position = grabInteractable.GetAttachPoseOnSelect(interactor).position;
            //�������� ī�޶� �ٶ󺸰�
            prism.transform.LookAt(mainCamera.transform);
            //������ Ȱ��ȭ
            prism.SetActive(true);

            //interactor �θ� �ִ� controller ������Ʈ�� �����´�.
            controller = interactor.GetComponentInParent<XRBaseController>();
            //Controller Collider ����
            sphereCollider = interactor.GetComponentInChildren<SphereCollider>();
            sphereCollider.radius = WhatController.instance.grabRadius;
            //Ŭ���� ��ƽ�� ���� ������ ���� ��Ʈ�ѷ� ��������
            WhatController.instance.lastController = controller;
            //�� ����
            controllerOnOff = controller.GetComponentInChildren<HandAnimation>().gameObject;
            controllerOnOff.SetActive(false);

        }

        if (firstGrab == false) //ù ���˽� Ʃ�丮�� �������
        {
            tutorialUI.SetBool("Disappear", true);
            firstGrab = true;
        }

    }

    // ��Ʈ�ѷ��� Torus���� ������ �� ȣ��� �Լ�
    private void OnRelease(XRBaseInteractor interactor)
    {
        if (!ClearRooms.Instance.stopRing)
        {
            isGrabbing = false;
            //������ ��Ȱ��ȭ
            prism.SetActive(false);
            //�� �״�
            controllerOnOff.SetActive(true);
            //controller ����
            controller = null;
            //ȸ������ �ƴ� �� üũ
            BottomClear();

            //����
            PuzzleSoundManager.instance.B_AudioFadeOut();
        }

    }

    private void Update()
    {
        if (!ClearRooms.Instance.stopRing)
        {
            //�������� �ǽð����� ī�޶� �ٶ󺾴ϴ�.
            prism.transform.LookAt(mainCamera.transform);
            //bottomRing normal �������� ������ Z�� ���߱�
            prism.transform.up = bottomRing.transform.up;
        }

    }


    void FixedUpdate()
    {
        if (!ClearRooms.Instance.stopRing)
        {
            if (isGrabbing)
            {
               
                // ��Ʈ�ѷ��� ��ġ�� Ring �߽��� ���� ���
                Vector3 controllerToA = bottomRing.position - grabInteractable.selectingInteractor.transform.position;
                // ��Ʈ�ѷ��� ��ġ�� Ring �߽��� ���� ���� ���
                rotationAngle = Vector3.SignedAngle(initialGrabVector, controllerToA, transform.up);

                // �ʱ� Torus ȸ������ ȸ�� ������ ���� ���� ���ο� ȸ�������� ����
                Quaternion newRotation = initialTorusRotation * Quaternion.Euler(0, rotationAngle * speed, 0);

                //������ ��, ���� �� ȸ���� ���ϱ�
                float compareRot = Quaternion.Angle(bottomRing.rotation, newRotation);
                //���� ���� ��ġ �̻��̸� ��������
                if (compareRot > WhatController.instance.compareValue)
                {

                    //����

                    PuzzleSoundManager.instance.B_AudioRingPlay();

                    //����
                    controller.SendHapticImpulse(WhatController.instance.ringhapticIntencity, WhatController.instance.ringhapticTime);

                }
                if (compareRot <= 0)
                {

                    //���� ����

                    PuzzleSoundManager.instance.B_AudioFadeOut();

                }

                // Torus ������Ʈ�� ȸ���� ������Ʈ
                bottomRing.rotation = Quaternion.Lerp(bottomRing.rotation, newRotation, lerpRingSpeed);

                RotatingStuff();

            }

        }
        YRotClear();
        ClearResult();

        if (ClearRooms.Instance.isEnd)
        {

            bottomPuzzles[ClearRooms.Instance.puzzleNumber].localRotation = Quaternion.Lerp(bottomPuzzles[ClearRooms.Instance.puzzleNumber].localRotation, Quaternion.Euler(0, 0, 0), 0.1f);
        }
    }

    private void ClearResult()
    {
        if (ClearRooms.Instance.AllCheck) 
        {
            if (!ClearRooms.Instance.isEnd && !ClearRooms.Instance.stopRing)
            {

                //����� ȸ�� �ʱ�ȭ
                RotFollowPuzzles();
                //���� ���� min,max �� �ٲ��ֱ�
                minClearRange = followPuzzles[ClearRooms.Instance.puzzleNumber].minVlaue;
                maxClearRange = followPuzzles[ClearRooms.Instance.puzzleNumber].maxVlaue;
                Debug.Log("Min�� : " + minClearRange + "Max �� : " + maxClearRange);
                ClearRooms.Instance.BCheck = false;
                
                ClearRooms.Instance.NextNumber();
            }
        }
        
    }

    private void YRotClear()
    {
        if (ClearRooms.Instance.bottomClear &&bottomPuzzles[ClearRooms.Instance.puzzleNumber] != null)
        {
                ClearRooms.Instance.bottomClear = false;
               
                //Y���� 0���� ���߱�
                bottomPuzzles[ClearRooms.Instance.puzzleNumber].localRotation = Quaternion.Lerp(bottomPuzzles[ClearRooms.Instance.puzzleNumber].localRotation, Quaternion.Euler(0, 0, 0), 0.1f);
                ClearRooms.Instance.BCheck = true;
                Debug.Log("Bcheck = true");
                 ClearRooms.Instance.NextNumber();
             Debug.Log("bottomClear = false");

        } 
        if (ClearRooms.Instance.bottomClear && bottomPuzzles[ClearRooms.Instance.puzzleNumber] == null)
        {
            ClearRooms.Instance.bottomClear = false;
            Debug.Log("bottomClear = false");
            ClearRooms.Instance.BCheck = true;
            Debug.Log("Bcheck = true");
            
        }
        
    }



    //Stuff ������Ʈ ������
    private void RotatingStuff()
    {
        if (bottomPuzzles[ClearRooms.Instance.puzzleNumber] != null && ClearRooms.Instance.puzzleNumber <= bottomPuzzles.Length)
        {
            if (ClearRooms.Instance.bottomMode == ClearRooms.BottomMode.X)
            {
                // �ʱ� Torus ȸ������ ȸ�� ������ ���� ���� ���ο� ȸ�������� ����
                Quaternion puzzleRotation = initialPuzzleRotation[ClearRooms.Instance.puzzleNumber] * Quaternion.Euler(rotationAngle * puzzleSpeed, 0, 0);
                // Torus ������Ʈ�� ȸ���� ������Ʈ
                bottomPuzzles[ClearRooms.Instance.puzzleNumber].localRotation = Quaternion.Lerp(bottomPuzzles[ClearRooms.Instance.puzzleNumber].localRotation, puzzleRotation, lerpObjSpeed);
                currentPuzzleRot = bottomPuzzles[ClearRooms.Instance.puzzleNumber].localRotation.eulerAngles;
                //Ŭ���� ���� -> X��
                clearRot = Mathf.Abs(currentPuzzleRot.x);
            }
            if (ClearRooms.Instance.bottomMode == ClearRooms.BottomMode.Y)
            {
                // �ʱ� Torus ȸ������ ȸ�� ������ ���� ���� ���ο� ȸ�������� ����
                Quaternion puzzleRotation = initialPuzzleRotation[ClearRooms.Instance.puzzleNumber] * Quaternion.Euler(0, rotationAngle * puzzleSpeed, 0);
                // Torus ������Ʈ�� ȸ���� ������Ʈ
                bottomPuzzles[ClearRooms.Instance.puzzleNumber].localRotation = Quaternion.Lerp(bottomPuzzles[ClearRooms.Instance.puzzleNumber].localRotation, puzzleRotation, lerpObjSpeed);
                currentPuzzleRot = bottomPuzzles[ClearRooms.Instance.puzzleNumber].localRotation.eulerAngles;
                //Ŭ���� ���� -> Y��
                clearRot = Mathf.Abs(currentPuzzleRot.y);
            }
            if (ClearRooms.Instance.bottomMode == ClearRooms.BottomMode.Z)
            {
                // �ʱ� Torus ȸ������ ȸ�� ������ ���� ���� ���ο� ȸ�������� ����
                Quaternion puzzleRotation = initialPuzzleRotation[ClearRooms.Instance.puzzleNumber] * Quaternion.Euler(0, 0, rotationAngle * puzzleSpeed);
                // Torus ������Ʈ�� ȸ���� ������Ʈ
                bottomPuzzles[ClearRooms.Instance.puzzleNumber].localRotation = Quaternion.Lerp(bottomPuzzles[ClearRooms.Instance.puzzleNumber].localRotation, puzzleRotation, lerpObjSpeed);
                currentPuzzleRot = bottomPuzzles[ClearRooms.Instance.puzzleNumber].localRotation.eulerAngles;
                //Ŭ���� ���� -> Z��
                clearRot = Mathf.Abs(currentPuzzleRot.z);
            }

            RotFollowPuzzles();
            
        }

    }

    //BottomRing���� ���� Ǯ��
    private void BottomClear()
    {
        if (bottomPuzzles[ClearRooms.Instance.puzzleNumber] != null) { Debug.Log(bottomPuzzles[ClearRooms.Instance.puzzleNumber].name + " : " + currentPuzzleRot); }
       
        
        // puzzle ȸ������ clearRange ���� ���� �� �� 
        if (clearRot <= minClearRange || clearRot >= maxClearRange)
        {

            //Ŭ���� ����� �Ѿ��
            ClearRooms.Instance.leftCheck();

        }
        // puzzle ȸ������ clearRange ���� ���̰� �ƴ� ��
        else
        {
            ClearRooms.Instance.leftNot();

        }
        
    }


    private void RotFollowPuzzles()
    {
        //puzzle2 ����, �ٸ� ������Ʈ�鵵 ���� �����ֱ�
        if (followPuzzles[ClearRooms.Instance.puzzleNumber] != null)
        {
            if (ClearRooms.Instance.puzzleNumber >= 2 && ClearRooms.Instance.puzzleNumber < 4)
            {
                for (int i = 0; i < followPuzzles[ClearRooms.Instance.puzzleNumber].puzzles.Length; i++)
                {
                    followPuzzles[ClearRooms.Instance.puzzleNumber].puzzles[i].localRotation = bottomPuzzles[ClearRooms.Instance.puzzleNumber].localRotation;
                }

            }
        }
        
    }
}
