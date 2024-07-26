using DG.Tweening;
using System.Collections;
using UnityEngine;

public class BossEnemy : Damagable
{
    [SerializeField] private int _health;
    private int _maxHealth;
    private int _stolenEnergy = 0;
    private HealthBar _healthBar;
    private bool ISALIVE => _health > 0;
    private IEnumerator _drainRoutine;
    public override void GetDamage(int damage)
    {
        _health -= damage;
        StopCoroutine(_drainRoutine);
        
        if (!ISALIVE)
        {
            
            ShowDeath();

            Player.Instance.EnergyScore += _energyReward + _stolenEnergy;
            Player.Instance.DeatailsAmount += _detailsReward;
            Destroy(gameObject, 3f);
            _healthBar.gameObject.SetActive(false);
        }
        else
        {
           StartCoroutine(_drainRoutine);
            Hide();
            _healthBar.SetHealth(_health, _maxHealth);
            transform.position = GetPosition();
        }
        
    }
    protected override void OnStart()
    {
        _drainRoutine = DrainEnergyRoutine();
        _maxHealth = _health;
        _healthBar = FindObjectOfType<UiCanvas>().KeeperHealthBar;
        _healthBar.gameObject.SetActive(true);
        _healthBar.SetHealth(_health, _maxHealth);
        StartCoroutine(_drainRoutine);
    }

    private IEnumerator DrainEnergyRoutine()
    {
        yield return new WaitForSeconds(2f);
        while (true)
        {
            int stolenEnergy = Player.Instance.EnergyScore >= 6 ? 6 : Player.Instance.EnergyScore;
            Player.Instance.EnergyScore -= stolenEnergy;
            _stolenEnergy += stolenEnergy;
            Player.Instance.ShowEnargyDrain(transform.position);
            yield return new WaitForSeconds(2f); ;
        }
    }

    private void Hide()
    {
        _sr.DOFade(0f, 0.3f).OnComplete(() => { _sr.DOFade(1f, 0.3f); });
        _outlineSr.DOFade(0f, 0.3f).OnComplete(() => { _outlineSr.DOFade(1f, 0.3f); });
    }

    protected override void EnterLightRayBehaviour()
    {
        if (!ISALIVE) return;
        _rb.velocity *= 2;
    }

    protected override void LeaveLightRayBehaviour()
    {
        if (!ISALIVE) return;
        _rb.velocity /= 2;
    }

    private Vector2 GetPosition()
    {
        var freeTiles = MapProvider.Instance.GetFreePositions();
        return freeTiles[Random.Range(0, freeTiles.Count)];
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        _healthBar.gameObject.SetActive(false);
    }
}
