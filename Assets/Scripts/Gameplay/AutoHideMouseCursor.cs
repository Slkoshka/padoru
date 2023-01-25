using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoHideMouseCursor : MonoBehaviour
{
    [SerializeField] private float _timeToHide = 3.0f;
    private float _timeLeft = 0.0f;
    private Vector3 _mousePosition = Vector3.zero;

    private void Update()
    {
        _timeLeft -= Time.unscaledDeltaTime;

        if (Input.mousePresent && Input.mousePosition != _mousePosition)
        {
            _mousePosition = Input.mousePosition;
            _timeLeft = _timeToHide;
        }

        Cursor.visible = _timeLeft > 0.0f;
    }
}
