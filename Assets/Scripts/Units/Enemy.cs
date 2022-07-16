using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : Unit
{
    [SerializeField]
    private Slider _healthBar;

    private void Start()
    {
        _healthBar.maxValue = _unitStats.Health;
    }

    private void Update()
    {
        _healthBar.value = _unitStats.Health;
    }
}
