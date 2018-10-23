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

    public GameObject hover_tower;
    public GameObject hover_gun;

    //needs to get the data from camera and grid (hence they are passed into script via the object)
    public Grid_Setup grid;
    public Camera cam;
    public static Tower selected_tower;
    public static bool moving_tower;

    private GameObject hover_sphere;
    private TowerType hover_type;
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
    public GameObject tower_scanner_turret;

    public RaycastHit hitInfo;
    public Ray ray;

    //initalized necessary variables, objects and other data
    private void Awake()
    {
        if (instance != null) throw new System.Exception();
        instance = this;

        grid = FindObjectOfType<Grid_Setup>();

        selected_tower = null;
        build_type = TowerType.Isolator;
    }

    void Start ()
    {
        igmc = InGameMenuController.instance;
    }

    private void Update()
    {
        if (hover_type != build_type)
        {
            Destroy(hover_tower);
            Destroy(hover_gun);
            switch (build_type)
            {
                case TowerType.Annihilator:
                    hover_tower = TowerTools.Instantiate(tower_annihilator, new Vector3(0, 0, 0), Quaternion.identity);
                    hover_gun = TowerTools.Instantiate(tower_annihilator_turret, new Vector3(0, 0, 0), Quaternion.identity);
                    hover_type = TowerType.Annihilator;
                    break;

                case TowerType.Defender:
                    hover_tower = TowerTools.Instantiate(tower_defender, new Vector3(0, 0, 0), Quaternion.identity);
                    hover_gun = TowerTools.Instantiate(tower_defender_turret, new Vector3(0, 0, 0), Quaternion.identity);

                    hover_type = TowerType.Defender;
                    break;

                case TowerType.Extractor:
                    hover_tower = TowerTools.Instantiate(tower_extractor, new Vector3(0, 0, 0), Quaternion.identity);
                    hover_gun = TowerTools.Instantiate(tower_extractor_turret, new Vector3(0, 0, 0), Quaternion.identity);

                    hover_type = TowerType.Extractor;
                    break;

                case TowerType.Isolator:
                    hover_tower = TowerTools.Instantiate(tower_isolator, new Vector3(0, 0, 0), Quaternion.identity);
                    hover_gun = TowerTools.Instantiate(tower_isolator_turret, new Vector3(0, 0, 0), Quaternion.identity);

                    hover_type = TowerType.Isolator;
                    break;

                case TowerType.Scanner:
                    hover_tower = TowerTools.Instantiate(tower_scanner, new Vector3(0, 0, 0), Quaternion.identity);
                    hover_gun = TowerTools.Instantiate(tower_scanner_turret, new Vector3(0, 0, 0), Quaternion.identity);

                    hover_type = TowerType.Scanner;
                    break;
            }
            var tower_color = hover_tower.GetComponent<SpriteRenderer>().color;
            tower_color.a = 0.5f;

            var gun_color = hover_tower.GetComponent<SpriteRenderer>().color;
            gun_color.a = 0.5f;

            hover_tower.SetActive(false);
            hover_gun.SetActive(false);
        }


        ray = cam.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hitInfo))
        {
            var finalPosition = grid.GetNearestPointOnGrid(hitInfo.point);
            finalPosition.z = 0;

            //draws tower
            if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject() && !moving_tower)
            {
                if (TileEmpty(finalPosition)) {
                    CreateTower(finalPosition);
                }
            }

            //selects a tower
            if (Input.GetMouseButtonDown(0) && Input.GetKey(KeyCode.LeftControl) && !moving_tower)
            {
                SelectTower(finalPosition);
            }

            if (selected_tower != null && moving_tower && Input.GetMouseButtonDown(0))
            {
                if (TileEmpty(finalPosition))
                {
                    RepositionTower(finalPosition);
                }
            }
            //renders hover placement for a given tile
            HoverPlacement(finalPosition);
        }
        else
        {
            hover_tower.SetActive(false);
            hover_gun.SetActive(false);
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

	public void SetBuildType(int type)
	{
		build_type = (TowerType)type;
        igmc.ToggleMenu(); // Close menu upon selecting a tower
	}

    /*
     * Summary:
     * Displays an overlay, to show user where a tower might be placed, given their current mouse position
    */
    private void HoverPlacement(Vector3 InputPosition)
    {
        Vector3 mousePosition = InputPosition;
        MapTile temp_tile = DeployTools.SearchTiles(InputPosition, grid.GameMap.Map_Tiles);

        //checks that the value returned, is actually a tile
        if (temp_tile == null)
        {
            hover_tower.transform.localScale = new Vector3(0, 0, 0);
            hover_gun.transform.localScale = new Vector3(0, 0, 0);
            return;
        }

        switch (temp_tile.Type)
        {
            //check that the tile is a of tiletype empty
            //if tile type isn't type empty, a tower won't be able to be placed there
            case TileType.empty:
                //makes the hover_sphere game object visible
                hover_tower.SetActive(true);
                hover_gun.SetActive(true);
                //sets object position to that as the underlying tile
                hover_tower.transform.position = mousePosition;
                hover_tower.transform.position = new Vector3(hover_tower.transform.position.x, hover_tower.transform.position.y, -1);

                hover_tower.GetComponent<SpriteRenderer>().color = Color.white;
                hover_gun.GetComponent<SpriteRenderer>().color = Color.white;
                
                hover_gun.transform.position = mousePosition;
                hover_gun.transform.position = new Vector3(hover_gun.transform.position.x, hover_gun.transform.position.y, -2);
                hover_gun.GetComponent<Renderer>().material.color = Color.white;

                break;

            case TileType.turret:
                hover_tower.SetActive(false);
                hover_gun.SetActive(false);
                break;

            default:
                //makes the hover_sphere game object visible
                hover_tower.SetActive(true);
                //sets object position to that as the underlying tile
                hover_tower.transform.position = mousePosition;
                hover_tower.transform.position = new Vector3(hover_tower.transform.position.x, hover_tower.transform.position.y, -1);
                hover_tower.GetComponent<SpriteRenderer>().color = Color.red;


                //makes the hover_sphere game object visible
                hover_gun.SetActive(true);
                //sets object position to that as the underlying tile
                hover_gun.transform.position = mousePosition;
                hover_gun.transform.position = new Vector3(hover_gun.transform.position.x, hover_gun.transform.position.y, -2);
                hover_gun.GetComponent<SpriteRenderer>().material.color = Color.red;
                hover_gun.GetComponent<SpriteRenderer>().color = Color.red;

                break;
        }
    }

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

    public void CreateTower(Vector3 finalPosition)
    {
        //GameObject.CreatePrimitive(PrimitiveType.Cube).transform.position = finalPosition;
        //sets the tile, to TileType.turrent, letting the game know that a turrent has been placed there
        DeployTools.Manage_Tile_Type(finalPosition, TileType.turret, grid.GameMap.Map_Tiles);

        if (grid.GameMap.Map_Towers != null)
        {
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

    public Grid_Setup Grid
    {
        get
        {
            return grid;
        }

        set
        {
            grid = value;
        }
    }
}
