using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Fade : MonoBehaviour
{
    public static Fade instance;
    public Image fadeImage;
    private Color fadeColor;
    public float durationTime = 3.0f;

    private void Awake()
    {
        if (instance == null) { instance = this; }
    }

    void Start()
    {
        fadeImage = fadeImage.GetComponent<Image>();
        fadeColor = fadeImage.color;
        
    }


    public IEnumerator FadeCoroutine(bool isFade)
    {
        
        float currentTime = 0f;
       
        while (true)
        {
            yield return null;
            currentTime += Time.unscaledDeltaTime;
            
            if (isFade)
            {
                fadeColor.a = Mathf.Clamp01(1 - currentTime / durationTime);
                fadeImage.color = fadeColor;
                if (fadeColor.a <= 0f)
                {
                    Debug.Log("FadeIN");
                    yield break;
                }
            } else
            {
                fadeColor.a = Mathf.Clamp01(currentTime / durationTime);
                fadeImage.color = fadeColor;
                if (fadeColor.a >= 1f)
                {
                    Debug.Log("FadeOut");
                    yield break;
                }
            }
        }
        
    }

    public void FadeIn()
    {
        fadeColor.a = 1f;
        fadeImage.color = fadeColor;
        StartCoroutine(FadeCoroutine(true));
    }

    public void FadeOut()
    {
        fadeColor.a = 0f;
        fadeImage.color = fadeColor;
        StartCoroutine(FadeCoroutine(false));
    }


}
