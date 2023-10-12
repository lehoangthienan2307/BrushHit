using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rubber : MonoBehaviour
{
    

    private void Start()
    {
        Transform childTransform = transform.GetChild(1);
        childTransform.gameObject.tag = "Uncolored";
        MeshRenderer[] meshRenderers = GetComponentsInChildren<MeshRenderer>();
        meshRenderers[0].material.color = LevelManager.Instance.GetColored();
        meshRenderers[1].material.color = LevelManager.Instance.GetUnColored();
        
    }
}
