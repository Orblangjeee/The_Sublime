using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class PlayerMovement : MonoBehaviour
{
    
     

    [Header("Move Info")]
    private float inputVertical = 0f; // 키보드 입력값 받기
    public float moveSpeed = 10f;  // 캐릭터 이동 속도
    public float tileSize = 10f;   // 타일의 너비

    private bool isMoving = false;  // 이동 중인지 여부를 나타내는 변수
    public bool IsMoving { get => isMoving; } // isMoving값을 밖에서 읽을 수 있도록
    // Get/Set Property
    private bool isButtonPressed = false;  // 이동 버튼이 눌렸는지 여부를 나타내는 변수

    [Header("Trigger Info")]
    private bool canMove = true;

    [Header("Ray Info")]
    private bool canMoveForward = true;
    private bool canMoveBackward = true;

    
    [Header("Gizmo Info")]
    public GameObject GizmoPoint;
    public GameObject BackwardGizmoPoint;
    

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //이동
        if (canMove)
        {
            // 조이스틱 입력 감지
            inputVertical = Input.GetAxisRaw("Vertical");
            
                
                // 입력에 따라 캐릭터를 이동시킨다
                if (inputVertical > 0.8) // 직진 키 입력 감지
                {
                    if (!isButtonPressed && canMoveForward == true)
                    {
                        StartCoroutine(Move(Vector3.forward));
                        isButtonPressed = true;
                    Debug.Log(inputVertical + "Up");
                    }
                }
                else if (inputVertical < -0.8 && canMoveBackward == true) // 후진 키 입력 감지
                {
                    if (!isButtonPressed)
                    {
                        StartCoroutine(Move(Vector3.back));
                        isButtonPressed = true;
                    Debug.Log(inputVertical + "Down");
                }
                }
                else
                {
                    isButtonPressed = false;
                }
            
           
        }
        /*
        // 타일 감지할 Ray 생성
        Ray ray = new Ray(GizmoPoint.transform.position, GizmoPoint.transform.forward);
        //RayCast 생성
        RaycastHit hitInfo;
        //충돌 제외할 Layer 설정
        int layer = 1 << LayerMask.NameToLayer("Tile");
        //Ray 발사
        if (Physics.Raycast(ray, out hitInfo, Mathf.Infinity, layer))//Gizmo용 Ray가 Tile에 부딪힌 경우
        {
            canMoveForward = true; //전진(w)을 허가한다
        }
        else //Ray가 부딪히지 않은 경우
        {
            canMoveForward = false; //전진(w)을 막는다
        }

        //Ray 하나더 생성
        Ray backwardray = new Ray(BackwardGizmoPoint.transform.position, BackwardGizmoPoint.transform.forward);
        //RayCast 생성
        RaycastHit backwardHitInfo;
        //충돌 제외할 Layer 설정
        int Layer = 1 << LayerMask.NameToLayer("Tile");
        //Ray 발사
        if (Physics.Raycast(backwardray, out backwardHitInfo, Mathf.Infinity, Layer))//Gizmo용 Ray가 Tile에 부딪힌 경우
        {
            canMoveBackward = true; //후진(s)을 허가한다
        }
        else //Ray가 부딪히지 않은 경우
        {
            canMoveBackward = false; //후진(s)을 막는다
        }*/

    }

    public IEnumerator Move(Vector3 direction)
    {
        if (isMoving) yield break; // 이미 이동 중이라면 추가적인 이동을 막는다
       // if (isRotating) yield break;  // 회전 중이라면 추가적인 이동을 막는다

        isMoving = true;  // 이동 시작

        float remainingDistance = tileSize;  // 남은 이동 거리
        Vector3 startPosition = transform.position;  // 시작 위치
        Vector3 endPosition = startPosition + direction * tileSize;  // 목표 위치

        while (remainingDistance > 0)
        {
            float moveDistance = moveSpeed * Time.deltaTime;  // 이동 거리 계산
            remainingDistance -= moveDistance;  // 남은 이동 거리 업데이트

            transform.Translate(direction * moveDistance);  // 캐릭터 이동

            yield return null;  // 다음 프레임까지 대기
        }

        isMoving = false;  // 이동 종료
    }

    /*
    public void OnDrawGizmos()
    {
        //Gizmo용 Ray 생성
        Ray ray = new Ray(GizmoPoint.transform.position, GizmoPoint.transform.forward);
        //Gizmo용 RayCast 생성
        RaycastHit hitInfo;
        //충돌 제외할 Layer 설정
        int layer = 1 << LayerMask.NameToLayer("Tile");
        //Gizmo용 Ray 발사
        if (Physics.Raycast(ray, out hitInfo, Mathf.Infinity, layer))//Gizmo용 Ray가 Tile에 부딪힌 경우
        {
            Gizmos.color = Color.red;
            Vector3 startPos = ray.origin;
            Vector3 endPos = hitInfo.point;
            Gizmos.DrawLine(startPos, endPos);
            //Gizmo용 Ray를 부딪힌 위치까지만 그려준다
        }
        else //Gizmo용 Ray가 부딪히지 않은 경우
        {
            Gizmos.color = Color.white;
            Vector3 startPos = ray.origin;
            Vector3 endPos = startPos + ray.direction * 100f;
            Gizmos.DrawLine(startPos, endPos);
            //Gizmo용 Ray의 원래 길이(100)만큼 그려준다
        }

        //Gizmo용 Ray 하나더 생성
        Ray backwardray = new Ray(BackwardGizmoPoint.transform.position, BackwardGizmoPoint.transform.forward);
        //Gizmo용 RayCast 생성
        RaycastHit backwardHitInfo;
        //충돌 제외할 Layer 설정
        int Layer = 1 << LayerMask.NameToLayer("Tile");
        //Gizmo용 Ray 발사
        if (Physics.Raycast(backwardray, out backwardHitInfo, Mathf.Infinity, Layer))//Gizmo용 Ray가 Tile에 부딪힌 경우
        {
            Gizmos.color = Color.red;
            Vector3 BackwardstartPos = backwardray.origin;
            Vector3 BackwardendPos = backwardHitInfo.point;
            Gizmos.DrawLine(BackwardstartPos, BackwardendPos);
            //Gizmo용 Ray를 부딪힌 위치까지만 그려준다
        }
        else //Gizmo용 Ray가 부딪히지 않은 경우
        {
            Gizmos.color = Color.white;
            Vector3 BackwardstartPos = backwardray.origin;
            Vector3 BackwardendPos = BackwardstartPos + backwardray.direction * 100f;
            Gizmos.DrawLine(BackwardstartPos, BackwardendPos);
            //Gizmo용 Ray의 원래 길이(100)만큼 그려준다
        }
    }*/
}
