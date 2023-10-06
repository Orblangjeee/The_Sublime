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
    public float dialogueStartDelayTime = 27f; //���̾�α� ���� ���� �ð�
    public float dialogueContinueDelayTime = 25f; //���̾�α� ���� ���� �ð�
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

    IEnumerator StartConversationCoroutine() //���� ���۽� ������ �ڷ�ƾ
    {
        yield return new WaitForSeconds(dialogueStartDelayTime); //��� ��

        StartConversation(); // StartConversation() �޼��� ����
    }

    IEnumerator ContinueConversationCoroutine() //�ƾ� ��ȯ�� ������ �ڷ�ƾ
    {
        yield return new WaitForSeconds(dialogueContinueDelayTime); //��� ��

        ContinueConverstation(); // StartConversation() �޼��� ����
    }

    void StartConversation() //ù ��ȭ ����
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
        if (cutSceneExecuted == false) //ó�� ȣ��� �ƾ� ���
        {
            Debug.Log("CutScene!");
            ooparts.SetActive(true);
            effectManager.CutSceneEffect();
            StartCoroutine(ContinueConversationCoroutine());
            cutSceneExecuted = true;
        }

        else  //�ι�° ���� ȣ��� ��Ʈ�ξ� ����
        {
            Debug.Log("Scene End!");
            endSceneUi.SetActive(true);
            effectManager.SceneEndEffect();
            sceneSwitcher.LoadSceneAsync();
        }
        
        
    }

    void ContinueConverstation() //�ƾ� �� ��ȭ �簳
    {
        origText2nd.SetActive(true);
        effectManager.ConvertFromCutscene();
    }
}