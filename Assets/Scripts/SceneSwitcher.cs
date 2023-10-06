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

        // 로딩 시작 시간 기록
        loadingStartTime = Time.time;

        // 로딩이 완료될 때까지 대기
        asyncLoad.allowSceneActivation = false; // 씬 전환 비허용으로 설정

        while (!asyncLoad.isDone)
        {
            float progress = Mathf.Clamp01(asyncLoad.progress / 0.9f);
            Debug.Log("로딩 중: " + (progress * 100) + "%");

            // 로딩 시간이 5초 이상 경과하면 씬 전환 허용
            if (Time.time - loadingStartTime >= loadingTime)
            {
                asyncLoad.allowSceneActivation = true; // 씬 전환 허용
            }

            yield return null;
        }
    }

    // 씬 로딩을 시작하는 메서드
    public void LoadSceneAsync()
    {
        StartCoroutine(LoadSceneAsyncCoroutine());
    }
}