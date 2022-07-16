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

    private Enemy _enemy;
    private int _enemiesDefaultHealth;
    private void Start()
    {
        _enemy = GetComponentInParent<Enemy>();
        _enemiesDefaultHealth = _enemy.Health;
    }

    private void Update()
    {
        _filler.color = Color.Lerp(_minColor, _maxColor, (float)_enemy.Health / _enemiesDefaultHealth);
    }
}
