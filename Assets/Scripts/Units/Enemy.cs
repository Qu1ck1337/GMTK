using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : Unit
{
    [SerializeField]
    private Slider _healthBar;
    [SerializeField]
    private AudioClip _enemyDamagedSound;

    private void Start()
    {
        _healthBar.maxValue = _unitStats.Health;
    }

    private void Update()
    {
        _healthBar.value = _unitStats.Health;
    }

    public void MakeStep()
    {
        if (GameManager.Self.Player == null) return;
        List<Vector2Int> Neighbours = GameManager.Self.HexGrid.GetNeighboursFor(_hex.HexCoords);
        Hex bestHex = null;
        float minDistance = float.MaxValue;
        foreach (Vector2Int neighbour in Neighbours)
        {
            Hex hex = GameManager.Self.HexGrid.GetTileAt(neighbour);
            if (hex != null && hex.GetComponentInChildren<Enemy>() == null)
            {
                float distance = Vector2.Distance(hex.transform.position, GameManager.Self.Player.transform.position);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    bestHex = hex;
                }
            }
        }
        MoveToSelectedHexagon(bestHex);
    }

    public event Action OnEnemyEndedStep;

    public void MoveToSelectedHexagon(Hex selectedHex)
    {
        if (selectedHex == null) return;
        var player = selectedHex.GetComponentInChildren<Player>();
        if (player == null)
        {
            transform.parent = selectedHex.transform;
            _hex = transform.parent.gameObject.GetComponent<Hex>();
            StartCoroutine(MoveToDestination(selectedHex.transform, true, false));
        }
        else
        {
            gameObject.layer = 8;
            StartCoroutine(MoveToDestination(selectedHex.transform, false, true));
            StartCoroutine(MoveToDestination(transform.parent.transform, true, false));
        }
    }

    protected override void EndCallEvent()
    {
        OnEnemyEndedStep?.Invoke();
        gameObject.layer = 0;
    }

    public override void GetDamage(int damage)
    {
        base.GetDamage(damage);
        _audioSource.clip = _enemyDamagedSound;
        _audioSource.Play();
    }
}
