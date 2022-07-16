using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Unit : MonoBehaviour
{
    [SerializeField]
    protected UnitStats _unitStats;

    [SerializeField]
    protected GameObject _hex;

    private void Awake()
    {
        _hex = GetComponentInParent<Hex>().gameObject;
    }

    public void ReduceHealth(int health)
    {
        _unitStats.Health -= health;
        if (_unitStats.Health <= 0)
        {
            Destroy(gameObject);
        }
    }
}
