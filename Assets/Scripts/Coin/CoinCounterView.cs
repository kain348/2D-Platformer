using TMPro;
using UnityEngine;

[AddComponentMenu("Game/UI/Coin Counter View")]
public class CoinCounterView : MonoBehaviour
{
    [SerializeField] private CoinCollector _collector;
    [SerializeField] private TMP_Text _countText;
    [SerializeField] private string _prefix = "Coins: ";

    private void Awake()
    {
        if (_countText == null)
            _countText = GetComponentInChildren<TMP_Text>();

        if (_collector == null || _countText == null)
        {
            Debug.LogError($"{nameof(CoinCounterView)} on {name}: dependencies are not assigned.", this);
            enabled = false;

            return;
        }
    }

    private void OnEnable()
    {
        if (_collector == null)
            return;

        _collector.CountChanged += OnCountChanged;
        UpdateText(_collector.Count);
    }

    private void OnDisable()
    {
        if(_collector == null)
            return;

        _collector.CountChanged -= OnCountChanged;
    }

    private void OnCountChanged(int count)
    {
        UpdateText(count);
    }

    private void UpdateText(int count)
    {
        _countText.text = _prefix + count;
    }
}