using System.Collections;
using UnityEngine;

public class SpeedBoost : BasePowerUp
{
    
    [SerializeField] private float duration = 5f;
    
    protected override void ApplyPowerUp(PlayerController player)
    {
        player.StartSpeedBoostEffect(duration);
    }
}
