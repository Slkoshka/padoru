using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerControls : MonoBehaviour
{
    [SerializeField] private float _movementForce = 1000.0f;
    [SerializeField] private float _jumpForce = 10000.0f;
    [SerializeField] private float _movementSpeedCap = 10.0f;
    [SerializeField] private BoxCollider2D _groundTrigger = null;
    [SerializeField] private float _coyoteTimeDuration = 0.1f;
    [SerializeField] private PlayerSprite _sprite = null;

    private readonly Cooldown _jumpCooldown = new(0.2f);
    private Rigidbody2D _body = null;
    private float _coyoteTime = 0.0f;
    private bool _isPaused = false;
    private float _jump = 0.0f;

    private void Start()
    {
        _body = GetComponent<Rigidbody2D>();
    }

    public void SetPaused(bool paused)
    {
        _isPaused = paused;
        if (_isPaused)
        {
            _jumpCooldown.Trigger();
        }
        _body.simulated = !paused;
    }

    private bool DetectGround(out float friction)
    {
        if (_groundTrigger == null)
        {
            friction = 0;
            return false;
        }

        var colliders = new List<Collider2D>();
        _groundTrigger.GetContacts(colliders);

        if (colliders.Count == 0 || colliders.All(x => x.isTrigger))
        {
            friction = 0;
            return false;
        }
        else
        {
            var modifiers = colliders.Where(x => !x.isTrigger).Select(x => x.GetComponent<SurfaceModifier>()).Where(x => x != null).ToArray();
            friction = modifiers.Length > 0 ? modifiers.Average(x => x.Friction) : 1.0f;
            return true;
        }
    }

    private void GetInput(out float horizontalMovement, out bool jumping)
    {
        horizontalMovement = Input.GetAxis("Horizontal");

        jumping = _jump > 0.0f;
    }

    private void Update()
    {
        _jump -= Time.deltaTime;

        if (!_isPaused)
        {
            if (Input.GetButtonDown("Jump"))
            {
                _jump = 0.1f;
            }
        }
    }

    private void FixedUpdate()
    {
        if (_isPaused)
        {
            return;
        }

        _jumpCooldown.Update(Time.fixedDeltaTime);
        if (DetectGround(out var friction))
        {
            _coyoteTime = _coyoteTimeDuration;
        }
        else
        {
            _coyoteTime -= Time.fixedDeltaTime;
        }
        var onGround = _coyoteTime > 0;

        if (_body == null)
        {
            return;
        }

        GetInput(out var horizontalMovement, out var jumping);

        if (Mathf.Abs(horizontalMovement) > float.Epsilon)
        {
            if ((horizontalMovement < float.Epsilon && _body.velocity.x >= -_movementSpeedCap) || (horizontalMovement > float.Epsilon && _body.velocity.x <= _movementSpeedCap))
            {
                _body.AddForce(new Vector2(_movementForce * horizontalMovement, 0));
            }

            _sprite.Direction = horizontalMovement > 0.0f;
        }
        else
        {
            _body.velocity = new Vector2(_body.velocity.x * (1 - friction), _body.velocity.y);
            if (Mathf.Abs(_body.velocity.x) < 0.1f)
            {
                _body.velocity = new Vector2(0, onGround ? Math.Max(0, _body.velocity.y) : _body.velocity.y);
            }
        }

        if (jumping && onGround && _jumpCooldown.Trigger())
        {
            _body.velocity = new Vector2(_body.velocity.x, _jumpForce);
            _coyoteTime = 0;
        }
    }
}
