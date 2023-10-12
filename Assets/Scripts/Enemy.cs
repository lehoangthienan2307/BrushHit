using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//TODO: inherit class
public class Enemy : MonoBehaviour
{
    public GameObject player;
    public Transform anchorHubPos, rotateHubPos, hubs;
    public Transform anchorHub, rotateHub;
    private Transform rayCast;
    public bool changeRotating = false;
    private Transform rotating;
    public float rotateSpeed = 3f;
    public LayerMask groundLayer;
    public bool start = false;
    private float elapsedTime; 
    [SerializeField] private float rotationCooldownFrom = 3f;
    [SerializeField] private float rotationCooldownTo = 6f;
    private float rotationCooldown = 2.0f;
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
    private void Start()
    {
        rotating = anchorHubPos;
        rayCast = rotateHub;

        Collider[] playerColliders = player.GetComponentsInChildren<Collider>();
        Collider[] colliders = GetComponentsInChildren<Collider>();
        IgnoreCollisions(playerColliders, colliders);

        rotationCooldown = Random.Range(rotationCooldownFrom, rotationCooldownTo);
        //Physics.IgnoreCollision(player.GetComponent<Collider>(), GetComponentInChildren<Collider>());
    }
    private void Update()
    {
        //TODO: start
        if (!start)
        {
            if (Input.GetMouseButtonDown(0))
            {
                start = true;
            }
        }
        else
        {
            if(!GameManager.Instance.IsGamePause)
            {
                RaycastHit hit;
                if (Physics.Raycast(rayCast.position, Vector3.down, out hit, 10f, groundLayer))
                {
                    
                    
                    if (elapsedTime >= rotationCooldown)
                    {
                        ChangeRotation(hit);
                        elapsedTime = 0f;
                        rotationCooldown = Random.Range(rotationCooldownFrom, rotationCooldownTo);
                    }
                }    
            }
            elapsedTime += Time.deltaTime;
        }
        
        rotating.transform.Rotate(0f, 90.0f * rotateSpeed * Time.deltaTime, 0.0f, Space.Self);
        
        
    }
    private void ChangeRotation(RaycastHit hit)
    {
        transform.SetParent(hit.transform.parent.parent);
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
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player" || other.gameObject.tag == "Enemy")
        {
            return;
        }
        if(other.gameObject.tag!="Uncolored" && start)
        {
            other.gameObject.tag = "Uncolored";
            GameManager.Instance.DecreaseRubberColored();
            GameManager.Instance.Brush(other);
        }

        Vector3 directionToPlayer = (GetPosition() - other.transform.position).normalized;
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
