using UnityEngine;

[AddComponentMenu("Game/Player/Player Sprite Facing")]
public class PlayerSpriteFacing : MonoBehaviour
{
    [SerializeField] private Rigidbody2D _rigidbody;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private float _deadZone = 0.01f;

    private void Awake()
    {
        if (_rigidbody == null)
            _rigidbody = GetComponentInChildren<Rigidbody2D>();

        if (_spriteRenderer == null)
            _spriteRenderer = GetComponentInChildren<SpriteRenderer>();

        if (_rigidbody == null || _spriteRenderer == null)
        {
            Debug.LogError($"{nameof(PlayerSpriteFacing)} on {name}: dependencies are not assigned.", this);
            enabled = false;
        }
    }

    private void Update()
    {
        float vx = _rigidbody.velocity.x;

        if (Mathf.Abs(vx) < _deadZone)
            return;

        _spriteRenderer.flipX = vx < 0f;
    }
}
