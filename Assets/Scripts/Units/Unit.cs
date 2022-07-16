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

    private Collider2D _collider;
    private bool _isAttacking;

    public UnitStats UnitStats => _unitStats;

    private void Awake()
    {
        _hex = GetComponentInParent<Hex>();
        _collider = GetComponent<Collider2D>();
    }

    public event Action<Unit> OnUnitDead;

    public void GetDamage(int damage)
    {
        if (UnityEngine.Random.Range(0f, 1f) < _unitStats.DodgeChance) return;
        float damageProtection = 1 - _unitStats.Protection;
        _unitStats.Protection = 0;
        _unitStats.Health -= Mathf.RoundToInt(damage * damageProtection);
        if (_unitStats.Health <= 0)
        {
            OnUnitDead?.Invoke(this);
            Destroy(gameObject);
        }
    }

    protected IEnumerator MoveToDestination(Transform destination, bool makeEndCallEvent)
    {
        _collider.isTrigger = true;
        _isAttacking = true;
        var time = 0f;
        while (time < 1f)
        {
            transform.position = Vector3.Lerp(transform.position, destination.position, time);
            time += Time.deltaTime;
            yield return null;
        }
        if (makeEndCallEvent)
            EndCallEvent();
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
