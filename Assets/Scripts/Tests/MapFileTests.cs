using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System;
using System.Linq;

public class MapFileTests {

    /*
    Description: 
    Tests that the file reading of map data works, collecting the right enum data from the file
    */
    [Test]
    public void ReadMapData()
    {
        Map output_map;
        GameObject grid_test_obj = new GameObject();
        List<GameObject[]> output_points;

        grid_test_obj.AddComponent<Grid_Setup>();
        grid_test_obj.GetComponent<Grid_Setup>().transform.position = new Vector3(0, 0, 0);
        output_map = grid_test_obj.GetComponent<Grid_Setup>().GetMapData("Assets/Data/Test/test_map.txt");
        output_points = grid_test_obj.GetComponent<Grid_Setup>().GetNavPoints("Assets/Data/Test/test_map_path.txt");

        Assert.That(
            output_map.Map_Tiles[0].Type == TileType.empty
            &&
            output_map.Map_Tiles[1].Type == TileType.empty
            &&
            output_map.Map_Tiles[2].Type == TileType.empty
            &&
            output_map.Map_Tiles[3].Type == TileType.path
            &&
            output_map.Map_Tiles[4].Type == TileType.path
            &&
            output_map.Map_Tiles[5].Type == TileType.terrain
            &&
            output_map.Map_Tiles[6].Type == TileType.empty
            &&
            output_map.Map_Tiles[7].Type == TileType.path
            &&
            output_map.Map_Tiles[8].Type == TileType.empty
            &&
            output_map.Map_Tiles[9].Type == TileType.terrain
            &&
            output_map.Map_Tiles[10].Type == TileType.path
            &&
            output_map.Map_Tiles[11].Type == TileType.path
        );
    }

    /*
    Description: 
    Tests that the Nav data file is correct read
    checks if the positions inputted are collected and translated properly for the game map
    */
    [Test]
    public void ReadNavData()
    {
        List<GameObject[]> output_points;
        GameObject grid_test_obj = new GameObject();

        grid_test_obj.AddComponent<Grid_Setup>();
        grid_test_obj.GetComponent<Grid_Setup>().transform.position = new Vector3(0, 0, 0);
        grid_test_obj.GetComponent<Grid_Setup>().GetMapData("Assets/Data/Test/test_map.txt");
        output_points = grid_test_obj.GetComponent<Grid_Setup>().GetNavPoints("Assets/Data/Test/test_map_path.txt");

        Assert.That(
            output_points[0][0].transform.position == new Vector3(-1, 1, 0)
            &&
            output_points[0][1].transform.position == new Vector3(0, 1, 0)
            &&
            output_points[0][2].transform.position == new Vector3(0, -1, 0)
            &&
            output_points[0][3].transform.position == new Vector3(1, -1, 0)
            );
    }

    //need to test if the correct componets were added (i.e. spawner for tile 0, etc and no box collider for last tile)


    /*
    Description: 
    Tests that the Nav data file is correct read
    checks if the positions inputted are collected and translated properly for the game map
    */
    [Test]
    public void ReadMultiNavData()
    {
        List<GameObject[]> output_points;
        GameObject grid_test_obj = new GameObject();

        grid_test_obj.AddComponent<Grid_Setup>();
        grid_test_obj.GetComponent<Grid_Setup>().transform.position = new Vector3(0, 0, 0);
        grid_test_obj.GetComponent<Grid_Setup>().GetMapData("Assets/Data/Test/test_map_b.txt");
        output_points = grid_test_obj.GetComponent<Grid_Setup>().GetNavPoints("Assets/Data/Test/test_map_path_b.txt");

        foreach (GameObject point in output_points[1])
        {
            Debug.Log(point.transform.position.x + " | " + point.transform.position.y);
        }

        Assert.That(
            output_points[0][0].transform.position == new Vector3(-1, 1, 0)
            &&
            output_points[0][1].transform.position == new Vector3(0, 1, 0)
            &&
            output_points[0][2].transform.position == new Vector3(0, -1, 0)
            &&
            output_points[0][3].transform.position == new Vector3(1, -1, 0)
            &&
            output_points[1][0].transform.position == new Vector3(-1, 1, 0)
            &&
            output_points[1][1].transform.position == new Vector3(0, 1, 0)
            &&
            output_points[1][2].transform.position == new Vector3(1, 1, 0)
        );

    //have tests for multiple paths (if implementing!)
    }


    /*
    Description: 
    Tests that the Nav data file is correct read
    checks if the positions inputted are collected and translated properly for the game map
    */
    [Test]
    public void ReadCheckNavPointComponents()
    {
        List<GameObject[]> output_points;
        GameObject grid_test_obj = new GameObject();

        grid_test_obj.AddComponent<Grid_Setup>();
        grid_test_obj.GetComponent<Grid_Setup>().transform.position = new Vector3(0, 0, 0);
        grid_test_obj.GetComponent<Grid_Setup>().GetMapData("Assets/Data/Test/test_map.txt");
        output_points = grid_test_obj.GetComponent<Grid_Setup>().GetNavPoints("Assets/Data/Test/test_map_path.txt");

        Assert.That(
            output_points[0][0].GetComponent<Spawner>() != null
            &&
            output_points[0][1].GetComponent<BoxCollider2D>() != null
            &&
            output_points[0][2].GetComponent<BoxCollider2D>() != null 
            &&
            output_points[0][3].GetComponent<BoxCollider2D>() == null
            );
    }
}


//add test were error outputted if number of points specificied in file don't match actual number of points


//see if we can test the actual path choice and target navpoint selection 