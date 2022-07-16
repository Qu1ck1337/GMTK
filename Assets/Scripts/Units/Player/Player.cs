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
        _selectionManager.OnMovingTo += OnMovingToHandler;
        _selectionManager.SelectHex(_hex);
    }
    private void Update()
    {
        transform.position = Vector3.Lerp(transform.position, transform.parent.position, Time.deltaTime * 10);
    }

    private void OnMovingToHandler(Hex hex)
    {
        transform.parent = hex.transform;
        //Vector3.Lerp(transform.position, hex.transform.position, Time.deltaTime * 10);
    }

    private void SelectCurrentHexHandler()
    {
        _selectionManager.SelectHex(_hex);
    }
}
