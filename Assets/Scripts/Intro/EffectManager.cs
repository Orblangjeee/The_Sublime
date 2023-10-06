using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectManager : MonoBehaviour
{
    private bool audioTrigger = true;
    private int whoWasTalked = 0; //������ ���� (NPC 1:������ 2:����� 3:����)
    private AudioSource soundManager;
    [Header("Orig")]
    public Animator origAnimator;
    public ChangeEmissionValueToRed origEmision;
    public AudioClip[] origSound;
    [Header("Wizdom")]
    public Animator wizdomAnimator;
    public ChangeEmissionValueToGreen wizdomEmision;
    public AudioClip[] wizdomSound;
    [Header("Logic")]
    public Animator logicAnimator;
    public ChangeEmissionValueToBlue logicEmision;
    public ChangeEmissionValueToRed logicRedEmision;
    public AudioClip[] logicSound;
    [Header("Scene Effect")]
    public AudioClip cutSceneSound;
    public AudioClip sceneEndSound;
    private float origMinEmission;
    private float origMaxEmission;

    // Start is called before the first frame update
    void Start()
    {
        soundManager = GetComponent<AudioSource>();
        origMinEmission = origEmision.minIntensity;
        origMaxEmission = origEmision.maxIntensity; //�⺻�� ����
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.F1)) //�׽�Ʈ��
        {
            if (audioTrigger)
            {
                OrigEyeAnimation();
                OrigEyeEmission();
            }
            else
            {
                OrigEyeAnimationRevert();
                OrigEyeEmissionRevert();
            }
            PlayOrigSound();
        }
    }


    public void ConvertToOrig() //ȭ�ڰ� Orig�� ��ȯ�Ǿ��� �� ������ �Լ�
    {
        OrigEyeAnimation();
        OrigEyeEmission();
        PlayOrigSound();

        if (whoWasTalked == 2) //������ ���� ĳ���Ͱ� Wizdom�� ���
        {
            //Wizdom ��Ȱ��ȭ
            WizdomEyeAnimationRevert();
            WizdomEyeEmissionRevert();
        }

        if (whoWasTalked == 3) //������ ���� ĳ���Ͱ� Logic�� ���
        {
            //Logic ��Ȱ��ȭ
            LogicEyeAnimationRevert();
            LogicEyeEmissionRevert();
        }

        whoWasTalked = 1;
    }

    public void ConvertToWizdom() //ȭ�ڰ� Wizdom���� ��ȯ�Ǿ��� �� ������ �Լ�
    {
        WizdomEyeAnimation();
        WizdomEyeEmission();
        PlayWizdomSound();

        if (whoWasTalked == 1) //������ ���� ĳ���Ͱ� Orig�� ���
        {
            OrigEyeAnimationRevert();
            OrigEyeEmissionRevert();
        }
        if (whoWasTalked == 3) //������ ���� ĳ���Ͱ� Logic�� ���
        {
            LogicEyeAnimationRevert();
            LogicEyeEmissionRevert();
        }

        whoWasTalked = 2;
    }

    public void ConvertToLogic() //ȭ�ڰ� Logic���� ��ȯ�Ǿ��� �� ������ �Լ�
    {
        LogicEyeAnimation();
        LogicEyeEmission();
        PlayLogicSound();

        if (whoWasTalked == 2) //������ ���� ĳ���Ͱ� Wizdom�� ���
        {
            WizdomEyeAnimationRevert();
            WizdomEyeEmissionRevert();
        }

        if (whoWasTalked == 1) //������ ���� ĳ���Ͱ� Orig�� ���
        {
            OrigEyeAnimationRevert();
            OrigEyeEmissionRevert();
        }

        whoWasTalked = 3;
    }

    public void CutSceneEffect() //�ƾ� ����Ʈ
    {
        OrigEyeAnimationCutScene();
        PlayCutSceneSound();

        if (whoWasTalked == 1) //������ ���� ĳ���Ͱ� Orig�� ���
        {
            OrigEyeAnimationRevert();
        }
        else { OrigEyeEmission(); }

        if (whoWasTalked == 2) //������ ���� ĳ���Ͱ� Wizdom�� ���
        {
            WizdomEyeAnimationRevert();
            WizdomEyeEmissionRevert();
        }

        if (whoWasTalked == 3) //������ ���� ĳ���Ͱ� Logic�� ���
        {
            LogicEyeAnimationRevert();
            LogicEyeEmissionRevert();
        }

    }

    public void ConvertFromCutscene() //�ƾ� ���� �̺�Ʈ
    {
        OrigEyeAnimation();
        OrigEyeEmission();
        PlayOrigSound();
        whoWasTalked = 1;
    }

        public void SceneEndEffect() //�� ���� ����Ʈ
    {
        PlaySceneEndSound();


        if (whoWasTalked == 1) //������ ���� ĳ���Ͱ� Orig�� ���
        {
            OrigEyeAnimationRevert();
            OrigEyeEmissionRevert();
        }

        if (whoWasTalked == 2) //������ ���� ĳ���Ͱ� Wizdom�� ���
        {
            WizdomEyeAnimationRevert();
            WizdomEyeEmissionRevert();
        }

        if (whoWasTalked == 3) //������ ���� ĳ���Ͱ� Logic�� ���
        {
            LogicEyeAnimationRevert();
            LogicEyeEmissionRevert();
        }

    }

        public void StillTalkingVoiceSound()
    {
        if (whoWasTalked == 1) { PlayOrigSound(); }
        if (whoWasTalked == 2) { PlayWizdomSound(); }
        if (whoWasTalked == 3) { PlayLogicSound(); }
    }


    void OrigEyeAnimation() //������ �� �ִϸ��̼� ����
    {

        if (origAnimator != null) //�ִϸ����Ͱ� ������� �ʴٸ�
        {
            origAnimator.SetTrigger("Context"); //������ �� �ִϸ��̼� ����
        }

    }

    void OrigEyeAnimationRevert() //������ �� �ִϸ��̼� ���� ����
    {

        if (origAnimator != null) //�ִϸ����Ͱ� ������� �ʴٸ�
        {
            origAnimator.SetTrigger("Discontext"); //������ �� �ִϸ��̼� ���� ����
        }

    }

    void WizdomEyeAnimation() //����� �� �ִϸ��̼� ����
    {

        if (wizdomAnimator != null) //�ִϸ����Ͱ� ������� �ʴٸ�
        {
            wizdomAnimator.SetTrigger("Context"); //����� �� �ִϸ��̼� ����
        }

    }

    void WizdomEyeAnimationRevert() //����� �� �ִϸ��̼� ���� ����
    {

        if (wizdomAnimator != null) //�ִϸ����Ͱ� ������� �ʴٸ�
        {
            wizdomAnimator.SetTrigger("Discontext"); //����� �� �ִϸ��̼� ���� ����
        }

    }

    void LogicEyeAnimation() //���� �� �ִϸ��̼� ����
    {

        if (logicAnimator != null) //�ִϸ����Ͱ� ������� �ʴٸ�
        {
            logicAnimator.SetTrigger("Context"); //���� �� �ִϸ��̼� ����
        }

    }

    void LogicEyeAnimationRevert() //���� �� �ִϸ��̼� ���� ����
    {

        if (logicAnimator != null) //�ִϸ����Ͱ� ������� �ʴٸ�
        {
            logicAnimator.SetTrigger("Discontext"); //���� �� �ִϸ��̼� ���� ����
        }

    }

    void OrigEyeAnimationCutScene() //������ �� �ִϸ��̼� ���� ����
    {

        if (origAnimator != null) //�ִϸ����Ͱ� ������� �ʴٸ�
        {
            origAnimator.SetTrigger("CutScene"); //������ �� �ִϸ��̼� ���� ����
        }

    }

    void OrigEyeEmission() //������ �� Emission �� ����
    {
        if (origEmision != null) // origEmision�� ������� �ʴٸ�
        {
            origEmision.EmissionStart(); //Emission �� ����
        }
    }

    void OrigEyeEmissionRevert() //������ �� Emission �� ����
    {
        if (origEmision != null) // origEmision�� ������� �ʴٸ�
        {
            origEmision.EmissionExit();
        }
    }

    void WizdomEyeEmission() //����� �� Emission �� ����
    {
        if (wizdomEmision != null) // wizdomEmision�� ������� �ʴٸ�
        {
            wizdomEmision.EmissionStart(); //Emission �� ����
        }
    }

    void WizdomEyeEmissionRevert() //����� �� Emission �� ����
    {
        if (wizdomEmision != null) // WizdomEmision�� ������� �ʴٸ�
        {
            wizdomEmision.EmissionExit();
        }
    }

    void LogicEyeEmission() //���� �� Emission �� ����
    {
        if (logicEmision != null) // LogicEmision�� ������� �ʴٸ�
        {
            logicEmision.EmissionStart(); 
            logicRedEmision.EmissionStart();//Emission �� ����
        }
    }

    void LogicEyeEmissionRevert() //���� �� Emission �� ����
    { 
        if (logicEmision != null) // LogicEmision�� ������� �ʴٸ�
        {
            logicEmision.EmissionExit();
            logicRedEmision.EmissionExit();//Emission �� ����
        }
    }

    public void PlayOrigSound()
    {
        int audiointtrigger = audioTrigger ? 1 : 0; //bool�� int�� ��ȯ
        if (origSound != null && origSound.Length > 0) //�迭�� ������� ���� ���
        {
            soundManager.clip = origSound[audiointtrigger]; //���� ����� ��ȣ ���
            soundManager.Stop();
            soundManager.Play(); //�Ҹ� ���
            audioTrigger = !audioTrigger; //����� ��ȣ ��ȯ
        }
    }
    public void PlayWizdomSound()
    {
        int audiointtrigger = audioTrigger ? 1 : 0; //bool�� int�� ��ȯ
        if (wizdomSound != null && wizdomSound.Length > 0) //�迭�� ������� ���� ���
        {
            soundManager.clip = wizdomSound[audiointtrigger];
            soundManager.Stop();
            soundManager.Play();
            audioTrigger = !audioTrigger;//����� ��ȣ ��ȯ
        }
    }
    public void PlayLogicSound()
    {
        int audiointtrigger = audioTrigger ? 1 : 0; //bool�� int�� ��ȯ
        if (logicSound != null && logicSound.Length > 0) //�迭�� ������� ���� ���
        {
            soundManager.clip = logicSound[audiointtrigger];
            soundManager.Stop();
            soundManager.Play();
            audioTrigger = !audioTrigger;//����� ��ȣ ��ȯ
        }
    }

    public void PlayCutSceneSound()
    {
        if (cutSceneSound != null) //������� ���� ���
        {
            soundManager.clip = cutSceneSound;
            soundManager.Stop();
            soundManager.Play();
        }
    }

    public void PlaySceneEndSound()
    {
        if (sceneEndSound != null) //������� ���� ���
        {
            soundManager.clip = sceneEndSound;
            soundManager.Stop();
            soundManager.Play();
        }
    }
}