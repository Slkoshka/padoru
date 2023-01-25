using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BackHandler : MonoBehaviour
{
    [SerializeField] private UnityEvent _backPressed = new UnityEvent();

    private void Update()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            _backPressed.Invoke();
        }
    }
}
