using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Tower
{
    private Vector3 _position;
    private GameObject rendered_tower;
    private bool _selected;

    public Tower(int x, int y)
    {
        rendered_tower = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        _position.x = x;
        _position.y = y;
        _position.z = 0;
        var shape_color = rendered_tower.GetComponent<Renderer>().material.color;
        shape_color.a = 0;
    }

    public Tower (Vector3 position)
    {
        rendered_tower = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        _position = position;
        var shape_color = rendered_tower.GetComponent<Renderer>().material.color;
        shape_color.a = 0;
    }

    public void Render()
    {
        rendered_tower.transform.position = _position;

        if (_selected)
        {
            rendered_tower.GetComponent<Renderer>().material.color = Color.yellow;
        }
        else
        {
            rendered_tower.GetComponent<Renderer>().material.color = Color.cyan;
        }
    }


    public bool Selected
    {
        get
        {
            return _selected;
        }

        set
        {
            _selected = value;
        }
    }
    
    public GameObject TowerObj
    {
        get
        {
            return rendered_tower;
        }

        set
        {
            rendered_tower = value;
        }
    }

    public Vector3 Position
    {
        get
        {
            return _position;
        }

        //if you want to move tower position!
        set
        {
            _position = value;
        }
    }
}

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

    public List<Tower> Map_Towers
    {
        get
        {
            return map_towers;
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

/*
    Class taken and modified from: https://unity3d.college/2017/10/08/simple-unity3d-snap-grid-system/
*/
public class Deploy : MonoBehaviour
{
    //needs to get the data from camera and grid (hence they are passed into script via the object)
    public Grid_Setup grid;
    public Camera cam;
    private static Tower selected_tower;

    private void Awake()
    {
        grid = FindObjectOfType<Grid_Setup>();
    }

    //draws sphere when clicked on a given tile
    private void Update()
    {
        RaycastHit hitInfo;
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);

        if (Input.GetMouseButtonDown(0))
        {
            if (Physics.Raycast(ray, out hitInfo))
            {
                PlaceCubeNear(hitInfo.point);
            }
        }
        
        if (Input.GetMouseButton(0) && Input.GetKey(KeyCode.LeftControl))
        {
            if (Physics.Raycast(ray, out hitInfo))
            {
                var finalPosition = grid.GetNearestPointOnGrid(hitInfo.point);
                finalPosition.z = 0;

                if (grid.GameMap.Map_Towers != null)
                {
                    if (selected_tower == null)
                    {
                        DeployTools.SelectTower(grid.GameMap.Map_Towers, finalPosition).Selected = true;
                        selected_tower = DeployTools.SelectTower(grid.GameMap.Map_Towers, finalPosition);
                    }
                    else
                    {
                        DeployTools.GetTower(grid.GameMap.Map_Towers, selected_tower).Selected = false;
                        selected_tower = null;
                    }
                }
            }
        }


        if (Input.GetKey(KeyCode.Delete))
        {
            if (selected_tower != null)
            {
          
                grid.GameMap.Map_Towers.Remove(DeployTools.GetTower(grid.GameMap.Map_Towers, selected_tower));
                DeployTools.Manage_Tile_Type(selected_tower.Position, TileType.empty, grid.GameMap.Map_Tiles);


                Destroy(selected_tower.TowerObj);
                selected_tower = null;
                
            }
        }

        RenderTowers();
    }

    private void HoverPlacement(Vector3 mousePosition)
    {
            var finalPosition = grid.GetNearestPointOnGrid(mousePosition);

           //GameObject.CreatePrimitive(PrimitiveType.Cube).transform.position = finalPosition;

            MapTile temp_tile = DeployTools.SearchTiles(finalPosition, grid.GameMap.Map_Tiles);
            
            if (temp_tile == null)
            {
                return;
            }

            GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            sphere.transform.position = finalPosition;
            var shape_color = sphere.GetComponent<Renderer>().material.color;
            shape_color.a = 0.5f;
    }

    private void PlaceCubeNear(Vector3 clickPoint)
    {
        var finalPosition = grid.GetNearestPointOnGrid(clickPoint);
        //GameObject.CreatePrimitive(PrimitiveType.Cube).transform.position = finalPosition;

        MapTile temp_tile = DeployTools.SearchTiles(finalPosition, grid.GameMap.Map_Tiles);

        //gets the given tile for the position that the user clicked, and checks that a tower can actually be placed there
        if (temp_tile.Type == TileType.empty)
        {
            DeployTools.Manage_Tile_Type(finalPosition, TileType.turret, grid.GameMap.Map_Tiles);

            if (grid.GameMap.Map_Towers != null)
            {
                grid.GameMap.Map_Towers.Add(new Tower(finalPosition));
            }
        }
        else
        {

        }
    }

    private void RenderTowers()
    {
        if (grid.GameMap.Map_Towers != null)
        {
            foreach (Tower tower in grid.GameMap.Map_Towers)
            {
                tower.Render();
            }
        }
    }
}
