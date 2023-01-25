using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchScreenAction : MonoBehaviour
{
    [SerializeField] private MenuScreenManager _manager = null;
    [SerializeField] private MenuScreen _target = null;

    public void Trigger()
    {
        if (_manager != null)
        {
            _manager.ShowScreen(_target);
        }
    }
}
