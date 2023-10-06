using UnityEngine;

public class InertiaGravityMove : MonoBehaviour
{
    private Rigidbody rb;
    private Vector3 originalPosition;
    public float returnSpeed = 5f; // 원래 위치로 돌아가는 속도 조절
    private float gravityStrength = 0f; // 초기 중력 강도 설정
    public float rotationDamping = 0.98f; // 회전 감속 계수
    public float maxGravity = 10f; // 최대 중력 강도 조절
    private bool isTouched = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        originalPosition = transform.position;
    }

    private void Update()
    {

        // 원래 위치까지의 방향 계산
        Vector3 toOriginalPosition = originalPosition - transform.position;

        // 원래 위치 방향으로 중력 적용
        Vector3 gravityForce = toOriginalPosition.normalized * gravityStrength;
        rb.AddForce(gravityForce, ForceMode.Force);

        // 원래 위치로 돌아가기 위해 필요한 속도 계산
        Vector3 velocityToOriginal = toOriginalPosition * returnSpeed;
        rb.velocity += velocityToOriginal * Time.deltaTime;

        // 회전 감속
        rb.angularVelocity *= rotationDamping; // 회전 속도 감소율 조절

        if (rb.velocity.magnitude < 0.1f) // 움직임이 감지되지 않으면
        {
            // 움직임이 감지되지 않을 때 보간을 사용하여 속도를 0으로 감소시킴
            rb.velocity = Vector3.Lerp(rb.velocity, Vector3.zero, Time.deltaTime * 1.5f); // 조절 가능한 감소 속도
        }

    }


    private void OnCollisionExit(Collision collision)
    {
        gravityStrength = maxGravity;
        // 충돌이 없을 때 중력 강도를 천천히 감소시켜 0으로 만듦
        StartCoroutine(DecreaseGravity());
        isTouched = true;

    }

    private System.Collections.IEnumerator DecreaseGravity()
    {
        while (gravityStrength > 0) //물체에 가해진 중력값이 0보다 크면
        {
            //1초마다 중력값이 서서히 감소
            gravityStrength -= maxGravity * 0.1f;
            yield return new WaitForSeconds(0.1f);
        }
        gravityStrength = 0f;//중력을 0으로 고정
    }

}