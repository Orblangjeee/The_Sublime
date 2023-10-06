using JetBrains.Annotations;
using System.Collections;
using UnityEngine;

public class SubTitleInViewportBackup : MonoBehaviour
{
    public Transform mainCamera; // 메인 카메라의 위치값
    public Transform target; // 다이얼로그 오브젝트의 기본 위치값
    public Transform uipos; // UI 위치를 저장할 변수
    public float smoothSpeed = 2f; // 부드러운 이동을 위한 스무딩 스피드
    public float rotSpeed = 3f; // 회전 속도
    public Camera playerCamera; // 플레이어 카메라
    public float maxRotationAngle = 30f; // 최대 회전 각도
    public float forwardDistance = 8f; // 타겟과의 전방 거리

    public float pullingDegree = 1.5f; // 중앙 위치에서 uipos 방향으로 당기는 정도
    public float xScreenPositionLimit = 1f; // X축 스크린 위치 제한값
    public float yScreenPositionLimit = 1f; // Y축 스크린 위치 제한값
    public float zScreenPositionLimit = 100f; // Z축 스크린 위치 제한값

    private void Start()
    {
        mainCamera = Camera.main.transform; //초기 카메라 위치 설정

    }

    private void Awake()
    {
        transform.position = uipos.position; //시작시 다이얼로그 UI 위치 초기화
    }

    void LateUpdate()
    {
        Vector3 direction = transform.position - mainCamera.position; // 카메라와 다이얼로그 사이의 방향 계산.
        direction.Normalize(); // 방향 정규화

        if (IsCaptionVisibleOnViewport()) // 다이얼로그가 뷰포트 상에 보이는지 확인.
        {
            //다이얼로그를 uipos 위치로 부드럽게 이동
            transform.position = Vector3.Lerp(transform.position, uipos.position + mainCamera.forward, smoothSpeed * Time.deltaTime);
        }
        else
        {

            //다이얼로그를 카메라 중앙 위치로 부드럽게 이동
            //uipos.position과 카메라 중앙 위치의 중간 지점 산출(중간 지점의 비율 변수로 조절 가능)
            Vector3 middlePosition = (uipos.position + mainCamera.position) / pullingDegree;
            //다이얼로그 화면으로부터의 거리 설정
            Vector3 followCameraPos = middlePosition + mainCamera.forward * forwardDistance;
            //다이얼로그를 중간 지점으로 부드럽게 이동
            transform.position = Vector3.Lerp(transform.position, followCameraPos, smoothSpeed * Time.deltaTime);

        }


        //---------------다이얼로그의 회전값이 플레이어 방향을 따라가도록 만들기 ------------------
        //현재 카메라의 forward 벡터를 계산
        //Quaternion rot = Quaternion.LookRotation(mainCamera.forward);
        //현재 오브젝트의 회전값을 보간.
        //transform.rotation = Quaternion.Lerp(transform.rotation, rot, rotSpeed * Time.deltaTime);


        //---------------다이얼로그의 회전값이 플레이어 Y축 방향을 따라가도록 만들기 ------------------
        //현재 카메라의 forward 벡터와 y축 방향을 계산
        Vector3 forwardWithYRotation = Vector3.ProjectOnPlane(mainCamera.forward, Vector3.up).normalized;
        Quaternion yRotation = Quaternion.LookRotation(forwardWithYRotation, Vector3.up);
        //현재 오브젝트의 회전을 y축 방향으로 보간
        transform.rotation = Quaternion.Lerp(transform.rotation, yRotation, rotSpeed * Time.deltaTime);

    }

    private bool IsCaptionVisibleOnViewport() // 다이얼로그가 뷰포트 상에 위치해 있는지 여부에 대한 조건 설정
    {
        Vector3 screenPosition = playerCamera.WorldToViewportPoint(uipos.position); //uipos의 월드 좌표를 카메라의 뷰포트 좌표로 변환한 값 반환
        Debug.Log(screenPosition); //현재 스크린 레이 위치 출력
        // 화면 위치 제한 내에 있는지 확인.
        return (screenPosition.x > 1 - xScreenPositionLimit && screenPosition.x <= xScreenPositionLimit && screenPosition.y > 1 - yScreenPositionLimit && screenPosition.y <= yScreenPositionLimit && screenPosition.z > 0 && screenPosition.z <= zScreenPositionLimit);
    }
}