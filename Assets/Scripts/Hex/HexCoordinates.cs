using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexCoordinates : MonoBehaviour
{
    public static float xOffset = 1.2f, yOffset = 1f;

    [Header("Offset coordinates")]
    [SerializeField]
    private Vector2Int _offsetCoordinates;

    internal Vector2Int GetHexCoords() => _offsetCoordinates;

    private void Awake()
    {
        _offsetCoordinates = ConvertPositionToOffset(transform.position);
    }

    private Vector2Int ConvertPositionToOffset(Vector3 position)
    {
        int x = Mathf.CeilToInt(position.x / xOffset);
        int y = Mathf.RoundToInt(position.y / yOffset);
        return new Vector2Int(x, y);
    }
}
