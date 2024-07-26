using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lazer : WeaponBase
{
    public Transform LaserRayPref;
    public override void Shoot(Vector2 placePoint)
    {
        AudioManager.Instance.SoundManager.PlayLaser();
        var targets = Physics2D.RaycastAll(Player.Instance.ShootPont.position, placePoint - (Vector2)Player.Instance.ShootPont.position,20f,_layerMask);

        var ray = Instantiate(LaserRayPref, Player.Instance.ShootPont.position, Quaternion.identity);
        ray.up = placePoint - (Vector2)Player.Instance.ShootPont.position;

        foreach (var target in targets)
        {
            if (target.collider.TryGetComponent<Damagable>(out var damagable))
            {
                damagable.GetDamage(_damage);
            }
        }
    }
}
