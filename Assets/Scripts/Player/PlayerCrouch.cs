using System;
using UnityEngine;

[AddComponentMenu("Game/Player/Player Crouch")]
public class PlayerCrouch : MonoBehaviour
{
    private bool _isCrouching;

    public bool IsCrouching => _isCrouching;

    public event Action<bool> CrouchChanged;

    public void SetCrouching(bool isCrouching)
    {
        if (_isCrouching == isCrouching)
            return;

        _isCrouching = isCrouching;
        CrouchChanged?.Invoke(_isCrouching);
    }
}