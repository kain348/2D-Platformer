using UnityEngine;

[RequireComponent (typeof(Collider2D))]
[AddComponentMenu("Game/COin/Coin View")]
public class CoinView : MonoBehaviour
{
    [SerializeField] private Collider2D _triggerCollider;
    [SerializeField] private SpriteRenderer _spriteRenderer;

    private void Awake()
    {
        if(_triggerCollider == null)
            _triggerCollider = GetComponent<Collider2D>();

        if(_spriteRenderer == null)
            _spriteRenderer = GetComponentInChildren<SpriteRenderer>();

        if( _triggerCollider == null || _spriteRenderer == null)
        {
            Debug.LogError($"{nameof(CoinView)} on {name}: dependencies are not assigned.", this);
            enabled = false;

            return;
        }
    }

    public void Show()
    {
        _triggerCollider.enabled = true;
        _spriteRenderer.enabled = true;
    }

    public void Hide()
    {
        _triggerCollider.enabled = false;
        _spriteRenderer.enabled = false;
    }
}
