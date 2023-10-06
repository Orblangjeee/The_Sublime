using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroProcess : MonoBehaviour
{
    public GameObject logicText;
    public GameObject wizdomText;
    public GameObject origText;
    public GameObject logicText2nd;
    public GameObject wizdomText2nd;
    public GameObject origText2nd;
    public float dialogueStartDelayTime = 27f; //다이얼로그 시작 지연 시간
    public float dialogueContinueDelayTime = 25f; //다이얼로그 시작 지연 시간
    public GameObject ooparts;
    public GameObject tutorialUI;
    public GameObject endSceneUi;
    private EffectManager effectManager;
    private SceneSwitcher sceneSwitcher;
    private bool cutSceneExecuted;
    

    void Start()
    {
        logicText.SetActive(false);
        wizdomText.SetActive(false);
        origText.SetActive(false);
        logicText2nd.SetActive(false);
        wizdomText2nd.SetActive(false);
        origText2nd.SetActive(false);
        tutorialUI.SetActive(false);
        cutSceneExecuted = false;
        effectManager = FindObjectOfType<EffectManager>();
        sceneSwitcher = FindObjectOfType<SceneSwitcher>();
        StartCoroutine(StartConversationCoroutine());
    }

    IEnumerator StartConversationCoroutine() //게임 시작시 실행할 코루틴
    {
        yield return new WaitForSeconds(dialogueStartDelayTime); //대기 후

        StartConversation(); // StartConversation() 메서드 실행
    }

    IEnumerator ContinueConversationCoroutine() //컷씬 전환시 실행할 코루틴
    {
        yield return new WaitForSeconds(dialogueContinueDelayTime); //대기 후

        ContinueConverstation(); // StartConversation() 메서드 실행
    }

    void StartConversation() //첫 대화 시작
    {
        effectManager.ConvertToLogic();
        logicText.SetActive(true);
        tutorialUI.SetActive(true);
        if (tutorialUI != null)
        {
            tutorialUI.SetActive(true);
        }
    }

    public void StartCutScene()
    {
        if (cutSceneExecuted == false) //처음 호출시 컷씬 재생
        {
            Debug.Log("CutScene!");
            ooparts.SetActive(true);
            effectManager.CutSceneEffect();
            StartCoroutine(ContinueConversationCoroutine());
            cutSceneExecuted = true;
        }

        else  //두번째 이후 호출시 인트로씬 종료
        {
            Debug.Log("Scene End!");
            endSceneUi.SetActive(true);
            effectManager.SceneEndEffect();
            sceneSwitcher.LoadSceneAsync();
        }
        
        
    }

    void ContinueConverstation() //컷씬 후 대화 재개
    {
        origText2nd.SetActive(true);
        effectManager.ConvertFromCutscene();
    }
}