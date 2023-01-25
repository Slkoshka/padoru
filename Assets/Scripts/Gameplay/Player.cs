using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private AudioSource _pickupSounds = null;
    [SerializeField] private AudioSource _deathSounds = null;
    [SerializeField] private PlayerSprite _playerSprite = null;
    [SerializeField] private GameplayManager _gameplayManager = null;

    private Vector3 _respawnPosition;
    private Checkpoint _checkpoint = null;

    private void Start()
    {
        _respawnPosition = transform.position;
    }

    public void Pickup(PickupItem item)
    {
        if (_gameplayManager.State != GameplayManager.GameState.Playing)
        {
            return;
        }

        if (_pickupSounds != null && _pickupSounds.enabled)
        {
            _pickupSounds.Play();
        }

        switch (item.ItemType)
        {
            case PickupItem.PickupItemType.Watermelon:
                _gameplayManager.PickupWatermelon(item.ID);
                break;
        }
    }

    public void CheckpointReached(Checkpoint checkpoint)
    {
        if (_checkpoint != checkpoint)
        {
            _checkpoint = checkpoint;
            _respawnPosition = checkpoint.transform.position;
            
            if (_checkpoint.PickupSounds != null && _checkpoint.PickupSounds.enabled)
            {
                _checkpoint.PickupSounds.Play();
            }
        }
    }

    public void Die()
    {
        if (_gameplayManager.State != GameplayManager.GameState.Playing)
        {
            return;
        }

        _gameplayManager.Died();
        if (_deathSounds != null && _deathSounds.enabled)
        {
            _deathSounds.Play();
        }
        transform.position = _respawnPosition;
        var rigidBody = GetComponent<Rigidbody2D>();
        if (rigidBody)
        {
            rigidBody.velocity = Vector2.zero;
        }
        if (_playerSprite != null)
        {
            _playerSprite.Reset();
        }
    }

    public void Win()
    {
        _gameplayManager.Win();
    }
}
