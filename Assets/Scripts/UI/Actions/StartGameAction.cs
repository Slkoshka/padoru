using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGameAction : MonoBehaviour
{
    [SerializeField] private MenuScreenManager _manager = null;
    [SerializeField] private bool _easyMode = false;

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

        var gameSettings = FindObjectOfType<GameSettings>();
        if (gameSettings != null)
        {
            gameSettings.IsEasyMode = _easyMode;
        }

        var transition = FindObjectOfType<TransitionManager>();
        if (transition != null)
        {
            transition.SwitchScene("Gameplay");
        }
        else
        {
            SceneManager.LoadScene("Gameplay");
        }
    }
}
