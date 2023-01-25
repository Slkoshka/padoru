using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SwitchSceneAction : MonoBehaviour
{
    [SerializeField] private MenuScreenManager _manager = null;
    [SerializeField] private string _target = "";

    public void Trigger()
    {
        StartCoroutine(SwitchCoroutine());
    }

    private IEnumerator SwitchCoroutine()
    {
        if (_manager != null)
        {
            yield return _manager.ShowScreenCoroutine(null);
        }

        var transition = FindObjectOfType<TransitionManager>();
        if (transition != null)
        {
            transition.SwitchScene(_target);
        }
        else
        {
            SceneManager.LoadScene(_target);
        }
    }
}
