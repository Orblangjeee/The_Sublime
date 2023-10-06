using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class HandAnimation : MonoBehaviour
{
    public InputActionProperty gripAction; // Animation (1) : 잡기 (해당 키 PlayerAction[select])
    public InputActionProperty pinchAction; // Animation (2) : 집기 (해당 키 PlayerAction)
    public Animator anim;

    void Start()
    {
        anim = GetComponentInChildren<Animator>();
    }


    void Update()
    {
        //잡기 키를 얼마나 눌렀는지에 대한 값을 가져온다.
        float gripValue = gripAction.action.ReadValue<float>(); ;
        anim.SetFloat("Grip", gripValue);

        float pinchValue = pinchAction.action.ReadValue<float>();
        anim.SetFloat("Trigger", pinchValue);

       
    }
}
