using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : Unit
{
    [SerializeField]
    private Slider _healthBar;

    public int Health => _unitStats.Health;

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
        if (selectedHex.GetComponentInChildren<Player>() == null)
        {
            transform.parent = selectedHex.transform;
            _hex = transform.parent.gameObject.GetComponent<Hex>();
            StartCoroutine(MoveToDestination(selectedHex.transform, true));
        }
        else
        {
            StartCoroutine(MoveToDestination(selectedHex.transform, false));
            StartCoroutine(MoveToDestination(transform.parent.transform, true));
        }
    }

    protected override void EndCallEvent()
    {
        OnEnemyEndedStep?.Invoke();
    }
}
