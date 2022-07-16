using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private Player _player;
    [SerializeField]
    private SelectionManager _selectionManager;
    [SerializeField]
    private HexGrid _hexGrid;
    [SerializeField]
    private bool _isPlayersStep = true;
    [SerializeField]
    private bool _isEnemiesStep;
    private int _previousEnemyIndex = 0;
    private UIAssistant _uiAssistant;
    private BuffsAssistant _buffsAssistant;
    private List<Enemy> _enemies = new List<Enemy>();

    public static GameManager Self;
    public UIAssistant UIAssistant => _uiAssistant;
    public Player Player => _player;
    public SelectionManager SelectionManager => _selectionManager;
    public HexGrid HexGrid => _hexGrid;
    public bool IsPlayersStep => _isPlayersStep;

    private void Awake()
    {
        Self = this;
        _uiAssistant = GetComponent<UIAssistant>();
        _buffsAssistant = GetComponent<BuffsAssistant>();
        _isEnemiesStep = !_isPlayersStep;
    }

    private void Start()
    {
        _player.OnPlayerEndedStep += OnPlayerEndedStep;
        _enemies = FindObjectsOfType<Enemy>().ToList();
        OnPlayersTurn?.Invoke();
    }

    public void ResetAllForNextStep()
    {
        //_uiAssistant.ResetDicePlace();
        _buffsAssistant.GetAllBuffs();
        Player.MoveToSelectedHexagon();
        _selectionManager.ClearSelectedHex();
    }

    private void Update()
    {
        if (!_isPlayersStep)
        {
            if (!_isEnemiesStep)
            {
                _isEnemiesStep = true;
                EnemiesStep();
            }
        }
    }

    private void OnPlayerEndedStep()
    {
        _isPlayersStep = false;
    }

    private void EnemiesStep()
    {
        if (_enemies.Count == 0) return;
        foreach (Enemy enemy in _enemies)
        {
            if (enemy == null) _enemies.Remove(enemy);
        }
        _previousEnemyIndex = 0;
        _enemies[_previousEnemyIndex].MakeStep();
        _enemies[_previousEnemyIndex].OnEnemyEndedStep += NextEnemy;
    }

    public event Action OnPlayersTurn;

    private void NextEnemy()
    {
        _enemies[_previousEnemyIndex].OnEnemyEndedStep -= NextEnemy;
        if (_previousEnemyIndex < _enemies.Count - 1)
        {
            _previousEnemyIndex++;
            _enemies[_previousEnemyIndex].MakeStep();
            _enemies[_previousEnemyIndex].OnEnemyEndedStep += NextEnemy;
        }
        else
        {
            _isEnemiesStep = false;
            _isPlayersStep = true;
            OnPlayersTurn?.Invoke();
        }
    }
}
