using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//https://unity3d.college/2017/10/08/simple-unity3d-snap-grid-system/


public class Map : MonoBehaviour
{
    private int _num_tiles;
    private int _tiles_wide;
    private int _tiles_high;
    private List<MapTile> tile_list;

    public Map(int num_tiles, int width, int height)
    {
        _num_tiles = num_tiles;
        _tiles_high = height;
        _tiles_wide = width;

        tile_list = new List<MapTile>();
    }

    public List<MapTile> Map_Tiles{
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

public class Deploy : MonoBehaviour
{
    public Grid_Setup grid;
    public Camera cam;


    private void Awake()
    {
        grid = FindObjectOfType<Grid_Setup>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hitInfo;
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hitInfo))
            {
                PlaceCubeNear(hitInfo.point);
            }
        }
    }

    private void PlaceCubeNear(Vector3 clickPoint)
    {
        var finalPosition = grid.GetNearestPointOnGrid(clickPoint);
        //GameObject.CreatePrimitive(PrimitiveType.Cube).transform.position = finalPosition;

        MapTile temp_tile = DeployTools.SearchTiles(finalPosition, grid.GameMap.Map_Tiles);

        if (temp_tile.Type == TileType.empty)
        {
            DeployTools.Manage_Tile_Type(finalPosition, TileType.turret, grid.GameMap.Map_Tiles);
            GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            
            sphere.transform.position = finalPosition;

            sphere.GetComponent<Renderer>().material.color = Color.cyan;

            DeployTools.Manage_Tile_Type(finalPosition, TileType.turret, grid.GameMap.Map_Tiles);

        }
        else
        {

        }
    }
}