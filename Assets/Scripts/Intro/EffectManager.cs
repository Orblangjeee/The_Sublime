using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectManager : MonoBehaviour
{
    private bool audioTrigger = true;
    private int whoWasTalked = 0; //직전에 말한 (NPC 1:오리그 2:위즈덤 3:로직)
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
        origMaxEmission = origEmision.maxIntensity; //기본값 저장
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.F1)) //테스트용
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


    public void ConvertToOrig() //화자가 Orig로 전환되었을 때 실행할 함수
    {
        OrigEyeAnimation();
        OrigEyeEmission();
        PlayOrigSound();

        if (whoWasTalked == 2) //직전에 말한 캐릭터가 Wizdom일 경우
        {
            //Wizdom 비활성화
            WizdomEyeAnimationRevert();
            WizdomEyeEmissionRevert();
        }

        if (whoWasTalked == 3) //직전에 말한 캐릭터가 Logic일 경우
        {
            //Logic 비활성화
            LogicEyeAnimationRevert();
            LogicEyeEmissionRevert();
        }

        whoWasTalked = 1;
    }

    public void ConvertToWizdom() //화자가 Wizdom으로 전환되었을 때 실행할 함수
    {
        WizdomEyeAnimation();
        WizdomEyeEmission();
        PlayWizdomSound();

        if (whoWasTalked == 1) //직전에 말한 캐릭터가 Orig일 경우
        {
            OrigEyeAnimationRevert();
            OrigEyeEmissionRevert();
        }
        if (whoWasTalked == 3) //직전에 말한 캐릭터가 Logic일 경우
        {
            LogicEyeAnimationRevert();
            LogicEyeEmissionRevert();
        }

        whoWasTalked = 2;
    }

    public void ConvertToLogic() //화자가 Logic으로 전환되었을 때 실행할 함수
    {
        LogicEyeAnimation();
        LogicEyeEmission();
        PlayLogicSound();

        if (whoWasTalked == 2) //직전에 말한 캐릭터가 Wizdom일 경우
        {
            WizdomEyeAnimationRevert();
            WizdomEyeEmissionRevert();
        }

        if (whoWasTalked == 1) //직전에 말한 캐릭터가 Orig일 경우
        {
            OrigEyeAnimationRevert();
            OrigEyeEmissionRevert();
        }

        whoWasTalked = 3;
    }

    public void CutSceneEffect() //컷씬 이펙트
    {
        OrigEyeAnimationCutScene();
        PlayCutSceneSound();

        if (whoWasTalked == 1) //직전에 말한 캐릭터가 Orig일 경우
        {
            OrigEyeAnimationRevert();
        }
        else { OrigEyeEmission(); }

        if (whoWasTalked == 2) //직전에 말한 캐릭터가 Wizdom일 경우
        {
            WizdomEyeAnimationRevert();
            WizdomEyeEmissionRevert();
        }

        if (whoWasTalked == 3) //직전에 말한 캐릭터가 Logic일 경우
        {
            LogicEyeAnimationRevert();
            LogicEyeEmissionRevert();
        }

    }

    public void ConvertFromCutscene() //컷씬 직후 이벤트
    {
        OrigEyeAnimation();
        OrigEyeEmission();
        PlayOrigSound();
        whoWasTalked = 1;
    }

        public void SceneEndEffect() //씬 종료 이펙트
    {
        PlaySceneEndSound();


        if (whoWasTalked == 1) //직전에 말한 캐릭터가 Orig일 경우
        {
            OrigEyeAnimationRevert();
            OrigEyeEmissionRevert();
        }

        if (whoWasTalked == 2) //직전에 말한 캐릭터가 Wizdom일 경우
        {
            WizdomEyeAnimationRevert();
            WizdomEyeEmissionRevert();
        }

        if (whoWasTalked == 3) //직전에 말한 캐릭터가 Logic일 경우
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


    void OrigEyeAnimation() //오리그 눈 애니메이션 실행
    {

        if (origAnimator != null) //애니메이터가 비어있지 않다면
        {
            origAnimator.SetTrigger("Context"); //오리그 눈 애니메이션 실행
        }

    }

    void OrigEyeAnimationRevert() //오리그 눈 애니메이션 실행 해제
    {

        if (origAnimator != null) //애니메이터가 비어있지 않다면
        {
            origAnimator.SetTrigger("Discontext"); //오리그 눈 애니메이션 실행 해제
        }

    }

    void WizdomEyeAnimation() //위즈덤 눈 애니메이션 실행
    {

        if (wizdomAnimator != null) //애니메이터가 비어있지 않다면
        {
            wizdomAnimator.SetTrigger("Context"); //위즈덤 눈 애니메이션 실행
        }

    }

    void WizdomEyeAnimationRevert() //위즈덤 눈 애니메이션 실행 해제
    {

        if (wizdomAnimator != null) //애니메이터가 비어있지 않다면
        {
            wizdomAnimator.SetTrigger("Discontext"); //위즈덤 눈 애니메이션 실행 해제
        }

    }

    void LogicEyeAnimation() //로직 눈 애니메이션 실행
    {

        if (logicAnimator != null) //애니메이터가 비어있지 않다면
        {
            logicAnimator.SetTrigger("Context"); //로직 눈 애니메이션 실행
        }

    }

    void LogicEyeAnimationRevert() //로직 눈 애니메이션 실행 해제
    {

        if (logicAnimator != null) //애니메이터가 비어있지 않다면
        {
            logicAnimator.SetTrigger("Discontext"); //로직 눈 애니메이션 실행 해제
        }

    }

    void OrigEyeAnimationCutScene() //오리그 눈 애니메이션 실행 해제
    {

        if (origAnimator != null) //애니메이터가 비어있지 않다면
        {
            origAnimator.SetTrigger("CutScene"); //오리그 눈 애니메이션 실행 해제
        }

    }

    void OrigEyeEmission() //오리그 눈 Emission 값 변경
    {
        if (origEmision != null) // origEmision이 비어있지 않다면
        {
            origEmision.EmissionStart(); //Emission 값 변경
        }
    }

    void OrigEyeEmissionRevert() //오리그 눈 Emission 값 복구
    {
        if (origEmision != null) // origEmision이 비어있지 않다면
        {
            origEmision.EmissionExit();
        }
    }

    void WizdomEyeEmission() //위즈덤 눈 Emission 값 변경
    {
        if (wizdomEmision != null) // wizdomEmision이 비어있지 않다면
        {
            wizdomEmision.EmissionStart(); //Emission 값 변경
        }
    }

    void WizdomEyeEmissionRevert() //위즈덤 눈 Emission 값 복구
    {
        if (wizdomEmision != null) // WizdomEmision이 비어있지 않다면
        {
            wizdomEmision.EmissionExit();
        }
    }

    void LogicEyeEmission() //로직 눈 Emission 값 변경
    {
        if (logicEmision != null) // LogicEmision이 비어있지 않다면
        {
            logicEmision.EmissionStart(); 
            logicRedEmision.EmissionStart();//Emission 값 변경
        }
    }

    void LogicEyeEmissionRevert() //로직 눈 Emission 값 복구
    { 
        if (logicEmision != null) // LogicEmision이 비어있지 않다면
        {
            logicEmision.EmissionExit();
            logicRedEmision.EmissionExit();//Emission 값 변경
        }
    }

    public void PlayOrigSound()
    {
        int audiointtrigger = audioTrigger ? 1 : 0; //bool을 int로 변환
        if (origSound != null && origSound.Length > 0) //배열이 비어있지 않을 경우
        {
            soundManager.clip = origSound[audiointtrigger]; //현재 오디오 번호 재생
            soundManager.Stop();
            soundManager.Play(); //소리 재생
            audioTrigger = !audioTrigger; //오디오 번호 전환
        }
    }
    public void PlayWizdomSound()
    {
        int audiointtrigger = audioTrigger ? 1 : 0; //bool을 int로 변환
        if (wizdomSound != null && wizdomSound.Length > 0) //배열이 비어있지 않을 경우
        {
            soundManager.clip = wizdomSound[audiointtrigger];
            soundManager.Stop();
            soundManager.Play();
            audioTrigger = !audioTrigger;//오디오 번호 전환
        }
    }
    public void PlayLogicSound()
    {
        int audiointtrigger = audioTrigger ? 1 : 0; //bool을 int로 변환
        if (logicSound != null && logicSound.Length > 0) //배열이 비어있지 않을 경우
        {
            soundManager.clip = logicSound[audiointtrigger];
            soundManager.Stop();
            soundManager.Play();
            audioTrigger = !audioTrigger;//오디오 번호 전환
        }
    }

    public void PlayCutSceneSound()
    {
        if (cutSceneSound != null) //비어있지 않을 경우
        {
            soundManager.clip = cutSceneSound;
            soundManager.Stop();
            soundManager.Play();
        }
    }

    public void PlaySceneEndSound()
    {
        if (sceneEndSound != null) //비어있지 않을 경우
        {
            soundManager.clip = sceneEndSound;
            soundManager.Stop();
            soundManager.Play();
        }
    }
}