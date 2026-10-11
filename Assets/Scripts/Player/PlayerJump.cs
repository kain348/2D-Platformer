using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[AddComponentMenu("Game/Player/Player Jump")]
public class PlayerJump : MonoBehaviour
{
    [Header("Jump Settings")]
    [SerializeField] private GroundChecker _groundChecker;
    [SerializeField] private float _jumpHeight = 2.5f;

    private Rigidbody2D _rigidbody;
    private bool _jumpRequested;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();

        if (_groundChecker == null)
            _groundChecker = GetComponentInChildren<GroundChecker>();

        if (_groundChecker == null)
        {
            Debug.LogError($"{nameof(PlayerJump)} on {name}: GroundChecker is not assigned.", this);
            enabled = false;
        }
    }

    private void FixedUpdate()
    {
        if (_jumpRequested == false)
            return;

        _jumpRequested = false;

        TryJump();
    }

    public void RequestJump()
    {
        _jumpRequested = true;
    }

    private void TryJump()
    {
        if (_groundChecker.IsGrounded == false)
            return;

        float gravity = Mathf.Abs(Physics2D.gravity.y * _rigidbody.gravityScale);
        float jumpSpeed = Mathf.Sqrt(2f * gravity * _jumpHeight);

        Vector2 currentVelocity = _rigidbody.velocity;
        currentVelocity.y = jumpSpeed;
        _rigidbody.velocity = currentVelocity;
    }
}