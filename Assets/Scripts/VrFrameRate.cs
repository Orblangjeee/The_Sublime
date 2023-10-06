using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class VrFrameRate : MonoBehaviour
{
    public TMP_Text frameRateText; // TextMeshProUGUI 요소를 여기에 할당합니다.

    private void Start()
    {
        if (frameRateText == null)
        {
            Debug.LogWarning("FrameRateDisplayTMP: TextMeshProUGUI 요소가 할당되지 않았습니다. 이 스크립트를 비활성화합니다.");
            enabled = false;
            return;
        }

        StartCoroutine(UpdateFrameRate());
    }

    private System.Collections.IEnumerator UpdateFrameRate()
    {
        while (true)
        {
            int frameRate = Mathf.RoundToInt(1f / Time.unscaledDeltaTime);
            frameRateText.text = "FPS : " + frameRate.ToString();

            yield return new WaitForSeconds(0.1f);
        }
    }
}