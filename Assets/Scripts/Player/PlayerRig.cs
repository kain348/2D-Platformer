using UnityEngine;

[AddComponentMenu("Game/Player/Player Rig")]
public class PlayerRig : MonoBehaviour
{
    [SerializeField] private PlayerInput _input;
    [SerializeField] private PlayerMover _mover;
    [SerializeField] private PlayerJump _jump;

    [SerializeField] private GroundChecker _groundChecker;
    [SerializeField] private PlayerCrouch _crouch;
    [SerializeField] private PlayerPlatformDropper _dropper;

    private void Awake()
    {
        if (_input == null)
            _input = GetComponentInChildren<PlayerInput>();

        if (_mover == null)
            _mover = GetComponentInChildren<PlayerMover>();

        if (_jump == null)
            _jump = GetComponentInChildren<PlayerJump>();

        if (_groundChecker == null)
            _groundChecker = GetComponentInChildren<GroundChecker>();

        if (_crouch == null)
            _crouch = GetComponentInChildren<PlayerCrouch>();

        if (_dropper == null)
            _dropper = GetComponentInChildren<PlayerPlatformDropper>();

        if (_input == null || _mover == null || _jump == null || _groundChecker == null || _crouch == null || _dropper == null)
        {
            Debug.LogError($"{nameof(PlayerRig)} on {name}: Player dependencies are not assigned.", this);
            enabled = false;
        }
    }

    private void Update()
    {
        bool isGrounded = _groundChecker.IsGrounded;
        bool isDownHeld = _input.IsDownHeld;
        bool isCrouching = isGrounded && isDownHeld;

        _crouch.SetCrouching(isCrouching);

        if (isCrouching)
            _mover.SetMove(0f);
        else
            _mover.SetMove(_input.MoveX);

        if (_input.ConsumeJumpPressed() == false)
            return;

        if (isDownHeld && isGrounded)
        {
            _dropper.TryDrop();
                return;
        }

        _jump.RequestJump();
    }
}