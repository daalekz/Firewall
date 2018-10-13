using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

/*
Class taken and modified from:
https://unity3d.college/2017/10/08/simple-unity3d-snap-grid-system/
*/
public class Grid_Setup : MonoBehaviour {
    private float size = 1f;
    private List<MapTile> tiles = new List<MapTile>();

    GameController gc;
    private Map game_map;

    // Use this for initialization
    void Start() {
        gc = GameController.instance;
        game_map = GetMapData();
        gc.navPoints = GetNavPoints();

        OnDraw(game_map);
    }
    
    // Update is called once per frame
    void Update() {
    }

    public GameObject[] GetNavPoints()
    {
        string file_path = "Assets/Data/map_a_path.txt";
        int array_size, x, y, i;
        GameObject[] NavPoints;
        GameObject parent_nav_points = GameObject.Find("NavPoints");


        /*
         * Note regarding file structure:
         * First Line: Array Size
         * 2nds - EOF: x, y, co-ordinates alternating
        */

        using (StreamReader sr = new StreamReader(file_path, true))
        {
            array_size = Convert.ToInt32(sr.ReadLine());
            NavPoints = new GameObject[array_size];
            BoxCollider2D boxcollider = gameObject.AddComponent<BoxCollider2D>();
            boxcollider.isTrigger = true;
            boxcollider.transform.localScale = new Vector2(0.01f, 0.01f);


            for (i = 0; i < array_size; i++)
            {
                GameObject temp_nav = new GameObject();
                x = Convert.ToInt32(sr.ReadLine());
                y = Convert.ToInt32(sr.ReadLine());


                if (x == 0)
                {
                    temp_nav.AddComponent<Spawner>();

                }
                else
                {
                    boxcollider = temp_nav.AddComponent<BoxCollider2D>();
                    boxcollider.isTrigger = true;

                }

                temp_nav.transform.position = new Vector3(x, y, 0);
                temp_nav.transform.localScale = new Vector3(0.01f, 0.01f, 1);
                temp_nav.name = "NavPoint";


                temp_nav.transform.parent = parent_nav_points.transform;

                NavPoints[i] = temp_nav;
            }
            return NavPoints;
        }
    }
    

   //reads through a data file to create the game map
    public Map GetMapData()
    {
        string file_path = "Assets/Data/map_a.txt";

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
        
        GameObject parent_path = GameObject.Find("Path");

        parent_path.transform.localScale = new Vector3(map.Width, map.Height, 1);

        foreach (MapTile tile in map.Map_Tiles)
        {
            GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            
            cube.transform.position = tile.Position;
            cube.GetComponent<Renderer>().sortingOrder = 1;
            cube.transform.localScale = new Vector3(1f, 1f, 0.1f);

            //draws differently colored squares depending on the enum value
            switch (tile.Type)
            {
                case (TileType.empty):
                   cube.GetComponent<Renderer>().material.color = Color.white;
                    break;

                case (TileType.path):
                    cube.GetComponent<Renderer>().material.color = Color.red;
                    cube.transform.parent = parent_path.transform;
                    cube.name = "MapTile";

                    break;

                case (TileType.terrain):
                    cube.GetComponent<Renderer>().material.color = Color.green;
                    cube.name = "TerrainTile";
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
