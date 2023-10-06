using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeEmissionValueToGreen : MonoBehaviour
{
    public Material[] targetMaterials; // Material 배열
    public float minIntensity = 0f; //기본값
    public float maxIntensity = 0.5f; //변환할 값 (min과 max의 사이값은 1을 넘으면 전환이 잘 안될 수 있음.)
    public float fadeInDuration = 1f;
    public float fadeOutDuration = 1f;
    private bool isRunning;

    void Start()
    {
        foreach (Material material in targetMaterials)
        {
            material.EnableKeyword("_EMISSION"); // Emission 켜기
            material.SetColor("_EmissionColor", Color.green * minIntensity); // Emission 초기값 설정
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
            float timeSinceStart = Time.time - startTime;
            float t = Mathf.Clamp01(timeSinceStart / fadeInDuration);

            float newIntensity = Mathf.Lerp(minIntensity, maxIntensity, t);

            foreach (Material material in targetMaterials)
            {

                material.SetColor("_EmissionColor", Color.green * newIntensity);
            }

            if (t >= 1f)
            {
                isRunning = false;
                FadeOutEmission();
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
            float timeSinceStart = Time.time - startTime;
            float t = Mathf.Clamp01(timeSinceStart / fadeOutDuration);

            float newIntensity = Mathf.Lerp(maxIntensity, minIntensity, t);

            foreach (Material material in targetMaterials)
            {
                material.SetColor("_EmissionColor", Color.green * newIntensity);
            }

            if (t >= 1f)
            {
                isRunning = false;
            }

            yield return null;
        }
    }

    private void OnApplicationQuit() // 게임 종료시 Emission 초기화
    {
        foreach (Material material in targetMaterials)
        {
            material.SetColor("_EmissionColor", Color.green * minIntensity); // Emission 초기값 설정
        }
    }
}