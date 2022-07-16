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
        var selectedHex = _selectionManager.SelectedHex.transform;
        if (selectedHex.GetComponentInChildren<Enemy>() == null)
        {
            transform.parent = selectedHex;
            _hex = transform.parent.gameObject;
            SelectCurrentHexHandler();
            StartCoroutine(MoveToDestination(_selectionManager.SelectedHex.transform));
        }
        else 
        {
            StartCoroutine(MoveToDestination(selectedHex));
            StartCoroutine(MoveToDestination(transform.parent.transform));
            SelectCurrentHexHandler();
        }
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var enemy = collision.GetComponent<Enemy>();
        if (enemy != null)
        {
            enemy.ReduceHealth(_unitStats.Damage);
        }
    }
}
