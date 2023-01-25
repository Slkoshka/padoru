using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TextButton : MonoBehaviour
{
    [SerializeField] private TMP_Text _textComponent = null;
    [SerializeField] private Color _normalColor = Color.white;
    [SerializeField] private Color _selectedColor = Color.white;
    [SerializeField] private Color _hoverColor = Color.white;

    private bool _isSelected = false;
    private bool _isHover = false;
    private EventSystem _eventSystem = null;

    private void Awake()
    {
        _textComponent = GetComponentInChildren<TMP_Text>();
        _eventSystem = FindObjectOfType<EventSystem>();
    }

    private void OnEnable()
    {
        if (_eventSystem != null)
        {
            _isSelected = _eventSystem.currentSelectedGameObject == gameObject;
        }

        UpdateColors();
    }

    private void UpdateColors()
    {
        if (_textComponent == null)
        {
            return;
        }

        _textComponent.color = (_isHover, _isSelected) switch
        {
            (true, _) => _hoverColor,
            (_, true) => _selectedColor,
            _ => _normalColor,
        };
    }

    public void OnEnter()
    {
        _isHover = true;
        UpdateColors();
    }

    public void OnExit()
    {
        _isHover = false;
        UpdateColors();
    }

    public void OnSelect()
    {
        _isSelected = true;
        UpdateColors();
    }

    public void OnDeselect()
    {
        _isSelected = false;
        UpdateColors();
    }
}
