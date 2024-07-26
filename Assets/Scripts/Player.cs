using DG.Tweening;
using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Player : SingletonPersistent<Player>
{
    public ParticleSystem Salut;
    public ParticleSystem BadSalut;
    [SerializeField] private Transform _energyPiece;
    private float _currentCD => _weapon.LastFireTime;
    private float _currentRadarCD;
    private LightHouse _lightHouse;
    [SerializeField] LayerMask _onlyEnemyLayerMask;

    [SerializeField] private Transform _shootPoint;
    [SerializeField] GameObject _radarUI;
    [SerializeField] Image _radarCdFiller;

    private Radar _radar;
    private bool _isRadarUnlocked = false;
    [field: SerializeField] public float RadarDuration { get; private set; }
    public Transform ShootPont => _shootPoint;

    private int _energyScore = 0;
    private int _deatailsAmount = 0;
    public int DeatailsAmount { get { return _deatailsAmount; } set { _deatailsAmount = Mathf.Max(0, value); DetailsAmountChanged?.Invoke(_deatailsAmount); } }
    public int EnergyScore { get { return _energyScore; } set { _energyScore = Mathf.Max(0, value); EnergyAmountChanged?.Invoke(_energyScore); } }
    public event Action<int> EnergyAmountChanged;
    public event Action<int> DetailsAmountChanged;
    public event Action<WeaponBase> WeaponChanged;
    public HashSet<EnemyType> EnemyUnlocked = new();


    public HashSet<UpgradeType> UpgradesUnlocked = new();

    // Weapons
    private WeaponBase _weapon;
    [SerializeField] private WeaponBase _rifle, _laser, _babaha;
    public List<WeaponBase> WeaponList = new();


    private WeaponBase _nextWeapon;
    private WeaponBase _previosWeapon;
    [SerializeField] Image _weaponCdFiller;
    [SerializeField] Image _nexrWeaponCdFiller;
    [SerializeField] Image _previousWeaponCdFiller;
    public WeaponBase NextWeapon => _nextWeapon;
    public WeaponBase PreviousWeapon=> _previosWeapon;



    private void Start()
    {
        WeaponList.Add(_rifle = Instantiate<WeaponBase>(_rifle));
        _rifle.LastFireTime = Time.time - _rifle.ReloadCD;
        UpgradesUnlocked.Add(UpgradeType.None);
        UpgradesUnlocked.Add(UpgradeType.Rifle);

        _radar = GetComponent<Radar>();
        _lightHouse = FindObjectOfType<LightHouse>();
        SetWeapon(WeaponList[0]);


    }

    public void SetNextWeaponAsMain()
    {
        if (WeaponList.Count > 1)
        {
            SetWeapon(WeaponList[(WeaponList.IndexOf(_weapon) + 1) % WeaponList.Count]);
        }
    }
    public void SetPreviousWeaponAsMain()
    {
        if (WeaponList.Count > 1)
        {
            int index = WeaponList.IndexOf(_weapon) - 1;

            SetWeapon(index < 0 ? WeaponList[^1] : WeaponList[index]);
        }
    }
    private bool IsMouseoverUI()
    {
        

        if (Input.touchCount >0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            if (EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
                return true;
        }
        else if (EventSystem.current.IsPointerOverGameObject())
        {
            return true;
        }
        return false;
    }
    public void UseRadar()
    {
        if (_currentRadarCD + _radar.CD < Time.time && Game.Instance.State == GameState.Night && _isRadarUnlocked)
        {
            _radar.UseRadar();
            _currentRadarCD = Time.time;
        }
    }


    private void Update()
    {
        if (Time.timeScale == 0f) { return; }
        if (Input.GetMouseButtonDown(0) && Game.Instance.State == GameState.Night && !IsMouseoverUI())
        {
            if (_currentCD + _weapon.ReloadCD < Time.time)
            {
                _weapon.Shoot(GetMousePos());
                _weapon.LastFireTime = Time.time;
            }
        }
        if ((Input.GetMouseButtonDown(1) || Input.GetKeyDown(KeyCode.Space)))
        {
            UseRadar();
        }

        
       

        SetWeaponCD(_weapon, _weaponCdFiller);
        if (WeaponList.Count > 1)
        {
            SetWeaponCD(_nextWeapon, _nexrWeaponCdFiller);
            SetWeaponCD(_previosWeapon, _previousWeaponCdFiller);
        }


        if (_isRadarUnlocked && _currentRadarCD + _radar.CD > Time.time)
        {
            _radarCdFiller.fillAmount = ((_currentRadarCD + _radar.CD) - Time.time) / _radar.CD;
        }
        else
        {
            _radarCdFiller.fillAmount = 0;
        }
        
        if (Input.GetKeyDown(KeyCode.A) && WeaponList.Count > 1)
        {
            SetPreviousWeaponAsMain();
        }
        if (Input.GetKeyDown(KeyCode.D) && WeaponList.Count > 1)
        {
            SetNextWeaponAsMain();
        }
        if (Input.GetKeyDown(KeyCode.Alpha1) && WeaponList.Count > 1)
        {
            SetWeapon(_rifle);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2) && WeaponList.Count > 1)
        {
            SetWeapon(_laser);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3) && WeaponList.Count > 2)
        {
            SetWeapon(_babaha);
        }
        
    }


    private void SetWeaponCD(WeaponBase weapon, Image cdFiller)
    {
        if (!cdFiller.isActiveAndEnabled) return;

        if(weapon.LastFireTime + weapon.ReloadCD > Time.time)
        {
            cdFiller.fillAmount = ((weapon.LastFireTime + weapon.ReloadCD) - Time.time) / weapon.ReloadCD;
        }
        else
        {
            cdFiller.fillAmount = 0;
        }
    }

    public void SetWeapon(WeaponBase weapon)
    {
        _weapon = weapon;
        
        if (WeaponList.Count > 1)
        {
            _previosWeapon = (WeaponList.IndexOf(_weapon) - 1) < 0 ? WeaponList[^1] : WeaponList[WeaponList.IndexOf(_weapon) - 1];
            _nextWeapon = WeaponList[(WeaponList.IndexOf(_weapon) + 1) % WeaponList.Count];
        }
        WeaponChanged?.Invoke(_weapon);
    }



    public void AddEnemy(EnemySpawner spawner, EnemyType type)
    {
        if (!EnemyUnlocked.Contains(type))
        {
            EnemyUnlocked.Add(type);
            EventBus.ShowTooltip(spawner.EnemieList[type].GetInfo(), spawner.EnemieList[type].Sprite);
        }
    }

    public void SetUpgrade(UpgradeType type, int upgradeCost,Sprite newSprite = null)
    {
        UpgradesUnlocked.Add(type);
        if (type == UpgradeType.Radar) { _radarUI.SetActive(true); }
        DeatailsAmount -= upgradeCost;
        switch (type)
        {
            case UpgradeType.Laser:
                WeaponList.Add(_laser = Instantiate<WeaponBase>(_laser));
                FindObjectOfType<UiCanvas>().MultipleWeaponsHandler();
                SetWeapon(_laser);
                EventBus.ShowTooltip?.Invoke("Now you can switch your weapons with A/D keys or by click on Weapon icon", null);
                break;
            case UpgradeType.Babaha:
                WeaponList.Add(_babaha = Instantiate<WeaponBase>(_babaha));
                SetWeapon(_babaha);
                EventBus.ShowTooltip?.Invoke("Press 1 to choose Pistol. \nPress 2 to  choose Light rifle.\nPress 3 to choose Babaha.", null);
                break;
            case UpgradeType.SignalRifle:
                _rifle.GetComponent<Rifle>().IsUpgraded = true;
                _rifle.Image = newSprite;
                _rifle.ReloadCD = 1.3f;
                WeaponChanged?.Invoke(_weapon);
                break;
            case UpgradeType.LaserCD:
                _laser.Image = newSprite;
                _laser.ReloadCD = 2.5f;
                WeaponChanged?.Invoke(_weapon);
                break;
            case UpgradeType.BabahaAvoid:
                _babaha.Image = newSprite;
                _babaha.UpgradeLayerMask(_onlyEnemyLayerMask);
                WeaponChanged?.Invoke(_weapon);
                break;
            case UpgradeType.LightRaySpeed:
                _lightHouse.UpgradeLightRaySpeed();
                break;
            case UpgradeType.LightRayInvisible:
                _lightHouse.UpgradeLightRayInvisible();
                break;
            case UpgradeType.InnerLightRange:
                _lightHouse.UpgradeInnerLight();
                break;
            case UpgradeType.Radar:
                _isRadarUnlocked = true;
                EventBus.ShowTooltip?.Invoke("To use Radar press Space/RMB or by click on Radar icon", null);
                break;
            case UpgradeType.RadarDuration:
                RadarDuration = 3f;
                break;
            case UpgradeType.RadarCD:
                _radar.ReduceRadarCD(5f);
                break;
        }

    }
    public static Vector3 GetMousePos()
    {
        var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;
        return mousePos;
    }

    public void ShowEnargyDrain(Vector2 target)
    {
        var piece = Instantiate(_energyPiece,Camera.main.ViewportToWorldPoint(new Vector2(0,1)),Quaternion.identity);
        piece.localScale = Vector3.zero;
        piece.DOScale(3f, 0.3f).OnComplete(() => piece.DOScale(1f, 0.3f));
        piece.DOMove(target, 0.7f).OnComplete(()=> Destroy(piece.gameObject));
    }

}
