using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexGrid : MonoBehaviour
{
    Dictionary<Vector2Int, Hex> _hexTileDict = new Dictionary<Vector2Int, Hex>();
    Dictionary<Vector2Int, List<Vector2Int>> _hexTileNeighboursDict = new Dictionary<Vector2Int, List<Vector2Int>>();

    public event Action OnHexesWereSorted;
    private void Start()
    {
        foreach (Hex hex in FindObjectsOfType<Hex>())
        {
            _hexTileDict[hex.HexCoords] = hex;
        }
        OnHexesWereSorted?.Invoke();
    }

    public Hex GetTileAt(Vector2Int hexCoordinates)
    {
        Hex result = null;
        _hexTileDict.TryGetValue(hexCoordinates, out result);
        return result;
    }

    public List<Vector2Int> GetNeighboursFor(Vector2Int hexCoordinates)
    {
        if (_hexTileDict.ContainsKey(hexCoordinates) == false)
            return new List<Vector2Int>();

        if (_hexTileNeighboursDict.ContainsKey(hexCoordinates))
            return _hexTileNeighboursDict[hexCoordinates];

        _hexTileNeighboursDict.Add(hexCoordinates, new List<Vector2Int>());

        foreach (var direction in Direction.GetDirectionList(hexCoordinates.y))
        {
            if (_hexTileDict.ContainsKey(hexCoordinates + direction))
            {
                _hexTileNeighboursDict[hexCoordinates].Add(hexCoordinates + direction);
            }
        }
        return _hexTileNeighboursDict[hexCoordinates];
    }
}

public static class Direction
{
    public static List<Vector2Int> directionOffsetOdd = new List<Vector2Int>
    {
        new Vector2Int(-1, 1),  //N1
        new Vector2Int(0, 1),   //N2
        new Vector2Int(1, 0),   //E
        new Vector2Int(0, -1),  //S2
        new Vector2Int(-1, -1), //S1
        new Vector2Int(-1, 0),  //W
    };

    public static List<Vector2Int> directionOffsetEven = new List<Vector2Int>
    {
        new Vector2Int(0, 1),   //N1
        new Vector2Int(1, 1),   //N2
        new Vector2Int(1, 0),   //E
        new Vector2Int(1, -1),  //S2
        new Vector2Int(0, -1),  //S1
        new Vector2Int(-1, 0),  //W
    };

    public static List<Vector2Int> GetDirectionList(int y) => y % 2 == 0 ? directionOffsetEven : directionOffsetOdd;
}
