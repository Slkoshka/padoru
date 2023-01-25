using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Camera))]
public class DefaultSceneCamera : MonoBehaviour
{
    public Camera Camera = null;
    public AudioListener AudioListener = null;

    private void Awake()
    {
        if (Camera == null)
        {
            Camera = GetComponent<Camera>();
        }

        if (AudioListener == null)
        {
            AudioListener = GetComponent<AudioListener>();
        }

        SetEnabled(false);
    }

    public void SetEnabled(bool isEnabled)
    {
        if (Camera != null)
        {
            Camera.enabled = isEnabled;
        }

        if (AudioListener != null)
        {
            AudioListener.enabled = isEnabled;
        }
    }

    private void Start()
    {
        if (FindObjectOfType<TransitionManager>() == null)
        {
            SceneManager.LoadScene("TransitionScene");
        }
    }
}
