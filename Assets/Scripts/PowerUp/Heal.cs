using UnityEngine;

public class Heal : BasePowerUp
{
    protected override void ApplyPowerUp(PlayerController player)
    {
        player.Heal();
    }
}
