using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayersHealthBar : MonoBehaviour
{
    [SerializeField]
    private Slider _slider;
    [SerializeField]
    private Image _filler;
    [SerializeField]
    private Color _minColor;
    [SerializeField]
    private Color _maxColor;
    [SerializeField]
    private TMPro.TextMeshProUGUI _healthLabel;

    private UnitStats _playerStats;
    private int _defaultHealth;

    private void Start()
    {
        _playerStats = GameManager.Self.Player.UnitStats;
        _defaultHealth = _playerStats.Health;
        _slider.maxValue = _defaultHealth;
        _slider.value = _defaultHealth;
    }
    private void Update()
    {
        _playerStats = GameManager.Self.Player.UnitStats;
        _filler.color = Color.Lerp(_minColor, _maxColor, (float)_playerStats.Health / _defaultHealth);
        _healthLabel.text = _playerStats.Health.ToString() + " / " + _defaultHealth;
        _slider.value = _playerStats.Health;
    }
}
