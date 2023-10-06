using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeEmissionValueToRedCore : MonoBehaviour
{
    public Material[] targetMaterials; // 머테리얼 배열
    public float minIntensity = -1.6f; //기본값
    public float maxIntensity = -0.5f; //변환할 값 (min과 max의 사이값은 1을 넘으면 전환이 잘 안될 수 있음.)
    public float fadeInDuration = 1f;
    public float fadeOutDuration = 0.1f;
    private float newIntensity;
    private bool isRunning;

    void Start()
    {
        //targetMaterials 배열에 있는 모든 Material 요소들을 material 변수에 하나씩 할당
        foreach (Material material in targetMaterials)
        {
            // 게임 초기화시 코드로 머테리얼 값 변경되지 않는 문제 수정
            material.EnableKeyword("_EMISSION"); // Emission 켜기
            material.SetColor("_EmissionColor", Color.red * minIntensity); //Emission 초기값 설정
            EmissionStart();
        }

    }

    public void EmissionStart()
    {
        StartCoroutine(FadeInEmission()); // 코루틴 시작
    }

    public void EmissionExit()
    {
        StartCoroutine(FadeOutEmission()); // 코루틴 시작
    }

    public IEnumerator FadeInEmission()
    {
        isRunning = true;

        float startTime = Time.time;
        while (isRunning)
        {
            float timeSinceStart = Time.time - startTime; // 시간 재기
            float t = Mathf.Clamp01(timeSinceStart / fadeInDuration); // 0에서 1 사이의 값으로 보간

            float newIntensity = Mathf.Lerp(minIntensity, maxIntensity, t);

            foreach (Material material in targetMaterials)
            {
                material.SetColor("_EmissionColor", Color.red * newIntensity); // Intensity값 조금씩 증가
            }

            if (t >= 1f)
            {
                isRunning = false;
            }
            
            yield return null;
        }

    }

    public IEnumerator FadeOutEmission()
    {
        isRunning = true;

        float startTime = Time.time;
        while (isRunning)
        {
            float timeSinceStart = Time.time - startTime; // 시간 재기
            float t = Mathf.Clamp01(timeSinceStart / fadeOutDuration); // 0에서 1 사이의 값으로 보간

            newIntensity = Mathf.Lerp(maxIntensity, minIntensity, t);

            foreach (Material material in targetMaterials)
            {
                material.SetColor("_EmissionColor", Color.red * newIntensity); // Intensity값 조금씩 감소
            }

            if (t >= 1f)
            {
                isRunning = false;
            }

            yield return null;
        }

    }

    private void OnApplicationQuit() //게임 종료시 Emision 초기화
    {
        foreach (Material material in targetMaterials)
        {
            material.SetColor("_EmissionColor", Color.red * minIntensity);//Emission 초기값 설정
        }
    }
}
