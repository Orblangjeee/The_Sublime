using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CountTriggerNumberForCutScene : MonoBehaviour
{
    private IntroProcess introProcess; //컷씬 재생을 위해 외부 코드 불러오기
    private bool cutSceneExecuted;
    public DialogueManager dialogueManager;
    public int dialgueEndCounter;
    public int targetdialgueEndCounter = 2;


    void Start()
    {
        introProcess = FindObjectOfType<IntroProcess>(); //외부 코드 자동으로 할당
        cutSceneExecuted = false;
    }


    void Update()
    {
        dialgueEndCounter = dialogueManager.dialgueEndCounter; //종료된 대화가 몇번인지 카운팅

        if (dialogueManager.dialgueEndCounter == targetdialgueEndCounter && cutSceneExecuted == false) //대화가 n번 종료되면
        {
            introProcess.StartCutScene(); //컷씬 실행
            cutSceneExecuted = true;
        }
    }
}
