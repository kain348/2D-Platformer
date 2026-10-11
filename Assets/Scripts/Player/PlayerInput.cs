using UnityEngine;

[AddComponentMenu("Game/Player/Player Input")]
public class PlayerInput : MonoBehaviour
{
    [Header("Input Settings")]
    [SerializeField] private string _horizontalAxis = "Horizontal";
    [SerializeField] private string _verticalAxis = "Vertical";
    [SerializeField] private string _jumpButton = "Jump";

    private float _moveX;
    private float _moveY;
    private bool _jumpPressed;

    public float MoveX => _moveX;
    public float MoveY => _moveY;
    public bool IsDownHeld => _moveY < -0.5f;

    private void Update()
    {
        _moveX = Input.GetAxisRaw(_horizontalAxis);
        _moveY = Input.GetAxisRaw(_verticalAxis);

        if (Input.GetButtonDown(_jumpButton))
            _jumpPressed = true;
    }

    public bool ConsumeJumpPressed()
    {
        if (_jumpPressed == false)
            return false;

        _jumpPressed = false;

        return true;
    }
}