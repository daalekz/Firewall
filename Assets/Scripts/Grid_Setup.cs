using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

/*
Class taken and modified from:
https://unity3d.college/2017/10/08/simple-unity3d-snap-grid-system/
*/
public class Grid_Setup : MonoBehaviour
{
    private WaveController wc;
    private float x_start, y_start;
    GameController gc;
    private Map game_map;
    public GameObject map_tile_a;
    public GameObject map_tile_b;
    public GameObject map_tile_c;
    public GameObject map_prefab;
	public AudioController audioControllerScript;

    private float size = 1f;

    // Use this for initialization
    void Start()
    {
        wc = WaveController.instance;

        //initalized the x and y values for the translation of the map
        //so that it's centre can be and (0,0)
        x_start = 0;
        y_start = 0;
        gc = GameController.instance;
        game_map = GetMapData("Assets/Data/map_b.txt");

        gc.navPoints = GetNavPoints("Assets/Data/map_b_path.txt");
        OnDraw(game_map);

		audioControllerScript = (AudioController) GameObject.FindGameObjectWithTag("SoundEffect").GetComponent(typeof(AudioController));
	}
    
    // Update is called once per frame
    void Update() {
        foreach (Tower tower in game_map.Map_Towers)
        {
			//If a tower has shot, play the shot sound effect
			if (tower.Fire(wc.SpawnedObjects) && tower.Active)
				audioControllerScript.ShootSoundEffect();

			tower.Attack(wc.SpawnedObjects);	
		}
    }

    //gets the path points that the AI will follow for the map
    public List<GameObject[]> GetNavPoints(string file_path)
    {
        int array_size, x, y, i, j, NumPaths;
        GameObject[] NavPoints;
        List<GameObject[]> GlobalPaths = new List<GameObject[]>();

        /*
         * Note regarding file structure:
         * First Line: Number of Paths present
         * 2nd: Number of points in path
         * 3rd - (3rd + 2nd line value * 2): x, y, co-ordinates alternating
         * (looped depending on number of paths)
        */

        using (StreamReader sr = new StreamReader(file_path, true))
        {
            NumPaths = Convert.ToInt32(sr.ReadLine());

            for (j = 0; j < NumPaths; j++)
            {
                GameObject NavPoint = new GameObject();

                array_size = Convert.ToInt32(sr.ReadLine());
                //initializes the return array, with the size (based on text file line)
                NavPoints = new GameObject[array_size];
                BoxCollider2D boxcollider = gameObject.AddComponent<BoxCollider2D>();
                boxcollider.isTrigger = true;
                boxcollider.transform.localScale = new Vector2(0.01f, 0.01f);

                //assigns the first nav point a spawner script
                //asssigns the 2nd - 2nd Last Navpoints a boxcollider2d component
                //assigns the last navpoint nothing!
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
                        if (i < (array_size - 1))
                        {
                            boxcollider = temp_nav.AddComponent<BoxCollider2D>();
                            boxcollider.isTrigger = true;
                        }
                    }

                    //sets the positions (with the translation) for the navpoint
                    temp_nav.transform.position = new Vector3(x + x_start, y + y_start + 1, 0);
                    temp_nav.transform.localScale = new Vector3(0.01f, 0.01f, 1);
                    temp_nav.name = "NavPoint";

                    //assigns the navpoints to NavPoints parents element
                    temp_nav.transform.parent = NavPoint.transform;
                    NavPoint.name = "NavPoints";
                    //assigns the nav element to the ith position in the array 
                    NavPoints[i] = temp_nav;
                }

                GlobalPaths.Add(NavPoints);
            }

            //returns the array, which is assigned to an array inside the game controller 
            //(which the rest of the game then accesses)
            return GlobalPaths;
        }
    }
    
   //reads through a data file to create the game map
    public Map GetMapData(string file_path)
    {
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

            float tile_x, tile_y;

            //gets the required x and y translation, for the given length and height
            x_start = -(width / 2);
            y_start = -(height / 2);

            //creates game map that will be returned
            Map method_map = new Map(num_cells, width, height);

            //gets the tile x and y values, so when it called, it can be rendered
            for (float y = height; y > 0; y -= size)
            {
                for (float x = 0; x < width; x += size)
                {
                    tile_x = x + x_start;
                    tile_y = y + y_start;

                    var point = GetNearestPointOnGrid(new Vector3(tile_x, tile_y, 0.2f));

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
        position -= this.transform.position;

        int xCount = Mathf.RoundToInt(position.x / size);
        int yCount = Mathf.RoundToInt(position.y / size);
        int zCount = Mathf.RoundToInt(position.z / size);

        Vector3 result = new Vector3(
            (float)xCount * size,
            (float)yCount * size,
            (float)zCount * size
        );

        result += this.transform.position;

        return result;
    }

    //draws the game squares 
    void OnDraw(Map map)
    {
        System.Random rnd = new System.Random();

        GameObject cube = null;
        GameObject placement_tiles = new GameObject();
        placement_tiles.name = "Placement Tiles";

        GameObject parent_path = GameObject.Find("Path");

        parent_path.transform.localScale = new Vector3(map.Width, map.Height, 1);

        foreach (MapTile tile in map.Map_Tiles)
        {
            int random = rnd.Next(0, 3);

            //draws differently colored squares depending on the enum value
            switch (tile.Type)
            {
                case (TileType.empty):

                    cube = Instantiate(map_prefab, tile.Position, Quaternion.identity);
                    //cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    cube.transform.position = tile.Position;
                    //cube.transform.localScale = new Vector3(1f, 1f, 0.1f);
                    cube.transform.position = new Vector3(cube.transform.position.x, cube.transform.position.y, 1);
                    //cube.GetComponent<Renderer>().material.color = Color.grey;
                    cube.transform.parent = placement_tiles.transform;
                    cube.name = "PlacementTile";
                    break;

                //chooses one of three different tile designs at random (using ingame prefabs, with images attached)
                case (TileType.path):
                    switch (random)
                    {
                        case 0:
                            cube = Instantiate(map_tile_a, tile.Position, Quaternion.identity);
                            break;

                        case 1:
                            cube = Instantiate(map_tile_b, tile.Position, Quaternion.identity);
                            break;

                        case 2:
                            cube = Instantiate(map_tile_c, tile.Position, Quaternion.identity);
                            break;
                    }

                    cube.transform.position = tile.Position;
                    //cube.transform.localScale = new Vector3(1f, 1f, 0.1f);
                    cube.GetComponent<Renderer>().material.color = Color.white;
                    cube.transform.parent = parent_path.transform;
                    cube.name = "MapTile";
                    break;

                case (TileType.terrain):
                    cube = Instantiate(map_prefab, tile.Position, Quaternion.identity);

                    cube.transform.localScale = new Vector3(1f, 1f, 0.1f);

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

        set
        {
            game_map.Map_Tiles = value;
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

        set
        {
            game_map = value;
        }
    }
}
