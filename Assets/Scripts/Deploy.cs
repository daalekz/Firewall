using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using System;

/*
    Class taken and modified from: https://unity3d.college/2017/10/08/simple-unity3d-snap-grid-system/
*/
public class Deploy : MonoBehaviour
{
    public static Deploy instance { get; private set; }
    private InGameMenuController igmc;
    
    //needs to get the data from camera and grid (hence they are passed into script via the object)
    public Grid_Setup grid;
    public Camera cam;
    public static Tower selected_tower;
    public static bool moving_tower;

    private GameObject hover_sphere;
    private TowerType build_type;

    public GameObject tower_template;
    public GameObject tower_annihilator;
    public GameObject tower_annihilator_turret;

    public GameObject tower_defender;
    public GameObject tower_defender_turret;

    public GameObject tower_extractor;
    public GameObject tower_extractor_turret;

    public GameObject tower_isolator;
    public GameObject tower_isolator_turret;

    public GameObject tower_scanner;
    public GameObject tower_scanner_tower;

    public RaycastHit hitInfo;
    public Ray ray;

    //initalized necessary variables, objects and other data
    private void Awake()
    {
        if (instance != null) throw new System.Exception();
        instance = this;

        grid = FindObjectOfType<Grid_Setup>();

        //intialized the placeholder hover sphere
        hover_sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        hover_sphere.transform.localScale = new Vector3(0, 0, 0);
        hover_sphere.GetComponent<Renderer>().sortingOrder = 3;

        selected_tower = null;
        build_type = TowerType.Isolator;
    }

    void Start ()
    {
        igmc = InGameMenuController.instance;
    }

