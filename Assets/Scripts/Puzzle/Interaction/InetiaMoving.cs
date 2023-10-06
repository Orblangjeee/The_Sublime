using UnityEngine;

public class InetiaMoving : MonoBehaviour
{
    private Vector3 originalPosition;
    private Quaternion originalRotation;
    private bool isReturning = false;

    public float returnSpeed = 2f; // 원래 위치로 돌아가는 속도 조절

    private void Start()
    {
        originalPosition = transform.position;
        originalRotation = transform.rotation;
    }

    private void Update()
    {
        if (isReturning)
        {
            // 원래 위치까지 부드럽게 이동
            transform.position = Vector3.Lerp(transform.position, originalPosition, returnSpeed * Time.deltaTime);
            transform.rotation = Quaternion.Lerp(transform.rotation, originalRotation, returnSpeed * Time.deltaTime);

            // 일정 거리 이하로 가까워지면 원래 위치에 고정
            if (Vector3.Distance(transform.position, originalPosition) < 0.05f)
            {
                transform.position = originalPosition;
                transform.rotation = originalRotation;
                isReturning = false;
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        isReturning = true;
    }
}




