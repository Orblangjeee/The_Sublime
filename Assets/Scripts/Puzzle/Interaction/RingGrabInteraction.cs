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
    private XRGrabInteractable grabInteractable; // XR Interaction Toolkit���� ��Ʈ�ѷ����� ��ȣ�ۿ��� ó��
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        // XRGrabInteractable ������Ʈ�� ������ ����
        grabInteractable = GetComponent<XRGrabInteractable>();

        if (!grabInteractable)
        {
            Debug.Log("Nope");
        }
        // ��Ʈ�ѷ��� Torus�� ���� �� ȣ��� �̺�Ʈ ���
        grabInteractable.onSelectEntered.AddListener(EnterSelect);
        // ��Ʈ�ѷ��� Torus���� ���� �� ȣ��� �̺�Ʈ ���
        grabInteractable.onSelectExited.AddListener(ExitSelect);
        // ��Ʈ�ѷ��� Torus ���� ���� �� ȣ��� �̺�Ʈ ���
        grabInteractable.onHoverEntered.AddListener(EnterHover);
        // ��Ʈ�ѷ��� Torus ��� �� ȣ��� �̺�Ʈ ���
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
