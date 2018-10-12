using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

//enum represents the types of objects that can be present on the tile
public enum TileType
{
    path,
    terrain,
    turret,
    empty
}

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


//class is a number of static methods that can used for data processing (i.e. not object specific) 
public class DeployTools
{
    public static MapTile SearchTiles(Vector3 Search_Position, List<MapTile> Search_List)
    {
        foreach (MapTile tile in Search_List)
        {
            if (tile.Position == Search_Position)
            {
                return tile;
            }
        }

        return null;
    }


    public static void Manage_Tile_Type(Vector3 Search_Position, TileType change_type, List<MapTile> Search_List)
    {
        foreach (MapTile tile in Search_List)
        {
            if (tile.Position == Search_Position)
            {
                tile.Type = change_type;
                return;
            }
        }
    }

    public static Tower SelectTower(List<Tower> towers, Vector3 Search_Position)
    {
        foreach(Tower tower in towers)
        {
            if (tower.Position.x == Search_Position.x && tower.Position.y == Search_Position.y)
            {
                return tower;
            }
        }

        return null;
    }

    public static Tower GetTower(List<Tower> towers, Tower selected_tower)
    {
        foreach (Tower tower in towers)
        {
            if (selected_tower == tower)
            {
                return tower;
            }
        }

        return null;
    }
}

/*
Class taken and modified from:
https://unity3d.college/2017/10/08/simple-unity3d-snap-grid-system/
*/
public class Grid_Setup : MonoBehaviour {
    private float size = 1f;
    private List<MapTile> tiles = new List<MapTile>();

    private Map game_map;

    // Use this for initialization
    void Start() {
        game_map = GetMapData();
        OnDraw(game_map);
    }
    
    // Update is called once per frame
    void Update() {

    }
    

   //reads through a data file to create the game map
    public Map GetMapData()
    {
        string file_path = "Assets/Data/sample_map.txt";

        int num_cells, width, height;

        /*
         * Note regarding file structure:
         * First Line: Total Number of Cells
         * Second Line: Number of horizontal blocks
         * Third Line: Number of vertical block
         * 4th - EOF: tile type (number that is converted to enum) - goes left to right, top to bottom
         * (hence last line on file represent the bottom right tile
        */
        
        using (StreamReader sr = new StreamReader(file_path, true))
        {
            num_cells = Convert.ToInt32(sr.ReadLine());

            width = Convert.ToInt32(sr.ReadLine());

            height = Convert.ToInt32(sr.ReadLine());

            //creates game map that will be returned
            Map method_map = new Map(num_cells, width, height);

            //gets the tile x and y values, so when it called, it can be rendered
            for (float y = height; y > 0; y -= size)
            {
                for (float x = 0; x < width; x += size)
                {
                    var point = GetNearestPointOnGrid(new Vector3(x, y, 0.2f));
                    string line;
                    if ((line = sr.ReadLine()) != null)
                    {
                        int line_value = Convert.ToInt32(line);
                        //creates a tile (from the calculated values and file data) and adds it to list within map obj
                        method_map.Map_Tiles.Add(new MapTile((TileType)line_value, point));
                    }
                    else
                    {
                        break;
                    }
                }
            }
            // Read the stream to a string, and write the string to the console.
            return method_map;
        }
    }
    
    //for a given mouse position, the closest grid point is found
    public Vector3 GetNearestPointOnGrid(Vector3 position)
    {
        position -= transform.position;

        int xCount = Mathf.RoundToInt(position.x / size);
        int yCount = Mathf.RoundToInt(position.y / size);
        int zCount = Mathf.RoundToInt(position.z / size);

        Vector3 result = new Vector3(
            (float)xCount * size,
            (float)yCount * size,
            (float)zCount * size);

        result += transform.position;

        return result;
    }

    //draws the game squares 
    void OnDraw(Map map)
    {
        foreach(MapTile tile in map.Map_Tiles)
        {
            GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cube.transform.position = tile.Position;
            cube.transform.localScale = new Vector3(0.9f, 0.9f, 0.1f);

            //draws differently colored squares depending on the enum value
            switch (tile.Type)
            {
                case (TileType.empty):
                   cube.GetComponent<Renderer>().material.color = Color.white;
                    break;

                case (TileType.path):
                    cube.GetComponent<Renderer>().material.color = Color.red;
                    break;

                case (TileType.terrain):
                    cube.GetComponent<Renderer>().material.color = Color.green;
                    break;
            }
        }
    }

    public List<MapTile> TileList
    {
        get
        {
            return game_map.Map_Tiles;
        }
    }

    public int Width
    {
        get
        {
            return game_map.Width;
        }
    }

    public int Height
    {
        get
        {
            return game_map.Height;
        }
    }

    public Map GameMap
    {
        get
        {
            return game_map;
        }
    }
}
