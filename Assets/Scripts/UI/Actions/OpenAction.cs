using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class OpenAction : MonoBehaviour
{
    [SerializeField] private string _target = "";

    public void Trigger()
    {
        Application.OpenURL(_target);
    }
}
