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

            // 게임 오브젝트의 태그가 Orig일 경우
            if (nextDialogue.CompareTag("Orig"))
            {
                Debug.Log("Orig is Talking");
                effectManager.ConvertToOrig();
            }

            // 게임 오브젝트의 태그가 Wisdom일 경우
            if (nextDialogue.CompareTag("Wizdom"))
            {
                Debug.Log("Wizdom is Talking");
                effectManager.ConvertToWizdom();
            }

            // 게임 오브젝트의 태그가 Logic일 경우
            if (nextDialogue.CompareTag("Logic"))
            {
                Debug.Log("Logic is Talking");
                effectManager.ConvertToLogic();
            }

            nextDialogue.SetActive(true);

        }
    }
}
