using UnityEngine;

[RequireComponent(typeof(Animator))]
[AddComponentMenu("Game/Coin/Coin Animation")]
public class CoinAnimation : MonoBehaviour
{
    [SerializeField] private Animator _animator;

    private void Awake()
    {
        if (_animator == null)
            _animator = GetComponent<Animator>();

        if (_animator == null)
        {
            Debug.LogError($"{nameof(CoinAnimation)} on {name}: Animator is not assigned.", this);
            enabled = false;

            return;
        }
    }

    public void Play()
    {
        _animator.enabled = true;
    }

    public void Stop()
    {
        _animator.enabled = false;
    }
}