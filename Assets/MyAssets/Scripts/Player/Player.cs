using System;
using System.Collections;
using System.Collections.Generic;
using MyAssets.ScriptableObjects.Events;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private TransformSceneReference CameraReference;
    
    [SerializeField] private GameEvent OnSelectPressed;

    private Camera mainCamera;
    
    void Awake()
    {
        mainCamera = CameraReference.Value.GetComponent<Camera>();
    }
    
    private void OnEnable()
    {
        OnSelectPressed.Subscribe(RaycastTarget);
    }

    private void OnDisable()
    {
        OnSelectPressed.Unsubscribe(RaycastTarget);
    }

    private void RaycastTarget()
    {
        RaycastHit hit;
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        
        if (Physics.Raycast(ray, out hit))
        {
            GameObject hitObj = hit.transform.gameObject;
            Debug.Log(hitObj.name);
        }
    }
}
