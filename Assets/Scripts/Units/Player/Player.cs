using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Unit
{
    [SerializeField]
    private SelectionManager _selectionManager;
    [SerializeField]
    private HexGrid _hexGrid;
    [SerializeField]
    private int _defaultHealth;
    [SerializeField]
    private AudioClip _playerDamagesSound;
    [SerializeField]
    private Animator _animator;
    private float _previousDodge;
    private Transform _selectedHex;
    private bool _alreadyDead;
    private Enemy _selectedEnemy;

    public TemporaryBuffs PlayerTemporaryBuffs;

    public int DefaultHealth => _defaultHealth;

    private void Start()
    {
        _animator = GetComponentInChildren<Animator>();
        _hexGrid.OnHexesWereSorted += SelectCurrentHexHandler;
        GameManager.Self.OnPlayersTurn += SelectCurrentHexHandler;
        _defaultHealth = _unitStats.Health;
        _selectionManager.SelectHex(_hex.gameObject);
    }

    public event Action OnPlayerEndedStep;

    public void MoveToSelectedHexagon()
    {
        _unitStats.Damage += PlayerTemporaryBuffs.DamageBoost;
        _unitStats.DodgeChance -= _previousDodge;
        _unitStats.DodgeChance += PlayerTemporaryBuffs.DodgeBoost;
        _previousDodge = PlayerTemporaryBuffs.DodgeBoost;
        var selectedHex = _selectionManager.SelectedHex.transform;
        _selectionManager.DisableHighlightsAll();
        if (selectedHex.GetComponentInChildren<Enemy>() == null)
        {
            _selectedEnemy = null;
            transform.parent = selectedHex;
            _hex = transform.parent.gameObject.GetComponent<Hex>();
            StartCoroutine(WalkingAnim());
        }
        else 
        {
            _selectedHex = selectedHex;
            _selectedEnemy = _selectedHex.GetComponentInChildren<Enemy>();
            _selectedEnemy.gameObject.layer = 8;
            _animator.SetTrigger("PlayerAttack");
        }
    }

    private IEnumerator WalkingAnim()
    {
        _animator.SetTrigger("PlayerWalking");
        yield return new WaitForSeconds(0.2f);
        StartCoroutine(MoveToDestination(transform.parent, true, false));
    }

    public void AttackUnit_AnimationEvent()
    {
        StartCoroutine(MoveToDestination(_selectedHex, false, true));
        StartCoroutine(MoveToDestination(transform.parent.transform, true, false));
    }

    protected override void DieAnimationHandler()
    {
        _alreadyDead = true;
        _animator.SetTrigger("PlayerDies");
        Destroy(this);
    }

    private void SelectCurrentHexHandler()
    {
        _selectionManager.SelectHex(_hex.gameObject);
    }

    protected override void EndCallEvent()
    {
        _unitStats.Damage -= PlayerTemporaryBuffs.DamageBoost;
        PlayerTemporaryBuffs.DamageBoost = 0;
        PlayerTemporaryBuffs.DodgeBoost = 0;
        if (PlayerTemporaryBuffs.ExtraTurn > 0)
        {
            PlayerTemporaryBuffs.ExtraTurn -= 1;
            SelectCurrentHexHandler();
            return;
        }
        OnPlayerEndedStep?.Invoke();
        if (_selectedEnemy != null)
            _selectedEnemy.gameObject.layer = 0;
    }

    public void AddHealth(int health)
    {
        _unitStats.Health += health;
        if (_unitStats.Health > _defaultHealth)
        {
            _unitStats.Health = _defaultHealth;
        }
    }

    public void AddProtection(float protection)
    {
        _unitStats.Protection += protection;
        if (_unitStats.Protection > 1f)
        {
            _unitStats.Protection = 1f;
        }
    }

    public override void GetDamage(int damage)
    {
        base.GetDamage(damage);
        //if (_alreadyDead) return;
        _animator.SetTrigger("PlayerDamaged");
        _audioSource.clip = _playerDamagesSound;
        _audioSource.Play();
    }
}