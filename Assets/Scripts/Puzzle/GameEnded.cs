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
        // delay �ð� ���� ����մϴ�.
        yield return new WaitForSeconds(delay);

        // n�� �Ŀ� ����� �ڵ带 �����մϴ�.
        backgroundAnim.SetTrigger("isEnded");
        soundManager.PlayEndSceneSound();
        endingCredit.SetActive(true);
    }


}
