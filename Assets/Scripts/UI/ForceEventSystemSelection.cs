using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(EventSystem))]
public class ForceEventSystemSelection : MonoBehaviour
{
    private GameObject _selectedObject = null;
    private EventSystem _eventSystem = null;

    private void Start()
    {
        _eventSystem = GetComponent<EventSystem>();
    }

    private void Update()
    {
        if (_eventSystem != null)
        {
            if (_eventSystem.currentSelectedGameObject == null)
            {
                _eventSystem.SetSelectedGameObject(_selectedObject);
            }
            else
            {
                _selectedObject = _eventSystem.currentSelectedGameObject;
            }
        }
    }
}
