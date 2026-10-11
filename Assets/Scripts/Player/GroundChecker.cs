using System;
using UnityEngine;

[AddComponentMenu("Game/Player/Ground Checker")]
public class GroundChecker : MonoBehaviour
{
    [Header("Ground Check Settings")]
    [SerializeField] private Transform _checkPoint;
    [SerializeField] private Vector2 _boxSize = new Vector2(0.6f, 0.15f);
    [SerializeField] private LayerMask _groundMask;

    private bool _isGrounded;
    private Collider2D _groundCollider;

    public bool IsGrounded => _isGrounded;
    public Collider2D GroundCollider => _groundCollider;

    public event Action<bool> GroundedChanged;

    private void Awake()
    {
        if (_checkPoint == null)
        {
            Debug.LogError($"{nameof(GroundChecker)} on {name}: check point is not assigned.", this);
            enabled = false;
        }
    }

    private void FixedUpdate()
    {
        bool wasGrounded = _isGrounded;

        _groundCollider = Physics2D.OverlapBox(_checkPoint.position, _boxSize, 0f, _groundMask);
        _isGrounded = _groundCollider != null;

        if (wasGrounded == _isGrounded)
            return;

        GroundedChanged?.Invoke(_isGrounded);
    }

    private void OnDrawGizmosSelected()
    {
        if (_checkPoint == null)
            return;

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(_checkPoint.position, _boxSize);
    }
}