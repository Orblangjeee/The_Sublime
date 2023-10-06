using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeEmissionValueToBlue : MonoBehaviour
{
    public Material[] targetMaterials; // ���׸��� �迭
    public float minIntensity = 0f; //�⺻��
    public float maxIntensity = 0.5f; //��ȯ�� �� (min�� max�� ���̰��� 1�� ������ ��ȯ�� �� �ȵ� �� ����.)
    public float fadeInDuration = 1f;
    public float fadeOutDuration = 0.5f;
    private float newIntensity;
    private bool isRunning;

    void Start()
    {
        //targetMaterials �迭�� �ִ� ��� Material ��ҵ��� material ������ �ϳ��� �Ҵ�
        foreach (Material material in targetMaterials)
        {
            // ���� �ʱ�ȭ�� ���׸��� �� ������� �ʴ� ���� ����
            material.EnableKeyword("_EMISSION"); // Emission �ѱ�
            material.SetColor("_EmissionColor", Color.cyan * minIntensity); //Emission �ʱⰪ ����
            //StartCoroutine(FadeInEmission());
        }

    }

    public void EmissionStart()
    {
        foreach (Material material in targetMaterials)
        {
            material.EnableKeyword("_EMISSION"); // Emission �ѱ�

        }
        StartCoroutine(FadeInEmission()); // �ڷ�ƾ ����
    }

    public void EmissionExit()
    {
        //foreach (Material material in targetMaterials)
        //{
        //    material.DisableKeyword("_EMISSION"); // Emission �ѱ�

       // }
        StartCoroutine(FadeOutEmission()); // �ڷ�ƾ ����
    }

    public IEnumerator FadeInEmission()
    {
        isRunning = true;

        float startTime = Time.time;
        while (isRunning)
        {
            float timeSinceStart = Time.time - startTime; // �ð� ���
            float t = Mathf.Clamp01(timeSinceStart / fadeInDuration); // 0���� 1 ������ ������ ����

            float newIntensity = Mathf.Lerp(minIntensity, maxIntensity, t);

            foreach (Material material in targetMaterials)
            {
                material.SetColor("_EmissionColor", Color.cyan * newIntensity); // Intensity�� ���ݾ� ����
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
            float timeSinceStart = Time.time - startTime; // �ð� ���
            float t = Mathf.Clamp01(timeSinceStart / fadeOutDuration); // 0���� 1 ������ ������ ����

            newIntensity = Mathf.Lerp(maxIntensity, minIntensity, t);

            foreach (Material material in targetMaterials)
            {
                material.SetColor("_EmissionColor", Color.cyan * newIntensity); // Intensity�� ���ݾ� ����
            }

            if (t >= 1f)
            {
                isRunning = false;
            }

            yield return null;
        }
    }

    private void OnApplicationQuit() //���� ����� Emission �ʱ�ȭ
    {
        foreach (Material material in targetMaterials)
        {
            material.SetColor("_EmissionColor", Color.cyan * minIntensity);//Emission �ʱⰪ ����
        }
    }
}
