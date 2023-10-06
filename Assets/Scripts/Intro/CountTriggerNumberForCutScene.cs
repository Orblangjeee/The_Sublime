using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CountTriggerNumberForCutScene : MonoBehaviour
{
    private IntroProcess introProcess; //�ƾ� ����� ���� �ܺ� �ڵ� �ҷ�����
    private bool cutSceneExecuted;
    public DialogueManager dialogueManager;
    public int dialgueEndCounter;
    public int targetdialgueEndCounter = 2;


    void Start()
    {
        introProcess = FindObjectOfType<IntroProcess>(); //�ܺ� �ڵ� �ڵ����� �Ҵ�
        cutSceneExecuted = false;
    }


    void Update()
    {
        dialgueEndCounter = dialogueManager.dialgueEndCounter; //����� ��ȭ�� ������� ī����

        if (dialogueManager.dialgueEndCounter == targetdialgueEndCounter && cutSceneExecuted == false) //��ȭ�� n�� ����Ǹ�
        {
            introProcess.StartCutScene(); //�ƾ� ����
            cutSceneExecuted = true;
        }
    }
}
