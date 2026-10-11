using System.Collections;
using UnityEngine;

[AddComponentMenu("Game/Player/Player Platform Dropper")]
public class PlayerPlatformDropper : MonoBehaviour
{
    private const float PlatformClearance = 0.02f;

    [Header("Dependencies")]
    [SerializeField] private GroundChecker _groundChecker;
    [SerializeField] private Collider2D _playerCollider;

    [Header("Drop Settings")]
    [SerializeField] private float _dropDuration = 0.35f;
    [SerializeField] private float _minDownVelocity = -3f;

    private Rigidbody2D _rigidbody;
    private Coroutine _dropRoutine;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();

        if (_groundChecker == null)
            _groundChecker = GetComponentInChildren<GroundChecker>();

        if (_playerCollider == null)
            _playerCollider = GetComponent<Collider2D>();

        if (_groundChecker == null || _playerCollider == null || _rigidbody == null)
        {
            Debug.LogError($"{nameof(PlayerPlatformDropper)} on {name}: dependencies are not assigned.", this);
            enabled = false;
        }
    }

    public bool TryDrop()
    {
        if (_groundChecker.IsGrounded == false)
            return false;

        Collider2D platformCollider = _groundChecker.GroundCollider;
        if (platformCollider == null)
            return false;

        PlatformEffector2D effector = platformCollider.GetComponent<PlatformEffector2D>();
        if (effector == null)
            effector = platformCollider.GetComponentInParent<PlatformEffector2D>();

        if (effector == null)
            return false;

        if (_dropRoutine != null)
            StopCoroutine(_dropRoutine);

        _dropRoutine = StartCoroutine(DropRoutine(platformCollider));
        return true;
    }

    private IEnumerator DropRoutine(Collider2D platformCollider)
    {
        Physics2D.IgnoreCollision(_playerCollider, platformCollider, true);

        Vector2 currentVelocity = _rigidbody.velocity;
        currentVelocity.y = Mathf.Min(currentVelocity.y, _minDownVelocity);
        _rigidbody.velocity = currentVelocity;

        float timeoutTime = Time.time + _dropDuration;

        while (Time.time < timeoutTime)
        {
            float playerTop = _playerCollider.bounds.max.y;
            float platformBottom = platformCollider.bounds.min.y;

            if (playerTop < platformBottom - PlatformClearance)
                break;

            yield return null;
        }

        Physics2D.IgnoreCollision(_playerCollider, platformCollider, false);
        _dropRoutine = null;
    }
}