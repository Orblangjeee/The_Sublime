using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public Dialogue dialogue;
    public GameObject nextDialogue;
    private EffectManager effectManager;
    


    public void TriggerDialogue()
    {
        GetComponent<DialogueManager>().StartDialogue(dialogue);
        effectManager = FindObjectOfType<EffectManager>();

    }

    public void NextDialogue()
    {

        if (nextDialogue != null)
        {

            // ���� ������Ʈ�� �±װ� Orig�� ���
            if (nextDialogue.CompareTag("Orig"))
            {
                Debug.Log("Orig is Talking");
                effectManager.ConvertToOrig();
            }

            // ���� ������Ʈ�� �±װ� Wisdom�� ���
            if (nextDialogue.CompareTag("Wizdom"))
            {
                Debug.Log("Wizdom is Talking");
                effectManager.ConvertToWizdom();
            }

            // ���� ������Ʈ�� �±װ� Logic�� ���
            if (nextDialogue.CompareTag("Logic"))
            {
                Debug.Log("Logic is Talking");
                effectManager.ConvertToLogic();
            }

            nextDialogue.SetActive(true);

        }
    }
}
