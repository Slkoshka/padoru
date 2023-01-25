using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupItem : MonoBehaviour
{
    public enum PickupItemType
    {
        None,
        Watermelon,
    }

    public PickupItemType ItemType => _itemType;
    public int ID => _id;

    [SerializeField] private PickupItemType _itemType = PickupItemType.None;
    [SerializeField] private int _id = 0;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.TryGetComponent<Player>(out var player))
        {
            player.Pickup(this);
            Destroy(gameObject);
        }
    }
}
