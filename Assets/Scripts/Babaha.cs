using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Babaha : WeaponBase
{
    [SerializeField] private ParticleSystem _particles;
    [SerializeField] private float _radius = 3;
    public override void Shoot(Vector2 placePoint)
    {
        AudioManager.Instance.SoundManager.PlayBabaha();
        var targets = Physics2D.OverlapCircleAll(placePoint,_radius,_layerMask);
        Instantiate(_particles, placePoint, Quaternion.identity);
        foreach (var target in targets)
        {
            if (target.TryGetComponent<Damagable>(out var damagable))
            {
                damagable.GetDamage(_damage);
            }
        }
        
        //shoot

    }
}
