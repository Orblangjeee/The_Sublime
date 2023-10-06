using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroTutorialUIRotationFix : MonoBehaviour
{
    private Quaternion initialRotation;
    public float rotSpeed = 2.0f; // ȸ�� �ӵ� ����

    private void Start()
    {
        // UI ����� �ʱ� ȸ������ �����մϴ�.
        initialRotation = transform.rotation;
    }

    private void LateUpdate()
    {
        // UI ����� X, Z �� ȸ������ �����մϴ�.
        Vector3 eulerRotation = transform.rotation.eulerAngles;
        eulerRotation.x = 0f; // X �� ȸ���� 0���� ����
        eulerRotation.z = 0f; // Z �� ȸ���� 0���� ����
        transform.rotation = Quaternion.Euler(eulerRotation);

        //---------------���̾�α��� ȸ������ �÷��̾� Y�� ������ ���󰡵��� ����� ------------------
        if (Camera.main != null) // ���� ���� ī�޶� �ִ��� Ȯ��
        {
            Transform mainCamera = Camera.main.transform; // ���� ���� ī�޶� ����

            // ���� ī�޶��� forward ���Ϳ� y�� ������ ���
            Vector3 forwardWithYRotation = Vector3.ProjectOnPlane(mainCamera.forward, Vector3.up).normalized;
            Quaternion yRotation = Quaternion.LookRotation(forwardWithYRotation, Vector3.up);

            // ���� ������Ʈ�� ȸ���� y�� �������� ����
            transform.rotation = Quaternion.Lerp(transform.rotation, yRotation, rotSpeed * Time.deltaTime);
        }
    }
}