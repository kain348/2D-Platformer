using UnityEngine;

[AddComponentMenu("Game/Coin/Coin Respawn Point")]
public class CoinRespawnPoint : MonoBehaviour
{
    [SerializeField] private Coin _coin;
    [SerializeField] private CoinView _coinView;
    [SerializeField] private CoinAnimation _coinAnimation;
    [SerializeField] private Transform _spawnAnchor;
    [SerializeField] private float _respawnDelay = 80f;

    private bool _isWaitingForRespawn;
    private float _remainingRespawnTime;

    private Vector3 SpawnPosition => _spawnAnchor.position;

    private void Awake()
    {
        if (_spawnAnchor == null)
            _spawnAnchor = transform;

        if (_coin == null)
            _coin = GetComponentInChildren<Coin>();

        if(_coinView == null)
            _coinView = GetComponentInChildren<CoinView>();

        if (_coin == null || _coinView == null || _spawnAnchor == null)
        {
            Debug.LogError($"{nameof(CoinRespawnPoint)} on {name}: dependencies are not assigned.", this);
            enabled = false;

            return;
        }

        if (_respawnDelay <= 0f)
        {
            Debug.LogError($"{nameof(CoinRespawnPoint)} on {name}: Respawn delay must be greater than 0.", this);
            enabled = false;

            return;
        }

        MoveCoinToSpawnPosition();
        _coinAnimation.Play();
        _coinView.Show();
    }

    private void OnEnable()
    {
        if (_coin == null)
            return;

        _coin.PickedUp += OnCoinPickedUp;
    }

    private void Update()
    {
        if (_isWaitingForRespawn == false)
            return;

        _remainingRespawnTime -= Time.deltaTime;

        if (_remainingRespawnTime > 0f)
            return;

        RespawnCoin();
    }

    private void OnDisable()
    {
        if (_coin == null)
            return;

        _coin.PickedUp -= OnCoinPickedUp;
    }

    private void OnCoinPickedUp(CoinCollector collector)
    {
        if (_isWaitingForRespawn)
            return;

        collector.Collect();

        _coinView.Hide();
        _coinAnimation.Stop();

        _remainingRespawnTime = _respawnDelay;
        _isWaitingForRespawn = true;
    }

    private void RespawnCoin()
    {
        _isWaitingForRespawn = false;

        MoveCoinToSpawnPosition();
        _coinAnimation.Play();
        _coinView.Show();
    }

    private void MoveCoinToSpawnPosition()
    {
        _coin.transform.position = SpawnPosition;
    }
}