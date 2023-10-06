using UnityEngine;

public class InertiaGravityRotate : MonoBehaviour
{
    private Vector3 targetGravityDirection; // 향하길 원하는 중력 방향

    private void Start()
    {
        targetGravityDirection = -transform.position.normalized;
    }

    private void Update()
    {
        // 현재 중력 방향을 가져옴
        Vector3 currentGravityDirection = Physics.gravity.normalized;

        // 목표 중력 방향과 현재 중력 방향 사이의 회전을 구함
        Quaternion targetRotation = Quaternion.FromToRotation(currentGravityDirection, targetGravityDirection) * transform.rotation;

        // 부드러운 회전 보간을 위해 Slerp를 사용하여 적용
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime);
    }
}