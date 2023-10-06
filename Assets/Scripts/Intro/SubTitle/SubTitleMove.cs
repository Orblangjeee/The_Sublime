using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//z���� �׻� ī�޶� ���Ѵ�.
//z���� ȸ���� ����

public class SubTitleMove : MonoBehaviour
{
    public Transform mainCamera;//�÷��̾� ī�޶�
    public Transform target; 
    public float smoothSpeed = 2f;
    public float rotSpeed = 3f;
    
    void Start()
    {
       
    }

    
    void LateUpdate()
    {
        Vector3 direction = transform.position - mainCamera.position ;

        transform.position = Vector3.Lerp(transform.position, target.position, smoothSpeed * Time.deltaTime) ;
        transform.forward = Vector3.Lerp(transform.forward, direction, rotSpeed * Time.deltaTime);
        
        transform.rotation = Quaternion.Lerp(transform.rotation, target.rotation, rotSpeed * Time.deltaTime);
     
    }
}
