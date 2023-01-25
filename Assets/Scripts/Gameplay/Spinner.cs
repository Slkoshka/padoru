using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spinner : MonoBehaviour
{
    [SerializeField] private float _speed = 0.5f;

    private void FixedUpdate()
    {
        transform.rotation = Quaternion.Euler(0, 0, transform.rotation.eulerAngles.z + _speed);
    }
}
