using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionManager : MonoBehaviour
{
    [SerializeField]
    private Camera _mainCamera;

    [SerializeField]
    private LayerMask _selectionMask;
    [SerializeField]
    private HexGrid _hexGrid;

    private Hex _selectedHex;
    private List<Vector2Int> _neighbours = new List<Vector2Int>();

    public Hex SelectedHex => _selectedHex;
    public void ClearSelectedHex() => _selectedHex = null;

    private void Awake()
    {
        if (_mainCamera == null)
            _mainCamera = Camera.main;
    }

    public void HandleClick(Vector3 mousePosition)
    {
        GameObject result;
        if (FindTarget(mousePosition, out result))
        {
            SelectHex(result);
        }
    }

    public void SelectHex(GameObject result) 
    {
        Hex selectedHex = result.GetComponent<Hex>();

        if (selectedHex != _selectedHex && _neighbours.Exists(x => x == selectedHex.HexCoords))
        {
            _selectedHex?.DisableSelect();
            _selectedHex = selectedHex;
            _selectedHex.EnableSelect();
        }

        if (selectedHex.GetComponentInChildren<Player>() == null) return;
        _selectedHex?.DisableSelect();
        foreach (Vector2Int neighbour in _neighbours)
        {
            _hexGrid.GetTileAt(neighbour).DisableHighlight();
        }

        _neighbours = _hexGrid.GetNeighboursFor(selectedHex.HexCoords);

        foreach (Vector2Int neighbour in _neighbours)
        {
            _hexGrid.GetTileAt(neighbour).EnableHighlight();
        }
    }

    private bool FindTarget(Vector3 mousePosition, out GameObject result)
    {
        RaycastHit hit;
        Ray ray = _mainCamera.ScreenPointToRay(mousePosition);
        if (Physics.Raycast(ray, out hit, _selectionMask))
        {
            result = hit.collider.gameObject;
            return true;
        }
        result = null;
        return false;
    }

    public void DisableHighlightsAll()
    {
        foreach (Vector2Int neighbour in _neighbours)
        {
            _hexGrid.GetTileAt(neighbour).DisableHighlight();
        }
    }
}
