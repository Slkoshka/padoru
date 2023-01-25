using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TransitionManager : MonoBehaviour
{
    [SerializeField] private Animator _animator = null;
    [SerializeField] private DefaultSceneCamera _mainCamera = null;
    [SerializeField] private string _defaultScene = "MainMenu";
    [SerializeField] private Image _backgroundImage = null;

    private string _currentScene = null;
    private DefaultSceneCamera _currentCamera = null;
    private bool _switchInProgress = false;

    private void Start()
    {
        SwitchScene(_defaultScene);
    }

    public void SwitchScene(string target)
    {
        if (_switchInProgress)
        {
            return;
        }
        StartCoroutine(SwitchSceneCoroutine(target));
    }

    private void Update()
    {
        if (_currentCamera != null)
        {
            _mainCamera.transform.position = _currentCamera.transform.position;
            _mainCamera.transform.localScale = _currentCamera.transform.localScale;
            _mainCamera.transform.rotation = _currentCamera.transform.rotation;
        }
    }

    private IEnumerator SwitchSceneCoroutine(string target)
    {
        _switchInProgress = true;
        Time.timeScale = 1.0f;
        if (_backgroundImage != null)
        {
            _backgroundImage.raycastTarget = true;
        }

        if (_animator != null)
        {
            _animator.SetBool("Show", true);
            yield return new WaitForSeconds(1.0f);
        }

        if (_currentCamera != null)
        {
            _currentCamera.SetEnabled(false);
        }

        _mainCamera.SetEnabled(true);

        if (_currentScene != null)
        {
            yield return SceneManager.UnloadSceneAsync(_currentScene);
        }

        yield return SceneManager.LoadSceneAsync(target, LoadSceneMode.Additive);

        _currentScene = target;
        _currentCamera = SceneManager.GetSceneByName(target).GetRootGameObjects().Select(x => x.GetComponent<DefaultSceneCamera>())
            .FirstOrDefault(x => x != null);

        _mainCamera.SetEnabled(false);
        _currentCamera.SetEnabled(true);

        if (_animator != null)
        {
            _animator.SetBool("Show", false);
            yield return new WaitForSeconds(1.0f);
        }

        if (_backgroundImage != null)
        {
            _backgroundImage.raycastTarget = false;
        }
        _switchInProgress = false;
    }
}
