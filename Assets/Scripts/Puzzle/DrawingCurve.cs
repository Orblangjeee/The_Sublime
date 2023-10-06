using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class Parabola : MonoBehaviour
{
    public Transform startPoint; // 시작점
    public Transform controlPoint; // 중간 제어점
    public Transform endPoint;   // 끝점
    public int resolution = 10; // 곡선의 해상도 (고려할 포인트 수)

    LineRenderer lr;

    void Start()
    {
        lr = GetComponent<LineRenderer>();
        lr.positionCount = resolution;
    }

    void Update()
    {
        // 시작점, 끝점, 중간 제어점을 가져오기
        Vector3 p0 = startPoint.position;
        Vector3 p1 = controlPoint.position;
        Vector3 p2 = endPoint.position;

        // 라인 렌더러에 포물선을 그리기 위한 루프
        for (int i = 0; i < resolution; i++)
        {
            float t = i / (float)(resolution - 1);

            // 베지에 곡선 포물선 공식 사용
            float u = 1 - t;
            float tt = t * t;
            float uu = u * u;
            float uuu = uu * u;
            float ttu = tt * u;

            Vector3 point = uuu * p0;
            point += 3 * uu * t * p1;
            point += 3 * u * tt * p2;
            point += tt * t * p2;

            // 라인 렌더러에 위치 할당
            lr.SetPosition(i, point);
        }

    }
}





