using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//z축이 항상 카메라를 향한다.
//z축의 회전은 고정

public class SubTitleMove : MonoBehaviour
{
    public Transform mainCamera;//플레이어 카메라
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
