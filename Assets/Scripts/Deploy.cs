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

    //initalized necessary variables, objects and other data
    private void Awake()
    {
        if (instance != null) throw new System.Exception();
        instance = this;

        grid = FindObjectOfType<Grid_Setup>();

        hover_sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        hover_sphere.transform.localScale = new Vector3(0, 0, 0);
        hover_sphere.GetComponent<Renderer>().sortingOrder = 3;

        selected_tower = null;
        build_type = TowerType.Defender;
    }

    void Start ()
    {
        igmc = InGameMenuController.instance;
    }

    private void Update()
    {
        RaycastHit hitInfo;
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);

        //draws tower
        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            if (Physics.Raycast(ray, out hitInfo))
            {
                CreateTower(hitInfo.point);
            }
        }

        //selects a tower
        if (Input.GetMouseButtonDown(0) && Input.GetKey(KeyCode.LeftControl))
        {
            if (Physics.Raycast(ray, out hitInfo))
            {
                var finalPosition = grid.GetNearestPointOnGrid(hitInfo.point);
                finalPosition.z = 0;

                //checks if the map towers list has been initalized
                if (grid.GameMap.Map_Towers != null)
                {
                    //checks if there is a pre-exisitng selected tower
                    if (selected_tower == null)
                    {
                        //assigns selected_tower and modifies object
                        selected_tower = DeployTools.SelectTower(grid.GameMap.Map_Towers, finalPosition);

                        DeployTools.SelectTower(grid.GameMap.Map_Towers, finalPosition).Selected = true;

                    }
                    else
                    {
                        //removes existing selected tower from variables and changes object data
                        DeployTools.GetTower(grid.GameMap.Map_Towers, selected_tower).Selected = false;
                        selected_tower = null;
                    }
                }
            }
        }

        //deletes a selected tile
        if (Input.GetKey(KeyCode.Delete))
        {
            //check if a tower has been selected
            if (selected_tower != null)
            {
                //removes tower object for Map_Towers list
                grid.GameMap.Map_Towers.Remove(DeployTools.GetTower(grid.GameMap.Map_Towers, selected_tower));
                DeployTools.Manage_Tile_Type(selected_tower.Position, TileType.empty, grid.GameMap.Map_Tiles);
                Destroy(selected_tower.tower_gun);
                //gets rid on the actual tower object
                Destroy(selected_tower.TowerObj);

                //sets selected to null
                selected_tower = null;
            }
        }

        //draws towers themselves
        RenderTowers();

        //renders hover placement for a given tile
        if (Physics.Raycast(ray, out hitInfo))
        {
            var finalPosition = grid.GetNearestPointOnGrid(hitInfo.point);
            finalPosition.z = 0;

            HoverPlacement(finalPosition);
        }
        else
        {
            //if user's cursor isn't on the board, then there should be no overlay shown
            hover_sphere.transform.localScale = new Vector3(0, 0, 0);
        }
    }

	public void SetBuildType(int type)
	{
		build_type = (TowerType)type;
        igmc.ToggleMenu(); // Close menu upon selecting a tower
	}

    /*
     * Summary:
     * Displays an overlay, to show user where a tower might be placed, given their current mouse position
    */
    private void HoverPlacement(Vector3 mousePosition)
    {
        var finalPosition = grid.GetNearestPointOnGrid(mousePosition);
        //finalPosition.z = -1;

        MapTile temp_tile = DeployTools.SearchTiles(finalPosition, grid.GameMap.Map_Tiles);

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

    private void CreateTower(Vector3 clickPoint)
    {
        var finalPosition = grid.GetNearestPointOnGrid(clickPoint);
        //GameObject.CreatePrimitive(PrimitiveType.Cube).transform.position = finalPosition;

        MapTile temp_tile = DeployTools.SearchTiles(finalPosition, grid.GameMap.Map_Tiles);

        //gets the given tile for the position that the user clicked, and checks that a tower can actually be placed there
        if (temp_tile.Type == TileType.empty)
        {
            //sets the tile, to TileType.turrent, letting the game know that a turrent has been placed there
            DeployTools.Manage_Tile_Type(finalPosition, TileType.turret, grid.GameMap.Map_Tiles);

            if (grid.GameMap.Map_Towers != null)
            {
                switch (build_type)
                {
                    case TowerType.Annihilator:
                        grid.GameMap.Map_Towers.Add(new Annihilator(finalPosition));
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
        else
        {
            //add code to display overlay, if tower cannot be placed on it
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
}
