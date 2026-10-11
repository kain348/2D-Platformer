using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[AddComponentMenu("Game/Player/Player Mover")]
public class PlayerMover : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float _maxSpeed = 6f;
    [SerializeField] private float _acceleration = 40f;

    private Rigidbody2D _rigidbody;
    private float _moveX;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        ApplyMovement();
    }

    public void SetMove(float moveX)
    {
        _moveX = Mathf.Clamp(moveX, -1f, 1f);
    }

    private void ApplyMovement()
    {
        float targetSpeedX = _moveX * _maxSpeed;
        Vector2 currentVelocity = _rigidbody.velocity;

        currentVelocity.x = Mathf.MoveTowards(currentVelocity.x, targetSpeedX, _acceleration * Time.fixedDeltaTime);
        _rigidbody.velocity = currentVelocity;
    }
}