using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(CanvasGroup))]
public class MenuScreen : MonoBehaviour
{
    [SerializeField] private float _animationDuration = 1.0f;
    [SerializeField] private GameObject _defaultControl = null;

    private Animator _animator = null;
    private CanvasGroup _canvasGroup = null;
    private EventSystem _eventSystem = null;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _canvasGroup = GetComponent<CanvasGroup>();
        _eventSystem = FindObjectOfType<EventSystem>();
    }

    public IEnumerator ShowCoroutine()
    {
        if (_animator == null)
        {
            yield break;
        }

        SetEnabled(false);
        _animator.SetBool("Show", true);
        yield return new WaitForSeconds(_animationDuration);
        SetEnabled(true);
        _eventSystem.SetSelectedGameObject(_defaultControl);
    }

    private void SetEnabled(bool isEnabled)
    {
        if (_canvasGroup == null)
        {
            return;
        }

        _canvasGroup.interactable = isEnabled;
        _canvasGroup.blocksRaycasts = isEnabled;
    }

    public IEnumerator HideCoroutine()
    {
        SetEnabled(false);

        if (_animator == null)
        {
            yield break;
        }

        _animator.SetBool("Show", false);
        yield return new WaitForSeconds(_animationDuration);
    }
}
