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
        _selectionManager.SelectHex(_hex);
    }

    public void MoveToSelectedHexagon()
    {
        transform.parent = _selectionManager.SelectedHex.transform;
        _hex = transform.parent.gameObject;
        SelectCurrentHexHandler();
        StartCoroutine(MoveToDestination(_selectionManager.SelectedHex.transform));
        //Vector3.Lerp(transform.position, hex.transform.position, Time.deltaTime * 10);
    }

    private IEnumerator MoveToDestination(Transform destination)
    {
        var time = 0f;
        while (time < 1f)
        {
            transform.position = Vector3.Lerp(transform.position, destination.position, time);
            time += Time.deltaTime;
            yield return null;
        }
    }

    private void SelectCurrentHexHandler()
    {
        _selectionManager.SelectHex(_hex);
    }
}
