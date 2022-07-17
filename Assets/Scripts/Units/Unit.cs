using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Unit : MonoBehaviour
{
    [SerializeField]
    protected UnitStats _unitStats;
    [SerializeField]
    protected Hex _hex;
    [SerializeField]
    private AudioClip _moveSound;
    [SerializeField]
    private DamageBar _damageBar;
    [SerializeField]
    private DamageBar _missBar;

    protected AudioSource _audioSource;
    private Collider2D _collider;
    private bool _isAttacking;

    public UnitStats UnitStats => _unitStats;

    private void Awake()
    {
        _hex = GetComponentInParent<Hex>();
        _collider = GetComponentInChildren<Collider2D>();
        _audioSource = GetComponent<AudioSource>();
    }

    public event Action<Unit> OnUnitDead;

    public virtual void GetDamage(int damage)
    {
        if (UnityEngine.Random.Range(0f, 1f) < _unitStats.DodgeChance)
        {
            _missBar.gameObject.SetActive(true);
            return;
        }
        float damageProtection = 1 - _unitStats.Protection;
        _unitStats.Protection = 0;
        int damageFinal = Mathf.RoundToInt(damage * damageProtection);
        _unitStats.Health -= damageFinal;
        _damageBar.SetDamage(damageFinal);
        _damageBar.gameObject.SetActive(true);
        if (_unitStats.Health <= 0)
        {
            _unitStats.Health = 0;
            OnUnitDead?.Invoke(this);
            DieAnimationHandler();
        }
    }

    protected virtual void DieAnimationHandler()
    {
        Destroy(gameObject);
    }

    protected IEnumerator MoveToDestination(Transform destination, bool makeEndCallEvent, bool attack)
    {
        if (attack)
            _collider.isTrigger = true;
        _collider.enabled = false;
        _collider.enabled = true;
        _isAttacking = true;
        var time = 0f;
        _audioSource.clip = _moveSound;
        _audioSource.Play();
        while (time < 1f)
        {
            transform.position = Vector3.Lerp(transform.position, destination.position, time);
            time += Time.deltaTime;
            yield return null;
        }
        if (makeEndCallEvent)
            EndCallEvent();
        if (attack)
            _collider.isTrigger = false;
        _isAttacking = false;
    }

    protected virtual void EndCallEvent()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!_isAttacking) return;
        var unit = collision.GetComponent<Unit>();
        if (unit != null)
        {
            unit.GetDamage(_unitStats.Damage);
        }
    }
}
