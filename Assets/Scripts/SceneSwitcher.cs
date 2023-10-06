using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneSwitcher : MonoBehaviour
{
    public string sceneNameToLoad;
    private float loadingStartTime;
    private float loadingTime = 7f;

    private IEnumerator LoadSceneAsyncCoroutine()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneNameToLoad);

        // �ε� ���� �ð� ���
        loadingStartTime = Time.time;

        // �ε��� �Ϸ�� ������ ���
        asyncLoad.allowSceneActivation = false; // �� ��ȯ ��������� ����

        while (!asyncLoad.isDone)
        {
            float progress = Mathf.Clamp01(asyncLoad.progress / 0.9f);
            Debug.Log("�ε� ��: " + (progress * 100) + "%");

            // �ε� �ð��� 5�� �̻� ����ϸ� �� ��ȯ ���
            if (Time.time - loadingStartTime >= loadingTime)
            {
                asyncLoad.allowSceneActivation = true; // �� ��ȯ ���
            }

            yield return null;
        }
    }

    // �� �ε��� �����ϴ� �޼���
    public void LoadSceneAsync()
    {
        StartCoroutine(LoadSceneAsyncCoroutine());
    }
}