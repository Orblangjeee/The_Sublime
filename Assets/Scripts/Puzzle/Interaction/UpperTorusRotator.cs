using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

[System.Serializable]
public class PuzzleObj_U
{
    public Transform[] puzzles; //followPuzzles 회전값
    public float minValue; //각 퍼즐마다 부여할 회전값 min
    public float maxValue; //각 퍼즐마다 부여할 회전값 max
}

public class UpperTorusRotator : MonoBehaviour
{

    [Header("Torus")]
    public Transform upRing; // 돌릴 Torus 오브젝트
    public float speed = 1f; //Torus 회전 속도
    public float lerpRingSpeed = 0.1f; //그랩(프리즘) Lerp 속도

    private Quaternion initialTorusRotation; // 초기 Torus 회전값
    private Vector3 initialGrabVector; // 컨트롤러 잡은 지점과 Torus 중심의 벡터
    private bool isGrabbing = false; // 현재 Torus를 잡고 있는지 여부
    private float rotationAngle; //Torus 돌린 회전값

    [Header("LocalRotateOBJ")]
    public Transform[] upPuzzles; //퍼즐 배열
    public float puzzleSpeed = 1f; //퍼즐 회전 속도
    public float lerpObjSpeed = 0.1f; //오브젝트 lerp 속도
    public float minClearRange = 3f; //최소 퍼즐 클리어 범위
    public float maxClearRange = 357f; //최대 퍼즐 클리어 범위
    public PuzzleObj_U[] followPuzzles; //puzzle과 같이 돌릴 오브젝트들, min & max value 조절
    
    private List<Quaternion> initialPuzzleRotation = new List<Quaternion>(); //퍼즐 초기 회전값
    private Vector3 currentPuzzleRot; //현재 퍼즐 회전값
    private float clearRot; //정답 회전값

    [Header("GrabOBJ")]
    public Animator tutorialUI;
    public GameObject prism;
    private bool firstGrab = false;
    private Camera mainCamera;

    private XRGrabInteractable grabInteractable; // XR Interaction Toolkit에서 컨트롤러와의 상호작용을 처리
    private XRBaseController controller; //햅틱 호출을 위한 컨트롤러 
    private SphereCollider sphereCollider; //컨트롤러 radius 조절
    private GameObject controllerOnOff; //OnOff할 컨트롤러 오브젝트

    void Start()
    {
        mainCamera = Camera.main;
        // 초기 Torus 회전값 저장
        initialTorusRotation = upRing.rotation;
        //초기 퍼즐 회전값 저장
        for (int i = 0; i < upPuzzles.Length; i++)
        {
            initialPuzzleRotation.Add(upPuzzles[i].localRotation);
            
        }
        minClearRange = followPuzzles[ClearRooms.Instance.puzzleNumber].minValue;
        maxClearRange = followPuzzles[ClearRooms.Instance.puzzleNumber].maxValue;

        // XRGrabInteractable 컴포넌트를 가져와 설정
        grabInteractable = GetComponent<XRGrabInteractable>();

        // 컨트롤러가 Torus를 잡을 때 호출될 이벤트 등록
        grabInteractable.onSelectEntered.AddListener(OnGrab);
        // 컨트롤러가 Torus에서 놓을 때 호출될 이벤트 등록
        grabInteractable.onSelectExited.AddListener(OnRelease);



        //프리즘 비활성화
        prism.SetActive(false);

    }

    // 컨트롤러가 Torus를 잡았을 때 호출될 함수
    private void OnGrab(XRBaseInteractor interactor)
    {
        

        if (!ClearRooms.Instance.stopRing)
        {
            isGrabbing = true;

            // 컨트롤러 잡은 지점과 Torus 중심의 벡터 계산
            initialGrabVector = upRing.position - grabInteractable.selectingInteractor.transform.position;

            // 초기 Torus 회전값 갱신
            initialTorusRotation = upRing.rotation;

            if (ClearRooms.Instance.puzzleNumber <= upPuzzles.Length)
            {
                //초기 퍼즐 회전값 갱신
                initialPuzzleRotation[ClearRooms.Instance.puzzleNumber] = upPuzzles[ClearRooms.Instance.puzzleNumber].localRotation;
            }

            //프리즘 위치를 그랩한 위치로 변경
            prism.transform.position = grabInteractable.GetAttachPoseOnSelect(interactor).position;
            //프리즘은 카메라를 바라보게
            prism.transform.LookAt(mainCamera.transform);
            //프리즘 활성화
            prism.SetActive(true);

            //interactor 부모에 있는 controller 컴포넌트를 가져온다.
            controller = interactor.GetComponentInParent<XRBaseController>();
            //Controller Collider 조정
            sphereCollider = interactor.GetComponentInChildren<SphereCollider>();
            sphereCollider.radius = WhatController.instance.grabRadius;
            //클리어 햅틱을 위한 마지막 잡은 컨트롤러 가져오기
            WhatController.instance.lastController = controller;
            //손 껐다
            controllerOnOff = controller.GetComponentInChildren<HandAnimation>().gameObject;
            controllerOnOff.SetActive(false);
        }

        if (firstGrab == false) //첫 접촉시 튜토리얼 사라지게
        {
            tutorialUI.SetBool("Disappear", true);
            firstGrab = true;
        }

    }



