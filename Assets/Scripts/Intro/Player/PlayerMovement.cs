using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class PlayerMovement : MonoBehaviour
{
    
     

    [Header("Move Info")]
    private float inputVertical = 0f; // Ű���� �Է°� �ޱ�
    public float moveSpeed = 10f;  // ĳ���� �̵� �ӵ�
    public float tileSize = 10f;   // Ÿ���� �ʺ�

    private bool isMoving = false;  // �̵� ������ ���θ� ��Ÿ���� ����
    public bool IsMoving { get => isMoving; } // isMoving���� �ۿ��� ���� �� �ֵ���
    // Get/Set Property
    private bool isButtonPressed = false;  // �̵� ��ư�� ���ȴ��� ���θ� ��Ÿ���� ����

    [Header("Trigger Info")]
    private bool canMove = true;

    [Header("Ray Info")]
    private bool canMoveForward = true;
    private bool canMoveBackward = true;

    
    [Header("Gizmo Info")]
    public GameObject GizmoPoint;
    public GameObject BackwardGizmoPoint;
    

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //�̵�
        if (canMove)
        {
            // ���̽�ƽ �Է� ����
            inputVertical = Input.GetAxisRaw("Vertical");
            
                
                // �Է¿� ���� ĳ���͸� �̵���Ų��
                if (inputVertical > 0.8) // ���� Ű �Է� ����
                {
                    if (!isButtonPressed && canMoveForward == true)
                    {
                        StartCoroutine(Move(Vector3.forward));
                        isButtonPressed = true;
                    Debug.Log(inputVertical + "Up");
                    }
                }
                else if (inputVertical < -0.8 && canMoveBackward == true) // ���� Ű �Է� ����
                {
                    if (!isButtonPressed)
                    {
                        StartCoroutine(Move(Vector3.back));
                        isButtonPressed = true;
                    Debug.Log(inputVertical + "Down");
                }
                }
                else
                {
                    isButtonPressed = false;
                }
            
           
        }
        /*
        // Ÿ�� ������ Ray ����
        Ray ray = new Ray(GizmoPoint.transform.position, GizmoPoint.transform.forward);
        //RayCast ����
        RaycastHit hitInfo;
        //�浹 ������ Layer ����
        int layer = 1 << LayerMask.NameToLayer("Tile");
        //Ray �߻�
        if (Physics.Raycast(ray, out hitInfo, Mathf.Infinity, layer))//Gizmo�� Ray�� Tile�� �ε��� ���
        {
            canMoveForward = true; //����(w)�� �㰡�Ѵ�
        }
        else //Ray�� �ε����� ���� ���
        {
            canMoveForward = false; //����(w)�� ���´�
        }

        //Ray �ϳ��� ����
        Ray backwardray = new Ray(BackwardGizmoPoint.transform.position, BackwardGizmoPoint.transform.forward);
        //RayCast ����
        RaycastHit backwardHitInfo;
        //�浹 ������ Layer ����
        int Layer = 1 << LayerMask.NameToLayer("Tile");
        //Ray �߻�
        if (Physics.Raycast(backwardray, out backwardHitInfo, Mathf.Infinity, Layer))//Gizmo�� Ray�� Tile�� �ε��� ���
        {
            canMoveBackward = true; //����(s)�� �㰡�Ѵ�
        }
        else //Ray�� �ε����� ���� ���
        {
            canMoveBackward = false; //����(s)�� ���´�
        }*/

    }

    public IEnumerator Move(Vector3 direction)
    {
        if (isMoving) yield break; // �̹� �̵� ���̶�� �߰����� �̵��� ���´�
       // if (isRotating) yield break;  // ȸ�� ���̶�� �߰����� �̵��� ���´�

        isMoving = true;  // �̵� ����

        float remainingDistance = tileSize;  // ���� �̵� �Ÿ�
        Vector3 startPosition = transform.position;  // ���� ��ġ
        Vector3 endPosition = startPosition + direction * tileSize;  // ��ǥ ��ġ

        while (remainingDistance > 0)
        {
            float moveDistance = moveSpeed * Time.deltaTime;  // �̵� �Ÿ� ���
            remainingDistance -= moveDistance;  // ���� �̵� �Ÿ� ������Ʈ

            transform.Translate(direction * moveDistance);  // ĳ���� �̵�

            yield return null;  // ���� �����ӱ��� ���
        }

        isMoving = false;  // �̵� ����
    }

    /*
    public void OnDrawGizmos()
    {
        //Gizmo�� Ray ����
        Ray ray = new Ray(GizmoPoint.transform.position, GizmoPoint.transform.forward);
        //Gizmo�� RayCast ����
        RaycastHit hitInfo;
        //�浹 ������ Layer ����
        int layer = 1 << LayerMask.NameToLayer("Tile");
        //Gizmo�� Ray �߻�
        if (Physics.Raycast(ray, out hitInfo, Mathf.Infinity, layer))//Gizmo�� Ray�� Tile�� �ε��� ���
        {
            Gizmos.color = Color.red;
            Vector3 startPos = ray.origin;
            Vector3 endPos = hitInfo.point;
            Gizmos.DrawLine(startPos, endPos);
            //Gizmo�� Ray�� �ε��� ��ġ������ �׷��ش�
        }
        else //Gizmo�� Ray�� �ε����� ���� ���
        {
            Gizmos.color = Color.white;
            Vector3 startPos = ray.origin;
            Vector3 endPos = startPos + ray.direction * 100f;
            Gizmos.DrawLine(startPos, endPos);
            //Gizmo�� Ray�� ���� ����(100)��ŭ �׷��ش�
        }

        //Gizmo�� Ray �ϳ��� ����
        Ray backwardray = new Ray(BackwardGizmoPoint.transform.position, BackwardGizmoPoint.transform.forward);
        //Gizmo�� RayCast ����
        RaycastHit backwardHitInfo;
        //�浹 ������ Layer ����
        int Layer = 1 << LayerMask.NameToLayer("Tile");
        //Gizmo�� Ray �߻�
        if (Physics.Raycast(backwardray, out backwardHitInfo, Mathf.Infinity, Layer))//Gizmo�� Ray�� Tile�� �ε��� ���
        {
            Gizmos.color = Color.red;
            Vector3 BackwardstartPos = backwardray.origin;
            Vector3 BackwardendPos = backwardHitInfo.point;
            Gizmos.DrawLine(BackwardstartPos, BackwardendPos);
            //Gizmo�� Ray�� �ε��� ��ġ������ �׷��ش�
        }
        else //Gizmo�� Ray�� �ε����� ���� ���
        {
            Gizmos.color = Color.white;
            Vector3 BackwardstartPos = backwardray.origin;
            Vector3 BackwardendPos = BackwardstartPos + backwardray.direction * 100f;
            Gizmos.DrawLine(BackwardstartPos, BackwardendPos);
            //Gizmo�� Ray�� ���� ����(100)��ŭ �׷��ش�
        }
    }*/
}
