using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefenderEnemy : Damagable
{
    private bool _inTheRay;
    public override void GetDamage(int damage)
    {
        if (!_inTheRay)
        {
            ShowDeath();

            Player.Instance.EnergyScore +=_energyReward;
            Player.Instance.DeatailsAmount += Random.Range(1, _detailsReward + 1);
            Destroy(gameObject, 3f);
        }
    }

    protected override void EnterLightRayBehaviour()
    {
        _inTheRay= true;
    }

    protected override void LeaveLightRayBehaviour()
    {
        _inTheRay= false;
    }
}