    // 컨트롤러가 Torus에서 놓았을 때 호출될 함수
    private void OnRelease(XRBaseInteractor interactor)
    {
       
        if (!ClearRooms.Instance.stopRing)
        {

            isGrabbing = false;
            //프리즘 비활성화
            prism.SetActive(false);

            //손 켰다
            controllerOnOff.SetActive(true) ;
            //controller 비우기
            controller = null;
            //회전중이 아닐 때 체크
            UpperClear();

            //사운드
            PuzzleSoundManager.instance.U_AudioFadeOut();
        }

    }

    private void Update()
    {
        if (!ClearRooms.Instance.stopRing)
        {
            //프리즘은 실시간으로 카메라를 바라봅니다.
            prism.transform.LookAt(mainCamera.transform);
            //upRing normal 방향으로 프리즘 Z축 맞추기
            prism.transform.up = upRing.transform.up;
        }



    }

    void FixedUpdate()
    {
       
        if (!ClearRooms.Instance.stopRing)
        {
            if (isGrabbing)
            {

                // 컨트롤러의 위치와 Ring 중심의 벡터 계산
                Vector3 controllerToA = upRing.position - grabInteractable.selectingInteractor.transform.position;
                // 컨트롤러의 위치와 Ring 중심의 벡터 각도 계산
                rotationAngle = Vector3.SignedAngle(initialGrabVector, controllerToA, transform.up);

                // 초기 Torus 회전값에 회전 각도를 더한 값을 새로운 회전값으로 설정
                Quaternion newRotation = initialTorusRotation * Quaternion.Euler(0, rotationAngle * speed, 0);

                //돌리기 전, 돌린 후 회전각 구하기
                float compareRot = Quaternion.Angle(upRing.rotation, newRotation);
                //값이 일정 수치 이상이면 진동실행
                if (compareRot > WhatController.instance.compareValue)
                {
                    PuzzleSoundManager.instance.U_AudioRingPlay();

                    //진동
                    controller.SendHapticImpulse(WhatController.instance.ringhapticIntencity, WhatController.instance.ringhapticTime);
                }
                if (compareRot <= 0)
                {

                    //사운드
                    PuzzleSoundManager.instance.U_AudioFadeOut();


                }


                // Torus 오브젝트의 회전을 업데이트
                upRing.rotation = Quaternion.Lerp(upRing.rotation, newRotation, lerpRingSpeed);
               

               
                    RotatingStuff();
               



            }

        }
        YRotClear();
        
        if (ClearRooms.Instance.isEnd)
        {
            
            upPuzzles[ClearRooms.Instance.puzzleNumber].localRotation = Quaternion.Lerp(upPuzzles[ClearRooms.Instance.puzzleNumber].localRotation, Quaternion.Euler(0, 0, 0), 0.1f);
        }


    }

    private void CelarResult()
    {
        
        if (ClearRooms.Instance.AllCheck)
        {
            if (!ClearRooms.Instance.isEnd && !ClearRooms.Instance.stopRing)
            {
               
                //퍼즐들 회전 초기화
                RotFollowPuzzles();
                //퍼즐 정답 min,max 값 바꿔주기
                minClearRange = followPuzzles[ClearRooms.Instance.puzzleNumber].minValue;
                maxClearRange = followPuzzles[ClearRooms.Instance.puzzleNumber].maxValue;
                Debug.Log("Min값 : " + minClearRange + "Max 값 : " + maxClearRange);
                ClearRooms.Instance.ACheck = false;
                ClearRooms.Instance.NextNumber();
            }
        }
       

    }

    private void YRotClear() //클리어시, 0도로 맞추기
    {
        
        if ( ClearRooms.Instance.upClear && upPuzzles[ClearRooms.Instance.puzzleNumber] != null)
        {
            ClearRooms.Instance.upClear = false;
           
            //Y축을 0으로 맞추기
            
            if (ClearRooms.Instance.puzzleNumber != 4)
            {
                upPuzzles[ClearRooms.Instance.puzzleNumber].localRotation = Quaternion.Lerp(upPuzzles[ClearRooms.Instance.puzzleNumber].localRotation, Quaternion.Euler(0, 0, 0), 0.1f);
            }
            ClearRooms.Instance.ACheck = true;
            ClearRooms.Instance.NextNumber();
           
        }
        if (ClearRooms.Instance.upClear && upPuzzles[ClearRooms.Instance.puzzleNumber] == null)
        {
            ClearRooms.Instance.upClear = false;
          
        }
        CelarResult();
    }



