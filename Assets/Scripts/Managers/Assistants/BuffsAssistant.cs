using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BuffsAssistant : MonoBehaviour
{
    [SerializeField]
    private CubeSlot[] _buffs;
    [SerializeField, Range(0f, 1f)]
    private float _damageBoostPerCent = 0.2f;
    [SerializeField, Range(0f, 1f)]
    private float _protectionBoostPerCent = 0.2f;
    [SerializeField, Range(0f, 1f)]
    private float _dodgeBoost = 0.2f;
    [SerializeField, Range(0f, 1f)]
    private float _healingPerCent = 0.2f;

    private List<CubeSlot> _cubeSlots = new List<CubeSlot>();

    private void Start()
    {
        _cubeSlots = FindObjectsOfType<CubeSlot>().ToList();
    }

    public void BuffActivate(BuffType type, int number)
    {
        switch (type)
        {
            case BuffType.DamageBoost:
                if (number % 2 != 0) return;
                GameManager.Self.Player.PlayerTemporaryBuffs.DamageBoost = Mathf.RoundToInt(GameManager.Self.Player.UnitStats.Damage * _damageBoostPerCent);
                break;
            case BuffType.ProtectionBoost:
                if (number % 2 != 0) return;
                GameManager.Self.Player.AddProtection(_protectionBoostPerCent);
                break;
            case BuffType.DodgeBoost:
                if (number % 2 != 0) return;
                GameManager.Self.Player.PlayerTemporaryBuffs.DodgeBoost = _dodgeBoost;
                break;
            case BuffType.Healing:
                if (number % 2 != 0) return;
                GameManager.Self.Player.AddHealth(Mathf.RoundToInt(GameManager.Self.Player.DefaultHealth * _healingPerCent));
                break;
            case BuffType.ExtraTurn:
                if (number != 6) return;
                GameManager.Self.Player.PlayerTemporaryBuffs.ExtraTurn = 1;
                break;
        }
    }

    public void GetAllBuffs()
    {
        foreach(CubeSlot slot in _cubeSlots)
        {
            var dice = slot.gameObject.GetComponentInChildren<Dice>();
            Debug.Log(dice);
            if (dice != null)
            {
                dice.ResetPlace();
                BuffActivate(slot.BuffType, Random.Range(1, 7));
            }
        }
    }
}
