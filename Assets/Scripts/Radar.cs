using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Radar : MonoBehaviour
{
    [SerializeField] Transform _radarPrefab;
    [SerializeField] private float _scale = 15f;
    [SerializeField] private float _speed;
    [field:SerializeField] public float CD { get; private set; }

    public void UseRadar()
    {
        AudioManager.Instance.SoundManager.PlayRadarSound();
        var radar = Instantiate(_radarPrefab,Player.Instance.ShootPont.position, Quaternion.identity);
        radar.DOScale(_scale,_speed).SetEase(Ease.InExpo).OnComplete(()=> Destroy(radar.gameObject));
    }

    public void ReduceRadarCD(float radarCD)
    {
        CD = Mathf.Max(1, CD - radarCD);
    }


}
