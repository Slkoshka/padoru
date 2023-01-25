using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public AudioSource PickupSounds => _pickupSounds;
    [SerializeField] private AudioSource _pickupSounds = null;

    private void Start()
    {
        var gameplayManager = FindObjectOfType<GameplayManager>();
        if (gameplayManager != null && !gameplayManager.IsEasyMode)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.TryGetComponent<Player>(out var player))
        {
            player.CheckpointReached(this);
        }
    }
}
