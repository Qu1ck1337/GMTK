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

    public TemporaryBuffs PlayerTemporaryBuffs;
    public int DefaultHealth => _defaultHealth;

    private void Start()
    {
        _hexGrid.OnHexesWereSorted += SelectCurrentHexHandler;
        GameManager.Self.OnPlayersTurn += SelectCurrentHexHandler;
        _defaultHealth = _unitStats.Health;
        //_selectionManager.SelectHex(_hex.gameObject);
    }

    public event Action OnPlayerEndedStep;

    public void MoveToSelectedHexagon()
    {
        _unitStats.Damage += PlayerTemporaryBuffs.DamageBoost;
        _unitStats.DodgeChance += PlayerTemporaryBuffs.DodgeBoost;
        var selectedHex = _selectionManager.SelectedHex.transform;
        _selectionManager.DisableHighlightsAll();
        if (selectedHex.GetComponentInChildren<Enemy>() == null)
        {
            transform.parent = selectedHex;
            _hex = transform.parent.gameObject.GetComponent<Hex>();
            StartCoroutine(MoveToDestination(_selectionManager.SelectedHex.transform, true));
        }
        else 
        {
            StartCoroutine(MoveToDestination(selectedHex, false));
            StartCoroutine(MoveToDestination(transform.parent.transform, true));
        }
    }

    private void SelectCurrentHexHandler()
    {
        _selectionManager.SelectHex(_hex.gameObject);
    }

    protected override void EndCallEvent()
    {
        
        _unitStats.Damage -= PlayerTemporaryBuffs.DamageBoost;
        _unitStats.DodgeChance -= PlayerTemporaryBuffs.DodgeBoost;
        PlayerTemporaryBuffs.DamageBoost = 0;
        PlayerTemporaryBuffs.DodgeBoost = 0;
        if (PlayerTemporaryBuffs.ExtraTurn > 0)
        {
            PlayerTemporaryBuffs.ExtraTurn -= 1;
            SelectCurrentHexHandler();
            return;
        }
        OnPlayerEndedStep?.Invoke();
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
}