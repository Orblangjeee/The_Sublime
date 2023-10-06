using UnityEngine;

public class LocalRotationLimit : MonoBehaviour
{
    //�� ȸ������ ���������� �ʵ��� ȸ���� �Ѱ� ����

    public float rotationlimit = 20f; //�� ȸ�� �Ѱ� ����
    float decelerationSpeed;//ȸ�� ���� �ӵ�
    float decelerationRateX;//���� X�� ������ rotationlimit ������ ��
    float decelerationRateZ;//���� X�� ������ rotationlimit ������ ��
    public float decelerationAntipathy = 8f;//���� ������ ����
    Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>(); // Rigidbody ������Ʈ�� ������
    }

    private void Update()
    {
        // ���� ���� ȸ�� ���� ������
        Quaternion currentLocalRotation = transform.localRotation;

        // x�� ���� ȸ�� ���� -30������ 30�� ���̷� ����
        if (currentLocalRotation.eulerAngles.x > rotationlimit && currentLocalRotation.eulerAngles.x < 180)
        { currentLocalRotation.eulerAngles = new Vector3(rotationlimit, currentLocalRotation.eulerAngles.y, currentLocalRotation.eulerAngles.z); }
        else if (currentLocalRotation.eulerAngles.x < 360 - rotationlimit && currentLocalRotation.eulerAngles.x > 180)
        { currentLocalRotation.eulerAngles = new Vector3(360 - rotationlimit, currentLocalRotation.eulerAngles.y, currentLocalRotation.eulerAngles.z); }


        // z�� ���� ȸ�� ���� -30������ 30�� ���̷� ����
        if (currentLocalRotation.eulerAngles.z > rotationlimit && currentLocalRotation.eulerAngles.z < 180)
        { currentLocalRotation.eulerAngles = new Vector3(currentLocalRotation.eulerAngles.x, currentLocalRotation.eulerAngles.y, rotationlimit); }
        else if (currentLocalRotation.eulerAngles.z < 360 - rotationlimit && currentLocalRotation.eulerAngles.z > 180)
        { currentLocalRotation.eulerAngles = new Vector3(currentLocalRotation.eulerAngles.x, currentLocalRotation.eulerAngles.y, 360 - rotationlimit); }

        // ������ ���� ȸ�� ���� ����
        transform.localRotation = currentLocalRotation;


        //�� ȸ������ 30�� ����������� ȸ���ӵ� ����
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