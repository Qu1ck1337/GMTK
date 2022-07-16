using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Unit
{
    [SerializeField]
    private SelectionManager _selectionManager;
    [SerializeField]
    private HexGrid _hexGrid;

    private void Start()
    {
        _hexGrid.OnHexesWereSorted += SelectCurrentHexHandler;
        GameManager.Self.OnPlayersTurn += SelectCurrentHexHandler;
        //_selectionManager.SelectHex(_hex.gameObject);
    }

    public event Action OnPlayerEndedStep;

    public void MoveToSelectedHexagon()
    {
        var selectedHex = _selectionManager.SelectedHex.transform;
        _selectionManager.DisableHighlightsAll();
        if (selectedHex.GetComponentInChildren<Enemy>() == null)
        {
            transform.parent = selectedHex;
            _hex = transform.parent.gameObject.GetComponent<Hex>();
            StartCoroutine(MoveToDestination(_selectionManager.SelectedHex.transform, true));
        }
        else 
        {
            StartCoroutine(MoveToDestination(selectedHex, false));
            StartCoroutine(MoveToDestination(transform.parent.transform, true));
        }
    }

    private void SelectCurrentHexHandler()
    {
        _selectionManager.SelectHex(_hex.gameObject);
    }

    protected override void EndCallEvent()
    {
        OnPlayerEndedStep?.Invoke();
    }
}
