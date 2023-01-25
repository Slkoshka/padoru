using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class ForceField : MonoBehaviour
{
    [SerializeField] private Vector2 _force = new Vector2(0, 100);
    private Collider2D _collider = null;

    private void Start()
    {
        _collider = GetComponent<Collider2D>();
    }

    private void FixedUpdate()
    {
        if (_collider == null)
        {
            return;
        }

        var colliders = new List<Collider2D>();
        _collider.GetContacts(colliders);
        foreach (var player in colliders.Select(x => x.GetComponent<Player>()).Where(x => x != null))
        {
            player.GetComponent<Rigidbody2D>().AddForce(_force);
        }
    }
}
