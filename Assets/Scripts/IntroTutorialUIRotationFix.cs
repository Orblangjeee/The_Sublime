using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroTutorialUIRotationFix : MonoBehaviour
{
    private Quaternion initialRotation;
    public float rotSpeed = 2.0f; // 회전 속도 조절

    private void Start()
    {
        // UI 요소의 초기 회전값을 저장합니다.
        initialRotation = transform.rotation;
    }

    private void LateUpdate()
    {
        // UI 요소의 X, Z 축 회전값을 고정합니다.
        Vector3 eulerRotation = transform.rotation.eulerAngles;
        eulerRotation.x = 0f; // X 축 회전을 0으로 고정
        eulerRotation.z = 0f; // Z 축 회전을 0으로 고정
        transform.rotation = Quaternion.Euler(eulerRotation);

        //---------------다이얼로그의 회전값이 플레이어 Y축 방향을 따라가도록 만들기 ------------------
        if (Camera.main != null) // 현재 메인 카메라가 있는지 확인
        {
            Transform mainCamera = Camera.main.transform; // 현재 메인 카메라 참조

            // 현재 카메라의 forward 벡터와 y축 방향을 계산
            Vector3 forwardWithYRotation = Vector3.ProjectOnPlane(mainCamera.forward, Vector3.up).normalized;
            Quaternion yRotation = Quaternion.LookRotation(forwardWithYRotation, Vector3.up);

            // 현재 오브젝트의 회전을 y축 방향으로 보간
            transform.rotation = Quaternion.Lerp(transform.rotation, yRotation, rotSpeed * Time.deltaTime);
        }
    }
}