    //퍼즐 오브젝트 돌리기
    private void RotatingStuff()
    {
        
        if (ClearRooms.Instance.puzzleNumber <= upPuzzles.Length && upPuzzles[ClearRooms.Instance.puzzleNumber] != null)
        {
            if (ClearRooms.Instance.upMode == ClearRooms.UpMode.X)
            {
                // 초기 Torus 회전값에 회전 각도를 더한 값을 새로운 회전값으로 설정
                Quaternion puzzleRotation = initialPuzzleRotation[ClearRooms.Instance.puzzleNumber] * Quaternion.Euler(rotationAngle * puzzleSpeed, 0, 0);
                // Torus 오브젝트의 회전을 업데이트 //러프로 자연스럽게
                upPuzzles[ClearRooms.Instance.puzzleNumber].localRotation = Quaternion.Lerp(upPuzzles[ClearRooms.Instance.puzzleNumber].localRotation, puzzleRotation, lerpObjSpeed);
                currentPuzzleRot = upPuzzles[ClearRooms.Instance.puzzleNumber].localRotation.eulerAngles;
                //클리어 조건 -> X축
                clearRot = Mathf.Abs(currentPuzzleRot.x);
            }

            if (ClearRooms.Instance.upMode == ClearRooms.UpMode.Y)
            {
                // 초기 Torus 회전값에 회전 각도를 더한 값을 새로운 회전값으로 설정
                Quaternion puzzleRotation = initialPuzzleRotation[ClearRooms.Instance.puzzleNumber] * Quaternion.Euler(0, rotationAngle * puzzleSpeed, 0);
                // Torus 오브젝트의 회전을 업데이트 //러프로 자연스럽게
                upPuzzles[ClearRooms.Instance.puzzleNumber].localRotation = Quaternion.Lerp(upPuzzles[ClearRooms.Instance.puzzleNumber].localRotation, puzzleRotation, lerpObjSpeed);
                currentPuzzleRot = upPuzzles[ClearRooms.Instance.puzzleNumber].localRotation.eulerAngles;
                //클리어 조건 -> Y축
                clearRot = Mathf.Abs(currentPuzzleRot.y);
            }

            if (ClearRooms.Instance.upMode == ClearRooms.UpMode.Z)
            {
                // 초기 Torus 회전값에 회전 각도를 더한 값을 새로운 회전값으로 설정
                Quaternion puzzleRotation = initialPuzzleRotation[ClearRooms.Instance.puzzleNumber] * Quaternion.Euler(0, 0, rotationAngle * puzzleSpeed);
                // Torus 오브젝트의 회전을 업데이트 //러프로 자연스럽게
                upPuzzles[ClearRooms.Instance.puzzleNumber].localRotation = Quaternion.Lerp(upPuzzles[ClearRooms.Instance.puzzleNumber].localRotation, puzzleRotation, lerpObjSpeed);
                currentPuzzleRot = upPuzzles[ClearRooms.Instance.puzzleNumber].localRotation.eulerAngles;
                //클리어 조건 -> Z축
                clearRot = Mathf.Abs(currentPuzzleRot.z);
            }
            


            RotFollowPuzzles();
        }


    }

    //UpRing으로 퍼즐 풀기
    private void UpperClear()
    {
        Debug.Log(MethodBase.GetCurrentMethod().Name);
        Debug.Log(upPuzzles[ClearRooms.Instance.puzzleNumber].name + " : " + currentPuzzleRot);
       
        // puzzle 회전값이 clearRange 범위 사이 일 때 
        if (ClearRooms.Instance.puzzleNumber != 4)
        {
            if (clearRot <= minClearRange || clearRot >= maxClearRange)
            {

                //클리어 결과로 넘어가기
                ClearRooms.Instance.RightCheck();

            }
            // puzzle 회전값이 clearRange 범위 사이가 아닐 때
            else
            {
                ClearRooms.Instance.RightNot();
            }
        } if (ClearRooms.Instance.puzzleNumber == 4) //number 4 는 조건식이 달라용
        {
            if (clearRot <= maxClearRange && clearRot >= minClearRange)
            {

                //클리어 결과로 넘어가기
                ClearRooms.Instance.RightCheck();

            }
            // puzzle 회전값이 clearRange 범위 사이가 아닐 때
            else
            {
                ClearRooms.Instance.RightNot();
            }
        }
        

    
    }

    private void RotFollowPuzzles()
    {
        //follow puzzle이 있으면 puzzle이랑 같이 돌려주기
        if (followPuzzles[ClearRooms.Instance.puzzleNumber] != null)
        {
            if (ClearRooms.Instance.puzzleNumber >= 2 && ClearRooms.Instance.puzzleNumber < 5)
            {
                for (int i = 0; i < followPuzzles[ClearRooms.Instance.puzzleNumber].puzzles.Length; i++)
                {
                    followPuzzles[ClearRooms.Instance.puzzleNumber].puzzles[i].localRotation = upPuzzles[ClearRooms.Instance.puzzleNumber].localRotation;
                }
            }
        }
    }


}
