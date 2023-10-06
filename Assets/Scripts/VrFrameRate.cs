using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class VrFrameRate : MonoBehaviour
{
    public TMP_Text frameRateText; // TextMeshProUGUI ��Ҹ� ���⿡ �Ҵ��մϴ�.

    private void Start()
    {
        if (frameRateText == null)
        {
            Debug.LogWarning("FrameRateDisplayTMP: TextMeshProUGUI ��Ұ� �Ҵ���� �ʾҽ��ϴ�. �� ��ũ��Ʈ�� ��Ȱ��ȭ�մϴ�.");
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