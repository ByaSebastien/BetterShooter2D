using System.Collections;
using UnityEngine;

public class DoubleDamage : BasePowerUp
{
    [SerializeField] private float duration = 5f;


    protected override void ApplyPowerUp(PlayerController player)
    {
        player.StartDoubleDamageEffect(duration);
    }
}