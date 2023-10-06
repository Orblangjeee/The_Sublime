using UnityEngine;

public class LocalRotationLimit : MonoBehaviour
{
    //링 회전축이 뒤집어지지 않도록 회전값 한계 설정

    public float rotationlimit = 20f; //링 회전 한계 설정
    float decelerationSpeed;//회전 감속 속도
    float decelerationRateX;//현재 X축 각도와 rotationlimit 사이의 값
    float decelerationRateZ;//현재 X축 각도와 rotationlimit 사이의 값
    public float decelerationAntipathy = 8f;//감속 시작할 각도
    Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>(); // Rigidbody 컴포넌트를 가져옴
    }

    private void Update()
    {
        // 현재 로컬 회전 값을 가져옴
        Quaternion currentLocalRotation = transform.localRotation;

        // x축 로컬 회전 값을 -30도에서 30도 사이로 제한
        if (currentLocalRotation.eulerAngles.x > rotationlimit && currentLocalRotation.eulerAngles.x < 180)
        { currentLocalRotation.eulerAngles = new Vector3(rotationlimit, currentLocalRotation.eulerAngles.y, currentLocalRotation.eulerAngles.z); }
        else if (currentLocalRotation.eulerAngles.x < 360 - rotationlimit && currentLocalRotation.eulerAngles.x > 180)
        { currentLocalRotation.eulerAngles = new Vector3(360 - rotationlimit, currentLocalRotation.eulerAngles.y, currentLocalRotation.eulerAngles.z); }


        // z축 로컬 회전 값을 -30도에서 30도 사이로 제한
        if (currentLocalRotation.eulerAngles.z > rotationlimit && currentLocalRotation.eulerAngles.z < 180)
        { currentLocalRotation.eulerAngles = new Vector3(currentLocalRotation.eulerAngles.x, currentLocalRotation.eulerAngles.y, rotationlimit); }
        else if (currentLocalRotation.eulerAngles.z < 360 - rotationlimit && currentLocalRotation.eulerAngles.z > 180)
        { currentLocalRotation.eulerAngles = new Vector3(currentLocalRotation.eulerAngles.x, currentLocalRotation.eulerAngles.y, 360 - rotationlimit); }

        // 수정된 로컬 회전 값을 적용
        transform.localRotation = currentLocalRotation;


        //각 회전값이 30에 가까워질수록 회전속도 감속
        if (currentLocalRotation.eulerAngles.x > rotationlimit - decelerationAntipathy && currentLocalRotation.eulerAngles.x < 180)
        {
            decelerationRateX = (rotationlimit - transform.localEulerAngles.x);
            rb.angularVelocity *= decelerationRateX / decelerationAntipathy;
        }

        if (currentLocalRotation.eulerAngles.z > rotationlimit - decelerationAntipathy && currentLocalRotation.eulerAngles.z < 180)
        {
            decelerationRateZ = (rotationlimit - transform.localEulerAngles.z);
            rb.angularVelocity *= decelerationRateZ / decelerationAntipathy;
        }

        if (currentLocalRotation.eulerAngles.x < 360 - rotationlimit + decelerationAntipathy && currentLocalRotation.eulerAngles.x > 180)
        {
            decelerationRateX = (transform.localEulerAngles.x - (360 - rotationlimit));
            rb.angularVelocity *= decelerationRateX / decelerationAntipathy;
        }

        if (currentLocalRotation.eulerAngles.z < 360 - rotationlimit + decelerationAntipathy && currentLocalRotation.eulerAngles.z > 180)
        {
            decelerationRateZ = (transform.localEulerAngles.z - (360 - rotationlimit));
            rb.angularVelocity *= decelerationRateZ / decelerationAntipathy;
        }
    }
}