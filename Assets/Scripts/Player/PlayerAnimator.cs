using UnityEngine;

[AddComponentMenu("Game/Player/Player Animator")]
public class PlayerAnimator : MonoBehaviour
{
    private const string XVelocityParam = "xVelocity";
    private const string YVelocityParam = "yVelocity";
    private const string IsJumpParam = "isJump";
    private const string IsCrouchParam = "isCrouch";

    [SerializeField] private Animator _animator;
    [SerializeField] private Rigidbody2D _rigidbody;
    [SerializeField] private GroundChecker _groundChecker;
    [SerializeField] private PlayerCrouch _crouch;

    private static readonly int XVelocityHash = Animator.StringToHash(XVelocityParam);
    private static readonly int YVelocityHash = Animator.StringToHash(YVelocityParam);
    private static readonly int IsJumpHash = Animator.StringToHash(IsJumpParam);
    private static readonly int IsCrouchHash = Animator.StringToHash(IsCrouchParam);

    private void Awake()
    {
        if (_animator == null)
            _animator = GetComponentInChildren<Animator>();

        if (_rigidbody == null)
            _rigidbody = GetComponentInChildren<Rigidbody2D>();

        if (_groundChecker == null)
            _groundChecker = GetComponentInChildren<GroundChecker>();

        if (_crouch == null)
            _crouch = GetComponentInChildren<PlayerCrouch>();

        if (_animator == null || _rigidbody == null || _groundChecker == null || _crouch == null)
        {
            Debug.LogError($"{nameof(PlayerAnimator)} on {name}: dependencies are not assigned.", this);
            enabled = false;
        }
    }

    private void Update()
    {
        float speedX = Mathf.Abs(_rigidbody.velocity.x);
        float speedY = _rigidbody.velocity.y;

        bool isJumping = _groundChecker.IsGrounded == false;
        bool isCrouching = _crouch.IsCrouching;

        _animator.SetFloat(XVelocityHash, speedX);
        _animator.SetFloat(YVelocityHash, speedY);
        _animator.SetBool(IsJumpHash, isJumping);
        _animator.SetBool(IsCrouchHash, isCrouching);
    }
}