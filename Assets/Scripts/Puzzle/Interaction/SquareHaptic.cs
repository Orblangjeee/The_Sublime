using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class SquareHaptic : MonoBehaviour
{
    private XRBaseController controller;

    [Range(0f, 1f)]
    public float hapticIntensity = 0.7f;
    
    public float duration = 0.1f;


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Push"))
        {
            
            controller = collision.gameObject.GetComponentInParent<XRBaseController>();
            controller.SendHapticImpulse(hapticIntensity, duration);
        }


    }
}
