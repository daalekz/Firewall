using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*
    Summary:
    Class is used to store the various information regarding the game tiles
*/
public class Map
{
    private int _num_tiles;
    private int _tiles_wide;
    private int _tiles_high;
    private List<MapTile> tile_list;
    private List<Tower> map_towers;


    //constructor takes number of tiles for the map grid
    //the number of tiles wide and the number of tiles high
    public Map(int num_tiles, int width, int height)
    {
        _num_tiles = num_tiles;
        _tiles_high = height;
        _tiles_wide = width;

        tile_list = new List<MapTile>();
        map_towers = new List<Tower>();
    }

    //returns the objects private list of tiles present in the game
    public List<MapTile> Map_Tiles
    {
        get
        {
            return tile_list;
        }

        set
        {
            tile_list = value;
        }
    }

    public int NumTiles
    {
        get
        {
            return _num_tiles;
        }

        set
        {
            _num_tiles = value;
        }
    }

    public List<Tower> Map_Towers
    {
        get
        {
            return map_towers;
        }

        set
        {
            map_towers = value;
        }
    }

    public int Width
    {
        get
        {
            return _tiles_wide;
        }

        set
        {
            _tiles_wide = value;
        }
    }

    public int Height
    {
        get
        {
            return _tiles_high;
        }

        set
        {
            _tiles_high = value;
        }
    }
}