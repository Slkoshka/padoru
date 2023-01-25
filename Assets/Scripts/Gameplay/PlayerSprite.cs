using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class PlayerSprite : MonoBehaviour
{
    public bool Direction { get; set; } = true;

    [SerializeField] private float _flipAnimationSpeed = 10.0f;
    private SpriteRenderer _spriteRenderer = null;
    private float _currentDirection = 0.0f;

    private void Start()
    {
        if (!TryGetComponent(out _spriteRenderer))
        {
            return;
        }

        Reset();
    }

    public void Reset()
    {
        Direction = true;
        _spriteRenderer.transform.localScale = Vector3.one;
        _spriteRenderer.flipX = !Direction;
        _currentDirection = Direction ? 1.0f : 0.0f;
    }

    private void Update()
    {
        _currentDirection = Mathf.Lerp(_currentDirection, Direction ? 1.0f : -1.0f, Time.deltaTime * _flipAnimationSpeed);
        _spriteRenderer.transform.localScale = new Vector3(Mathf.Abs(_currentDirection), 1, 1);
        _spriteRenderer.flipX = _currentDirection < 0.0f;
    }
}
