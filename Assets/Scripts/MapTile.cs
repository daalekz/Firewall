using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapTile
{
    private TileType _tile_type;
    private Vector3 _tile_position;


    public MapTile(TileType type, Vector3 position)
    {
        _tile_position = position;
        _tile_type = type;
    }

    public MapTile(Vector3 position)
    {
        _tile_type = TileType.empty;
        _tile_position = position;
    }

    public TileType Type
    {
        get
        {
            return _tile_type;
        }

        set
        {
            _tile_type = value;
        }
    }

    public Vector3 Position
    {
        get
        {
            return _tile_position;
        }

        set
        {
            _tile_position = value;
        }
    }
}
