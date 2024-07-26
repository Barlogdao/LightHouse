using UnityEngine;

public class SoundManager : MonoBehaviour
{

    private AudioSource _audioSource;

    [SerializeField] private AudioClip _rifleSound, _laserSound, _babahaSound, _deathSound, _radarSound, _boatSound, _newDaySound, _goodSalut, _badSalut;





    public void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    public void PlayRifle()
    {
        _audioSource.PlayOneShot(_rifleSound);
    }
    public void PlayLaser()
    {
        _audioSource.PlayOneShot(_laserSound);
    }
    public void PlayBabaha()
    {
        _audioSource.PlayOneShot(_babahaSound);
    }
    public void PlayDeathSound()
    {
        _audioSource.PlayOneShot(_deathSound);
    }
    public void PlayRadarSound()
    {
        _audioSource?.PlayOneShot(_radarSound);
    }
    public void PlaeBoatSound()
    {
        _audioSource.PlayOneShot(_boatSound);
    }
    public void PlayNewDay()
    {
        _audioSource.PlayOneShot(_newDaySound);
    }

    public void PlayGoodSalut()
    {
        _audioSource.PlayOneShot(_goodSalut);
    }
    public void PlayBadSalut()
    {
        _audioSource.PlayOneShot(_badSalut);
    }
}