    private void Update()
    {
        //gets the mouse positioning in relation to the camera
        ray = cam.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hitInfo))
        {
            //checks the Vector3 position from fields above
            var finalPosition = grid.GetNearestPointOnGrid(hitInfo.point);
            finalPosition.z = 0;

            //draws tower
            if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject() && !moving_tower)
            {
                //checks if the tile has anything placed on it
                if (TileEmpty(finalPosition)) {
                    //if nothing is present on the tile, a new tower is placed on it
                    CreateTower(finalPosition);
                }
            }

            //selects a tower, based on where mouse is
            if (Input.GetMouseButtonDown(0) && Input.GetKey(KeyCode.LeftControl) && !moving_tower)
            {
                SelectTower(finalPosition);
            }

            if (selected_tower != null && moving_tower && Input.GetMouseButtonDown(0))
            {
                //checks if there is anything currently on the tile
                if (TileEmpty(finalPosition))
                {
                    //if there is nothing current on the intended place
                    //tile is moved there
                    RepositionTower(finalPosition);
                }
            }
            //renders hover placement for a given tile
            HoverPlacement(finalPosition);
        }
        else
        {
            hover_sphere.transform.localScale = new Vector3(0, 0, 0);
        }

        //draws towers themselves
        RenderTowers();

        //initalize move of selected tower
        if (selected_tower != null && Input.GetKey(KeyCode.M))
        {
            selected_tower.Active = false;
            moving_tower = true;
        }

        //cancel moving of selected tower
        if (moving_tower && Input.GetKey(KeyCode.C))
        {
            selected_tower.Active = true;
            moving_tower = false;
            selected_tower.Selected = false;
            selected_tower = null;
        }

        //deletes a selected_towers tile
        if (Input.GetKey(KeyCode.Delete))
        {
            RemoveSelectedTower();
        }
    }

    //gets the selected build type from the menu
	public void SetBuildType(int type)
	{
		build_type = (TowerType)type;
        igmc.ToggleMenu(); // Close menu upon selecting a tower
	}

    /*
    Summary:
    Displays an overlay, to show user where a tower might be placed, given their current mouse position
    */
    private void HoverPlacement(Vector3 InputPosition)
    {
        Vector3 mousePosition = InputPosition;
        MapTile temp_tile = DeployTools.SearchTiles(InputPosition, grid.GameMap.Map_Tiles);

        //checks that the value returned, is actually a tile
        if (temp_tile == null)
        {
            hover_sphere.transform.localScale = new Vector3(0, 0, 0);
            return;
        }

        switch (temp_tile.Type)
        {
            //check that the tile is a of tiletype empty
            //if tile type isn't type empty, a tower won't be able to be placed there
            case TileType.empty:
                //makes the hover_sphere game object visible
                hover_sphere.transform.localScale = new Vector3(1, 1, 1);
                //sets object position to that as the underlying tile
                hover_sphere.transform.position = mousePosition;
                var shape_color = hover_sphere.GetComponent<Renderer>().material.color;
                hover_sphere.GetComponent<Renderer>().material.color = Color.white;
                //gets object opactiy to 50%, so that it actually acts as a overlay
                shape_color.a = 0.5f;
                break;

            case TileType.turret:
                hover_sphere.transform.localScale = new Vector3(0, 0, 0);
                break;

            default:
                //makes the hover_sphere game object visible
                hover_sphere.transform.localScale = new Vector3(1, 1, 1);
                //sets object position to that as the underlying tile
                hover_sphere.transform.position = mousePosition;
                hover_sphere.GetComponent<Renderer>().material.color = Color.red;
                shape_color = hover_sphere.GetComponent<Renderer>().material.color;
                //gets object opactiy to 50%, so that it actually acts as a overlay
                shape_color.a = 0.5f;
                break;
        }
    }

    //selects on of the towers currently present on the board
    public void SelectTower(Vector3 finalPosition)
    {
        //checks if the map towers list has been initalized
        if (grid.GameMap.Map_Towers != null)
        {
            //checks if there is a pre-exisitng selected_towers tower
            if (selected_tower == null)
            {
                //assigns selected_towers_tower and modifies object
                selected_tower = DeployTools.SelectTower(grid.GameMap.Map_Towers, finalPosition);

                DeployTools.SelectTower(grid.GameMap.Map_Towers, finalPosition).Selected = true;
            }
            else
            {
                //removes existing selected_towers tower from variables and changes object data
                DeployTools.GetTower(grid.GameMap.Map_Towers, selected_tower).Selected = false;
                selected_tower = null;
            }
        }
    }


    //checks if a tile (from a given input position) is blank
    public bool TileEmpty(Vector3 finalPosition)
    {
        //GameObject.CreatePrimitive(PrimitiveType.Cube).transform.position = finalPosition;

        MapTile temp_tile = DeployTools.SearchTiles(finalPosition, grid.GameMap.Map_Tiles);

        if (temp_tile != null)
        {
            //gets the given tile for the position that the user clicked, and checks that a tower can actually be placed there
            if (temp_tile.Type == TileType.empty)
            {
                return true;
            }
        }
        return false;
    }

    //creates a tower object, on the specificed location, of the selected tower type
    public void CreateTower(Vector3 finalPosition)
    {
        //GameObject.CreatePrimitive(PrimitiveType.Cube).transform.position = finalPosition;
        //sets the tile, to TileType.turrent, letting the game know that a turrent has been placed there
        DeployTools.Manage_Tile_Type(finalPosition, TileType.turret, grid.GameMap.Map_Tiles);

        if (grid.GameMap.Map_Towers != null)
        {
            Debug.Log(build_type);

            switch (build_type)
            {
                case TowerType.Annihilator:
                    //update based on menu selection
                    grid.GameMap.Map_Towers.Add(new Annihilator(finalPosition, true));
                    break;

                case TowerType.Defender:
                    grid.GameMap.Map_Towers.Add(new Defender(finalPosition));
                    break;

                case TowerType.Extractor:
                    //grid.GameMap.Map_Towers.Add();
                    break;

                case TowerType.Isolator:
                    grid.GameMap.Map_Towers.Add(new Isolator(finalPosition));
                    break;

                case TowerType.Scanner:
                    grid.GameMap.Map_Towers.Add(new Scanner(finalPosition));
                    break;
            }
        }   
    }

    //moves a selected tower to a new specified location
    private void RepositionTower(Vector3 finalPosition)
    {
        finalPosition.z = 0;

        //checks if the map towers list has been initalized
        if (grid.GameMap.Map_Towers != null)
        {
            //sets initial tile back to empty
            DeployTools.Manage_Tile_Type(selected_tower.Position, TileType.empty, grid.GameMap.Map_Tiles);


            selected_tower.Active = true;
            selected_tower.UpdatePosition(finalPosition);

            //sets the target position to tile type: turret
            DeployTools.Manage_Tile_Type(finalPosition, TileType.turret, grid.GameMap.Map_Tiles);

            selected_tower.Selected = false;
            selected_tower = null;
            moving_tower = false;
        }
    }

    //draws all of the towers on the board
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

    public void RemoveSelectedTower()
    {
        //check if a tower has been selected_towers
        if (selected_tower != null)
        {
            //removes tower object for Map_Towers list
            grid.GameMap.Map_Towers.Remove(DeployTools.GetTower(grid.GameMap.Map_Towers, selected_tower));
            DeployTools.Manage_Tile_Type(selected_tower.Position, TileType.empty, grid.GameMap.Map_Tiles);
            //gets rid on the actual tower object, tower gun and aimline
            Destroy(selected_tower.tower_gun);
            Destroy(selected_tower.TowerObj);
            Destroy(selected_tower.AimLine);

            //sets selected_towers to null
            selected_tower = null;
        }
    }

    //returns and sets the game current grid
    public Grid_Setup Grid
    {
        get
        {
            return grid;
        }

        //required for testing! (otherwise won't need to be present)
        set
        {
            grid = value;
        }
    }
}
