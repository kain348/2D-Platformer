using System;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
[AddComponentMenu("Game/Coin/Coin")]
public class Coin : MonoBehaviour
{
    [SerializeField] private Collider2D _triggerCollider;

    public event Action<CoinCollector> PickedUp;

    private void Awake()
    {
        if (_triggerCollider == null)
            _triggerCollider = GetComponent<Collider2D>();

        if (_triggerCollider == null)
        {
            Debug.LogError($"{nameof(Coin)} on {name}: Collider2D is not assigned.", this);
            enabled = false;

            return;
        }

        if (_triggerCollider.isTrigger == false)
        {
            Debug.LogError($"{nameof(Coin)} on {name}: Collider2D must be trigger.", this);
            enabled = false;

            return;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Rigidbody2D attachedRigidbody = other.attachedRigidbody;
        if (attachedRigidbody == null)
            return;

        if (attachedRigidbody.TryGetComponent(out PlayerMarker player) == false)
            return;

        if (attachedRigidbody.TryGetComponent(out CoinCollector collector) == false)
        {
            Debug.LogError($"{nameof(Coin)} on {name}: {nameof(CoinCollector)} is missing on player object.", attachedRigidbody);

            return;
        }

        PickedUp?.Invoke(collector);
    }
}