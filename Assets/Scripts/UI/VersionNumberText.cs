using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class VersionNumberText : MonoBehaviour
{
    private void Start()
    {
        if (TryGetComponent<TMP_Text>(out var textComponent))
        {
            textComponent.text = $"v{Application.version}";
        }
    }
}
