using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEnded : MonoBehaviour
{
    public Animator backgroundAnim;
    public GameObject endingCredit;
    public PuzzleSoundManager soundManager;
    public float endingSceneDelay = 6f;
    
    public void GameisEnded()
    {
        StartCoroutine(ActivateEndingAfterDelay(6f));
    }

    // Update is called once per frame
    IEnumerator ActivateEndingAfterDelay(float delay)
    {
        // delay 시간 동안 대기합니다.
        yield return new WaitForSeconds(delay);

        // n초 후에 실행될 코드를 실행합니다.
        backgroundAnim.SetTrigger("isEnded");
        soundManager.PlayEndSceneSound();
        endingCredit.SetActive(true);
    }


}
