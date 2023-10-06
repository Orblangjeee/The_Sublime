using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public bool isStart = false; // 대화가 시작되었는지 여부를 나타내는 변수
    public bool isFirst = false; // 첫 번째 대화인지 여부를 나타내는 변수
    public TextMeshProUGUI dialogueText; // 텍스트를 표시할 TextMeshProUGUI 객체
    public DialogueTrigger[] triggers; // 대화 트리거들을 담을 배열
    

    public Queue<string> sentences; // 대화 문장을 저장하는 큐
    public int triggerNumber = 0; // 현재 활성화된 대화 트리거의 인덱스를 나타내는 변수
    public int dialgueEndCounter = 0;

    private IntroProcess introprocess; //컷씬 재생을 위해 외부 코드 불러오기
    EffectManager effectManager; //소리 재생을 위해 외부 코드 불러오기

    void Start()
    {

        effectManager = FindObjectOfType<EffectManager>(); //외부 코드 자동으로 할당
        
        sentences = new Queue<string>(); // 대화 문장을 저장할 큐를 초기화

        // 첫 대화가 아직 시작되지 않았다면 첫 번째 대화 트리거를 시작함
        if (!isFirst)
        {
            triggers[triggerNumber].TriggerDialogue();
            isStart = true;
            isFirst = true;
        }

    }

    void Update()
    {
        // 사용자가 A 혹은 B 버튼을 누르면 대화를 계속 진행
        if (Input.GetButtonDown("Submit") || Input.GetButtonDown("Fire1") || Input.GetButtonDown("Fire2") || Input.GetButtonDown("Fire3"))
        {
            ContinueConversation();
        }
    }

    public void ContinueConversation()
    {
        Debug.Log("Continue"); // 디버그 로그 출력

        if (!isStart) // 대화가 아직 시작되지 않았다면
        {
            if (triggerNumber < triggers.Length) // 아직 대화 트리거가 남아있다면
            {
                triggers[triggerNumber].TriggerDialogue(); // 첫 번째 대화를 시작함
                isStart = true;
            }
            else // 모든 대화가 끝났다면
            {
                return; // 아무 작업도 하지 않고 종료
            }
        }
        else // 이미 대화가 시작되었다면
        {
            DisplayNextSentence(); // 다음 대사를 표시함
        }
    }

    public void StartDialogue(Dialogue dialogue)
    {
        sentences.Clear(); // 대화 문장을 초기화

        // 주어진 대화 객체의 모든 문장을 큐에 추가함
        foreach (string sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }
        //Debug.Log(sentences.Count); // 큐에 저장된 문장 수를 디버그로 출력
        DisplayNextSentence(); // 다음 대사를 표시함
    }

    public void DisplayNextSentence()
    {

        if (sentences.Count == 0) // 더 이상 표시할 대사가 없다면
        {
            EndDialogue(); // 대화 종료 처리를 수행함
            dialgueEndCounter++; //대화 종료 카운트
            return;
        }

        else
        {
            effectManager.StillTalkingVoiceSound();
        }

        string sentence = sentences.Dequeue(); // 큐에서 다음 대사를 가져옴
        dialogueText.text = sentence; // 대화 텍스트를 업데이트
        //StopAllCoroutines();
        //StartCoroutine(TypeSentence(sentence));
    }

    IEnumerator TypeSentence(string sentence)
    {
        dialogueText.text = sentence; // 대화 텍스트를 업데이트
        yield return null;
    }

    public void EndDialogue()
    {
        isStart = false; // 대화가 끝났음을 나타내는 변수를 false로 설정
        triggerNumber++; // 다음 대화 트리거로 이동
        if (triggerNumber <= triggers.Length) // 아직 대화 트리거가 남아있다면
        {
            triggers[triggerNumber - 1].NextDialogue(); // 현 NPC를 재활성화하고
            ContinueConversation(); // 대화를 계속함
        }

        gameObject.SetActive(false); // 대화 매니저 게임 오브젝트를 비활성화함
        

    }
}
