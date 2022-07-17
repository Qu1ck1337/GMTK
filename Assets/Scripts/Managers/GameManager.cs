using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    [Space, SerializeField]
    private int _nextSceneIndex;
    [Space, SerializeField]
    private AudioSource _mainAudioSource;
    [SerializeField]
    private AudioClip _winLevelMusic;
    [SerializeField]
    private AudioClip _loseLevelMusic;
    private int _previousEnemyIndex = 0;
    private UIAssistant _uiAssistant;
    private BuffsAssistant _buffsAssistant;
    [SerializeField]
    private List<Enemy> _enemies = new List<Enemy>();
    private bool _isLevelEnded;

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
        foreach (Enemy enemy in _enemies)
        {
            enemy.OnUnitDead += RemoveEnemyInEnemiesList;
        }
        OnPlayersTurn?.Invoke();
    }

    private void RemoveEnemyInEnemiesList(Unit enemy)
    {
        _enemies.Remove((Enemy)enemy);
    }

    public void ResetAllForNextStep()
    {
        //_uiAssistant.ResetDicePlace();
        StartCoroutine(_buffsAssistant.GetAllBuffs());
    }

    public void ResetAllForNextStepAfterBuffs()
    {
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
        if (!_isLevelEnded)
        {
            if (_enemies.Count == 0)
            {
                _mainAudioSource.clip = _winLevelMusic;
                _mainAudioSource.loop = false;
                _mainAudioSource.Play();
                StartCoroutine(WaitForTheEndOfLevel(_winLevelMusic.length));
                _isLevelEnded = true;
            }
            if (_player == null)
            {
                _mainAudioSource.clip = _loseLevelMusic;
                _mainAudioSource.loop = true;
                _mainAudioSource.Play();
                _isLevelEnded = true;
                _uiAssistant.InstantiateLosePanel();
            }
        }
    }

    private IEnumerator WaitForTheEndOfLevel(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        SceneManager.LoadScene(_nextSceneIndex);
    }

    private void OnPlayerEndedStep()
    {
        _isPlayersStep = false;
    }

    private void EnemiesStep()
    {
        if (_enemies.Count == 0) return;
        _previousEnemyIndex = 0;
        _enemies[_previousEnemyIndex].OnEnemyEndedStep += NextEnemy;
        _enemies[_previousEnemyIndex].MakeStep();
    }

    public event Action OnPlayersTurn;

    private void NextEnemy()
    {
        _enemies[_previousEnemyIndex].OnEnemyEndedStep -= NextEnemy;
        if (_previousEnemyIndex < _enemies.Count - 1)
        {
            _previousEnemyIndex++;
            _enemies[_previousEnemyIndex].OnEnemyEndedStep += NextEnemy;
            _enemies[_previousEnemyIndex].MakeStep();
        }
        else
        {
            _isEnemiesStep = false;
            _isPlayersStep = true;
            OnPlayersTurn?.Invoke();
        }
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
