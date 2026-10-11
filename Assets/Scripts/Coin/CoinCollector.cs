using System;
using UnityEngine;

[AddComponentMenu("Game/Coin/Coin Collector")]
public class CoinCollector : MonoBehaviour
{
    private int _count;

    public int Count => _count;

    public event Action<int> CountChanged;

    public void Collect()
    {
        _count++;
        CountChanged?.Invoke(_count);
    }
}