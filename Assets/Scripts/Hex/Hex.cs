using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hex : MonoBehaviour
{
    [SerializeField]
    private Color _highligtColor;
    [SerializeField]
    private Color _selectedColor;
    [SerializeField]
    private Color _dangerColor;

    private Color _defaultColor;
    private SpriteRenderer _spriteRenderer;
    private HexCoordinates _hexCoordinates;
    private bool _isHighlighted;

    public Vector2Int HexCoords => _hexCoordinates.GetHexCoords();

    private void Awake()
    {
        _hexCoordinates = GetComponent<HexCoordinates>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _defaultColor = _spriteRenderer.color;
    }

    public void EnableHighlight()
    {
        _spriteRenderer.color = _highligtColor;
        _isHighlighted = true;
    }

    public void DisableHighlight()
    {
        _spriteRenderer.color = _defaultColor;
        _isHighlighted = false;
    }

    public void EnableSelect()
    {
        _spriteRenderer.color = _selectedColor;
        if (GetComponentInChildren<Enemy>() != null)
            _spriteRenderer.color = _dangerColor;
    }

    public void DisableSelect()
    {
        if (_isHighlighted) _spriteRenderer.color = _highligtColor;
        else _spriteRenderer.color = _defaultColor;
    }
}
