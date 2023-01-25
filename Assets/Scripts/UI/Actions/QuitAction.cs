using System.Collections;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
#endif

using UnityEngine;

public class QuitAction : MonoBehaviour
{
    [SerializeField] private MenuScreenManager _manager = null;

    public void Trigger()
    {
        StartCoroutine(QuitCoroutine());
    }

    private IEnumerator QuitCoroutine()
    {
        if (_manager != null)
        {
            yield return _manager.ShowScreenCoroutine(null);
        }

#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
        Application.Quit();
#endif
    }
}
