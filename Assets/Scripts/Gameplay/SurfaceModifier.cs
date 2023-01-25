using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SurfaceModifier : MonoBehaviour
{
    public float Friction => _friction;
    [SerializeField] private float _friction = 1.0f;
}
