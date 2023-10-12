using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Player : MonoBehaviour
{
    public GameObject enemy;
    public Transform anchorHubPos, rotateHubPos, hubs;
    public Transform anchorHub, rotateHub;
    private Transform rayCast;
    public bool changeRotating = false;
    public Transform rotating;
    public float rotateSpeed = 3f;
    public LayerMask groundLayer;
    public bool start = false;
    private void IgnoreCollisions(Collider[] colliders1, Collider[] colliders2)
    {
        foreach (Collider collider1 in colliders1)
        {
            foreach (Collider collider2 in colliders2)
            {
                Physics.IgnoreCollision(collider1, collider2);
            }
        }
    }
    private void Awake()
    {
        rotating = anchorHubPos;
        rayCast = rotateHub;
    }
    private void Start()
    {
        /*rotating = anchorHubPos;
        rayCast = rotateHub;*/

        if (enemy!=null)
        {
            Collider[] enenmyColliders = enemy.GetComponentsInChildren<Collider>();
            Collider[] colliders = GetComponentsInChildren<Collider>();
            IgnoreCollisions(enenmyColliders, colliders);
            //Physics.IgnoreCollision(enemy.GetComponentInChildren<Collider>(), GetComponentInChildren<Collider>());
        }
        
        
    }
    private void Update()
    {
        if(Input.GetMouseButtonDown(0) && !GameManager.Instance.IsGamePause && !EventSystem.current.currentSelectedGameObject)
        {
            if (!start)
            {
                GameManager.Instance.StartPlaying();
            }
            start = true;
            RaycastHit hit;
            if (Physics.Raycast(rayCast.position, Vector3.down, out hit, 10f, groundLayer))
            {
                //So player can move with rubbers
                transform.SetParent(hit.transform.parent.parent);
            }
            else
            {
                GameManager.Instance.GameOver();
            }
            ChangeRotation();
        }
        
        rotating.transform.Rotate(0f, 90.0f * rotateSpeed * Time.deltaTime, 0.0f, Space.Self);
        
        
    }
    public void ChangeRotation()
    {
        GameManager.Instance.CheckCombo();

        if (!changeRotating)
        {
            changeRotating = true;
            anchorHubPos.DetachChildren();
            hubs.SetParent(rotateHubPos);
            anchorHubPos.SetParent(rotateHubPos);
            transform.position = rotateHubPos.position;
            rotateHubPos.SetParent(transform);
            rotating = rotateHubPos;

            rayCast = anchorHub;
            
        }
        else
        {
            changeRotating = false;
            rotateHubPos.DetachChildren();
            hubs.SetParent(anchorHubPos);
            rotateHubPos.SetParent(anchorHubPos);
            transform.position = anchorHubPos.position;
            anchorHubPos.SetParent(transform);
            rotating = anchorHubPos;

            rayCast = rotateHub;
        }

        rotateSpeed = -rotateSpeed;
    }
    public void ChangeRotationForPlayerPowerUp()
    {

        if (changeRotating)
        {
            changeRotating = true;
            anchorHubPos.DetachChildren();
            hubs.SetParent(rotateHubPos);
            anchorHubPos.SetParent(rotateHubPos);
            rotateHubPos.SetParent(transform);
            rotating = rotateHubPos;
            

            rayCast = anchorHub;

            rotateSpeed = -rotateSpeed;
            
        }
        else
        {
            changeRotating = false;
            rotateHubPos.DetachChildren();
            hubs.SetParent(anchorHubPos);
            rotateHubPos.SetParent(anchorHubPos);
            anchorHubPos.SetParent(transform);
            rotating = anchorHubPos;

            rayCast = rotateHub;
        }

        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            return;
        }
        if(other.gameObject.tag!="Colored" && start)
        {
            other.gameObject.tag = "Colored";
            GameManager.Instance.IncreaseRubberColored();
            GameManager.Instance.Brush(other);
        }
        
        Vector3 directionToPlayer = (GetPosition() - other.transform.position).normalized;

        //Vector3 pointOfContact = other.ClosestPoint(GetPosition());
        //Vector3 directionToPlayer = (GetPosition() - pointOfContact).normalized;

        if (changeRotating)
        {
            other.transform.rotation = Quaternion.Euler(-directionToPlayer * 130.0f);
        }
        else
        {
            other.transform.rotation = Quaternion.Euler(directionToPlayer * 130.0f);
        }

    }
    
    private void OnTriggerExit(Collider other)
    {
        Quaternion targetRotation = Quaternion.Euler(0, 0, 0);
        StartCoroutine(InterpolateRotation(other, targetRotation));
    }
    private IEnumerator InterpolateRotation(Collider other, Quaternion targetRotation)
    {
        float elapsedTime = 0f;
        float lerpTime = 1f;
        while (elapsedTime < lerpTime)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / lerpTime);  // Interpolation factor between 0 and 1

            other.transform.rotation = Quaternion.Slerp(other.transform.rotation, targetRotation, t);
            yield return null;
        }
    }
    public Vector3 GetPosition()
    {
        if (!changeRotating)
        {
            return anchorHub.transform.position;
        }
        else
        {
            return rotateHub.transform.position;
        }
    }
}
