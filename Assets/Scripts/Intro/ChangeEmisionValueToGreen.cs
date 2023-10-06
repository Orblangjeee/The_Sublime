using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeEmissionValueToGreen : MonoBehaviour
{
    public Material[] targetMaterials; // Material �迭
    public float minIntensity = 0f; //�⺻��
    public float maxIntensity = 0.5f; //��ȯ�� �� (min�� max�� ���̰��� 1�� ������ ��ȯ�� �� �ȵ� �� ����.)
    public float fadeInDuration = 1f;
    public float fadeOutDuration = 1f;
    private bool isRunning;

    void Start()
    {
        foreach (Material material in targetMaterials)
        {
            material.EnableKeyword("_EMISSION"); // Emission �ѱ�
            material.SetColor("_EmissionColor", Color.green * minIntensity); // Emission �ʱⰪ ����
        }
    }

    public void EmissionStart()
    {

        StartCoroutine(FadeInEmission()); // �ڷ�ƾ ����
    }

    public void EmissionExit()
    {

        StartCoroutine(FadeOutEmission()); // �ڷ�ƾ ����
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

    private void OnApplicationQuit() // ���� ����� Emission �ʱ�ȭ
    {
        foreach (Material material in targetMaterials)
        {
            material.SetColor("_EmissionColor", Color.green * minIntensity); // Emission �ʱⰪ ����
        }
    }
}