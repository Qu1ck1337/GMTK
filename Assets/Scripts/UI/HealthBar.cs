using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField]
    private Color _minColor;
    [SerializeField]
    private Color _maxColor;

    [SerializeField]
    private Image _filler;

    private UnitStats _enemyStats;
    private int _enemieDefaultHealth;
    private void Start()
    {
        _enemyStats = GetComponentInParent<Enemy>().UnitStats;
        _enemieDefaultHealth = _enemyStats.Health;
    }

    private void Update()
    {
        _filler.color = Color.Lerp(_minColor, _maxColor, (float)_enemyStats.Health / _enemieDefaultHealth);
    }
}
