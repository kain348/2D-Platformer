using UnityEngine;

[AddComponentMenu("Game/Enemy/Enemy Sprite Facing")]
public class EnemySpriteFacing : MonoBehaviour
{
    [SerializeField] private EnemyPatroller _patroller;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private bool _isSpriteFacingRightByDefault = true;

    private void Awake()
    {
        if (_patroller == null)
            _patroller = GetComponentInParent<EnemyPatroller>();

        if (_spriteRenderer == null)
            _spriteRenderer = GetComponentInChildren<SpriteRenderer>();

        if (_patroller == null || _spriteRenderer == null)
        {
            Debug.LogError($"{nameof(EnemySpriteFacing)} on {name}: dependencies are not assigned", this);
            enabled = false;

            return;
        }
    }

    private void OnEnable()
    {
        if (_patroller == null)
            return;

        _patroller.FacingChanged += OnFacingChanged;
    }

    private void Start()
    {
        if (_patroller == null)
            return;

        ApplyFacing(_patroller.FacingSign);
    }

    private void OnDisable()
    {
        if (_patroller == null)
            return;

        _patroller.FacingChanged -= OnFacingChanged;
    }

    private void OnFacingChanged(int facingSign)
    {
        ApplyFacing(facingSign);
    }

    private void ApplyFacing(int facingSign)
    {
        if (_isSpriteFacingRightByDefault)
        {
            _spriteRenderer.flipX = facingSign < 0;

            return;
        }

        _spriteRenderer.flipX = facingSign > 0;
    }
}