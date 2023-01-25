using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowGameObject : MonoBehaviour
{
    [SerializeField] private GameObject _target = null;
    [SerializeField] private float _followSpeed = 1.0f;
    [SerializeField] private bool _snapAtStart = true;

    private void Start()
    {
        if (_target != null && _snapAtStart)
        {
            transform.position = new Vector3(_target.transform.position.x, _target.transform.position.y, transform.position.z);
        }
    }

    private void Update()
    {
        if (_target != null)
        {
            var newPosition = Vector3.Lerp(transform.position, _target.transform.position, Time.deltaTime * _followSpeed);
            transform.position = new Vector3(newPosition.x, newPosition.y, transform.position.z);
        }
    }
}
