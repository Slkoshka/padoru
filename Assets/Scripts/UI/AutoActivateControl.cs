using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class AutoActivateControl : MonoBehaviour
{
    [SerializeField] private GameObject _object = null;

    private EventSystem _eventSystem = null;
    private bool _forceSelection = false;

    private void Awake()
    {
        _eventSystem = FindObjectOfType<EventSystem>();
    }

    private void OnEnable()
    {
        _forceSelection = true;
    }

    private void Update()
    {
        if (_forceSelection)
        {
            _eventSystem.SetSelectedGameObject(_object);
            _forceSelection = false;
        }
    }
}
