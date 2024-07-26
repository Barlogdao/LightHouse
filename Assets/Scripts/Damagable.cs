using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
using Random = UnityEngine.Random;
using Unity.VisualScripting;

public abstract class Damagable : MonoBehaviour
{
    public string Name;
    [TextArea]
    public string Description;

    public Sprite Sprite;



    [SerializeField] private EnemyType _type;
    public EnemyType Type => _type;
    protected const int LIGHT_HOUSE_Layer = 7;
    protected const int RADAR_Layer = 10;
    protected Rigidbody2D _rb;
    [SerializeField] protected float _speed;
    [SerializeField] private Animator _animator;
    [SerializeField] protected SpriteRenderer _sr;
    [SerializeField] protected SpriteRenderer _outlineSr;
    [SerializeField] protected LayerMask _radarLayer;


    [SerializeField] protected int _energyReward;
    [SerializeField] protected int _detailsReward;
    
    

    public string GetInfo()
    {
        return Name + "\n\n" + Description;
    }

    private void Awake()
    {
        _animator = GetComponentInChildren<Animator>();
        _animator.SetBool("IsDead", false);
        _animator.SetBool("InTheRay", false);
        _rb = GetComponent<Rigidbody2D>();
        Game.Instance.StateChanged += OnStateChanged;
    }
    private void Start()
    {
        _outlineSr.enabled = false;
        _rb.AddForce(new Vector2(Random.value, Random.value).normalized * _speed);
        CheckDirection();
        _sr.transform.localScale = Vector3.zero;
        _sr.transform.DOScale(1f, 0.4f);
        OnStart();
    }
    protected virtual void OnStart() { }
    private void OnStateChanged(GameState state)
    {
        if (state == GameState.Day || state == GameState.EndGame) 
        {
            Dissapear();
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        CheckDirection();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.layer == RADAR_Layer) 
        {
           StartCoroutine(ShowOnRadars());  
        }
        if(collision.gameObject.layer == LIGHT_HOUSE_Layer)
        {
            EnterLightRayBehaviour();
            _animator.SetBool("InTheRay", true);

        }
    }


    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LIGHT_HOUSE_Layer)
        {
            LeaveLightRayBehaviour();
            _animator.SetBool("InTheRay", false);
        }
    }
    protected abstract void EnterLightRayBehaviour();


    protected abstract void LeaveLightRayBehaviour();


    IEnumerator ShowOnRadars()
    {
        _outlineSr.enabled = true;
        yield return new WaitForSeconds(Player.Instance.RadarDuration);
        _outlineSr.enabled = false;
    }

    private void CheckDirection()
    {
        _sr.flipX = _rb.velocity.x < 0f;
        _outlineSr.flipX = _sr.flipX = _rb.velocity.x < 0f;
    }

    public abstract void GetDamage(int damage);

    protected void ShowDeath()
    {
        _animator.SetBool("IsDead", true);
        _outlineSr.GetComponent<Animator>().SetBool("IsDead", true);
        _rb.velocity= Vector3.zero;
        _outlineSr.enabled = true;
        _outlineSr.color = Color.red;
       GetComponent<Collider2D>().enabled= false;
        AudioManager.Instance.SoundManager.PlayDeathSound();
    }

    public void Dissapear()
    {
        _rb.velocity = Vector2.zero;
        _sr.transform.DOScale(0f, 0.3f).OnComplete(() => Destroy(gameObject));
    }

    protected virtual void OnDestroy()
    {
        Game.Instance.StateChanged -= OnStateChanged;
    }


}

public enum EnemyType
{
    Standart,
    Defender,
    Kamikadze,
    Invisible,
    Boss
}
