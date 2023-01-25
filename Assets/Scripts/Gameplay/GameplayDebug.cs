using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayDebug : MonoBehaviour
{
    [SerializeField] private Transform _teleportPosition = null;

    private void Start()
    {
#if !UNITY_EDITOR
        Destroy(this);
#endif
    }

    private void Update()
    {
        if (FindObjectOfType<GameplayManager>().State == GameplayManager.GameState.Playing)
        {
            if (Input.GetKeyDown(KeyCode.F10))
            {
                FindObjectOfType<GameplayManager>().ClearScores();
            }
            if (Input.GetKeyDown(KeyCode.F9))
            {
                FindObjectOfType<Player>().transform.position = _teleportPosition.position;
            }
        }
    }
}
