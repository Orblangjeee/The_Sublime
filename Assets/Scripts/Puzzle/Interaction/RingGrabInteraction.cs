using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class RingGrabInteraction : MonoBehaviour
{
    private Rigidbody rb;
    public bool isHover = false;
    public bool isGrab = false;
    private XRGrabInteractable grabInteractable; // XR Interaction Toolkit에서 컨트롤러와의 상호작용을 처리
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        // XRGrabInteractable 컴포넌트를 가져와 설정
        grabInteractable = GetComponent<XRGrabInteractable>();

        if (!grabInteractable)
        {
            Debug.Log("Nope");
        }
        // 컨트롤러가 Torus를 잡을 때 호출될 이벤트 등록
        grabInteractable.onSelectEntered.AddListener(EnterSelect);
        // 컨트롤러가 Torus에서 놓을 때 호출될 이벤트 등록
        grabInteractable.onSelectExited.AddListener(ExitSelect);
        // 컨트롤러가 Torus 위에 있을 때 호출될 이벤트 등록
        grabInteractable.onHoverEntered.AddListener(EnterHover);
        // 컨트롤러가 Torus 벗어날 때 호출될 이벤트 등록
        grabInteractable.onHoverExited.AddListener(ExitHover);
    }

    public void EnterHover(XRBaseInteractor interactor)
    {
        isHover = true;
    }

    public void ExitHover(XRBaseInteractor interactor)
    {
        isHover = false;
        if (!isGrab)
        {
            rb.isKinematic = false;

        }
    }
    public void EnterSelect(XRBaseInteractor interactor)
    {
        isGrab = true;
    }

    public void ExitSelect(XRBaseInteractor interactor)
    {
        isGrab = false;
        if (isHover)
        {
            rb.isKinematic = true;
        } 
    }

}
