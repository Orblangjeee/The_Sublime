using UnityEngine;

public class InertiaGravityRotate : MonoBehaviour
{
    private Vector3 targetGravityDirection; // ���ϱ� ���ϴ� �߷� ����

    private void Start()
    {
        targetGravityDirection = -transform.position.normalized;
    }

    private void Update()
    {
        // ���� �߷� ������ ������
        Vector3 currentGravityDirection = Physics.gravity.normalized;

        // ��ǥ �߷� ����� ���� �߷� ���� ������ ȸ���� ����
        Quaternion targetRotation = Quaternion.FromToRotation(currentGravityDirection, targetGravityDirection) * transform.rotation;

        // �ε巯�� ȸ�� ������ ���� Slerp�� ����Ͽ� ����
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime);
    }
}