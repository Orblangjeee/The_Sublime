using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class Parabola : MonoBehaviour
{
    public Transform startPoint; // ������
    public Transform controlPoint; // �߰� ������
    public Transform endPoint;   // ����
    public int resolution = 10; // ��� �ػ� (����� ����Ʈ ��)

    LineRenderer lr;

    void Start()
    {
        lr = GetComponent<LineRenderer>();
        lr.positionCount = resolution;
    }

    void Update()
    {
        // ������, ����, �߰� �������� ��������
        Vector3 p0 = startPoint.position;
        Vector3 p1 = controlPoint.position;
        Vector3 p2 = endPoint.position;

        // ���� �������� �������� �׸��� ���� ����
        for (int i = 0; i < resolution; i++)
        {
            float t = i / (float)(resolution - 1);

            // ������ � ������ ���� ���
            float u = 1 - t;
            float tt = t * t;
            float uu = u * u;
            float uuu = uu * u;
            float ttu = tt * u;

            Vector3 point = uuu * p0;
            point += 3 * uu * t * p1;
            point += 3 * u * tt * p2;
            point += tt * t * p2;

            // ���� �������� ��ġ �Ҵ�
            lr.SetPosition(i, point);
        }

    }
}





