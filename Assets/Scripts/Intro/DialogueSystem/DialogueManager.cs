using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public bool isStart = false; // ��ȭ�� ���۵Ǿ����� ���θ� ��Ÿ���� ����
    public bool isFirst = false; // ù ��° ��ȭ���� ���θ� ��Ÿ���� ����
    public TextMeshProUGUI dialogueText; // �ؽ�Ʈ�� ǥ���� TextMeshProUGUI ��ü
    public DialogueTrigger[] triggers; // ��ȭ Ʈ���ŵ��� ���� �迭
    

    public Queue<string> sentences; // ��ȭ ������ �����ϴ� ť
    public int triggerNumber = 0; // ���� Ȱ��ȭ�� ��ȭ Ʈ������ �ε����� ��Ÿ���� ����
    public int dialgueEndCounter = 0;

    private IntroProcess introprocess; //�ƾ� ����� ���� �ܺ� �ڵ� �ҷ�����
    EffectManager effectManager; //�Ҹ� ����� ���� �ܺ� �ڵ� �ҷ�����

    void Start()
    {

        effectManager = FindObjectOfType<EffectManager>(); //�ܺ� �ڵ� �ڵ����� �Ҵ�
        
        sentences = new Queue<string>(); // ��ȭ ������ ������ ť�� �ʱ�ȭ

        // ù ��ȭ�� ���� ���۵��� �ʾҴٸ� ù ��° ��ȭ Ʈ���Ÿ� ������
        if (!isFirst)
        {
            triggers[triggerNumber].TriggerDialogue();
            isStart = true;
            isFirst = true;
        }

    }

    void Update()
    {
        // ����ڰ� A Ȥ�� B ��ư�� ������ ��ȭ�� ��� ����
        if (Input.GetButtonDown("Submit") || Input.GetButtonDown("Fire1") || Input.GetButtonDown("Fire2") || Input.GetButtonDown("Fire3"))
        {
            ContinueConversation();
        }
    }

    public void ContinueConversation()
    {
        Debug.Log("Continue"); // ����� �α� ���

        if (!isStart) // ��ȭ�� ���� ���۵��� �ʾҴٸ�
        {
            if (triggerNumber < triggers.Length) // ���� ��ȭ Ʈ���Ű� �����ִٸ�
            {
                triggers[triggerNumber].TriggerDialogue(); // ù ��° ��ȭ�� ������
                isStart = true;
            }
            else // ��� ��ȭ�� �����ٸ�
            {
                return; // �ƹ� �۾��� ���� �ʰ� ����
            }
        }
        else // �̹� ��ȭ�� ���۵Ǿ��ٸ�
        {
            DisplayNextSentence(); // ���� ��縦 ǥ����
        }
    }

    public void StartDialogue(Dialogue dialogue)
    {
        sentences.Clear(); // ��ȭ ������ �ʱ�ȭ

        // �־��� ��ȭ ��ü�� ��� ������ ť�� �߰���
        foreach (string sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }
        //Debug.Log(sentences.Count); // ť�� ����� ���� ���� ����׷� ���
        DisplayNextSentence(); // ���� ��縦 ǥ����
    }

    public void DisplayNextSentence()
    {

        if (sentences.Count == 0) // �� �̻� ǥ���� ��簡 ���ٸ�
        {
            EndDialogue(); // ��ȭ ���� ó���� ������
            dialgueEndCounter++; //��ȭ ���� ī��Ʈ
            return;
        }

        else
        {
            effectManager.StillTalkingVoiceSound();
        }

        string sentence = sentences.Dequeue(); // ť���� ���� ��縦 ������
        dialogueText.text = sentence; // ��ȭ �ؽ�Ʈ�� ������Ʈ
        //StopAllCoroutines();
        //StartCoroutine(TypeSentence(sentence));
    }

    IEnumerator TypeSentence(string sentence)
    {
        dialogueText.text = sentence; // ��ȭ �ؽ�Ʈ�� ������Ʈ
        yield return null;
    }

    public void EndDialogue()
    {
        isStart = false; // ��ȭ�� �������� ��Ÿ���� ������ false�� ����
        triggerNumber++; // ���� ��ȭ Ʈ���ŷ� �̵�
        if (triggerNumber <= triggers.Length) // ���� ��ȭ Ʈ���Ű� �����ִٸ�
        {
            triggers[triggerNumber - 1].NextDialogue(); // �� NPC�� ��Ȱ��ȭ�ϰ�
            ContinueConversation(); // ��ȭ�� �����
        }

        gameObject.SetActive(false); // ��ȭ �Ŵ��� ���� ������Ʈ�� ��Ȱ��ȭ��
        

    }
}
