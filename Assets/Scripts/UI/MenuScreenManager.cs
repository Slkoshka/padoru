using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuScreenManager : MonoBehaviour
{
    [SerializeField] private MenuScreen _defaultScreen = null;

    private MenuScreen _currentScreen = null;
    private MenuScreen _nextScreen = null;

    private bool _transitionInProgress = false;

    private void Start()
    {
        ShowScreen(_defaultScreen);
    }

    private void Update()
    {
        if (!_transitionInProgress && _nextScreen != _currentScreen)
        {
            ShowScreen(_nextScreen);
        }
    }

    public void ShowScreen(MenuScreen screen)
    {
        _nextScreen = screen;
        if (!_transitionInProgress)
        {
            StartCoroutine(ShowScreenCoroutine(screen));
        }
    }

    public IEnumerator ShowScreenCoroutine(MenuScreen screen)
    {
        _nextScreen = screen;
        while (_transitionInProgress)
        {
            yield return null;
        }

        if (_nextScreen != screen)
        {
            yield break;
        }

        _transitionInProgress = true;

        try
        {
            if (_currentScreen == screen)
            {
                yield break;
            }

            if (_currentScreen != null)
            {
                yield return _currentScreen.HideCoroutine();
            }

            _currentScreen = screen;

            if (_currentScreen != null)
            {
                yield return _currentScreen.ShowCoroutine();
            }
        }
        finally
        {
            _transitionInProgress = false;
        }
    }
}
