using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System;
using System.Linq;

public class TestsAim {
    public List<GameObject> enemy_objects;
    public GameObject enemy;

    [Test]
    public void DefaultTowerAimTest()
    {
        //applicable for defender and scanner

        //test data
        enemy_objects = new List<GameObject>();
        enemy = new GameObject();
        //enemy1
        enemy.transform.position = new Vector3(2, 30, 0);
        enemy_objects.Add(enemy);
        //enemy2
        enemy.transform.position = new Vector3(10, 12, 0);
        enemy_objects.Add(enemy);
        //enemy3
        enemy.transform.position = new Vector3(12, 5, 0);
        enemy_objects.Add(enemy);

        //list of enemies being tested
        Vector3 tower_pos = new Vector3(15, 15, 0);
        
        Defender test_tower = new Defender(tower_pos);

        GameObject selected_enemy;
        test_tower.Target(enemy_objects);
        selected_enemy = test_tower.selected_unit;
        Assert.That(selected_enemy == enemy_objects[1]);
    }

    //NEED TO FIX
    [Test]
    public void IsolatorTowerAimTest()
    {
        //NAVPOINTS
        GameObject[] navPointsArray = new GameObject[4]; // Array of empty game objects used as nodes to navigate between || FIX

        int i;
        GameObject temp_nav;

        //create NavPoints
        BoxCollider2D boxcollider;

        for (i = 0; i < navPointsArray.Length; i++)
        {
            temp_nav = new GameObject();
            if (i == 0)
            {
                temp_nav.AddComponent<Spawner>();
            }
            else
            {
                if (i < (navPointsArray.Length - 2))
                {
                    boxcollider = temp_nav.AddComponent<BoxCollider2D>();
                    boxcollider.isTrigger = true;
                }
            }

            switch (i)
            {
                case 0:
                    temp_nav.transform.position = new Vector3(0, 30, 0);
                    break;

                case 1:
                    temp_nav.transform.position = new Vector3(10, 30, 0);
                    break;

                case 2:
                    temp_nav.transform.position = new Vector3(10, 5, 0);
                    break;

                case 3:
                    temp_nav.transform.position = new Vector3(30, 5, 0);
                    break;
            }

            navPointsArray[i] = temp_nav;
        }

        GameController gc = new GameController();
        gc.navPoints.Add(navPointsArray);

        //test data
        enemy_objects = new List<GameObject>();
        enemy = new GameObject();
        //enemy1
        enemy.transform.position = new Vector3(2, 30, 0);
        enemy.AddComponent<AIController>();
        enemy.GetComponent<AIController>().data = new DDoS();
        
        enemy_objects.Add(enemy);
        //enemy2
        enemy.transform.position = new Vector3(12, 12, 0);
        enemy.GetComponent<AIController>().NavPointNum = 2;

        enemy_objects.Add(enemy);
        //enemy3
        enemy.transform.position = new Vector3(14, 5, 0);
        enemy_objects.Add(enemy);

        //list of enemies being tested

        //need to set the nav points somehow 
        Vector3 tower_pos = new Vector3(15, 15, 0);
        Isolator test_tower = new Isolator(tower_pos);
        
        test_tower.Target(enemy_objects);
        Assert.That(test_tower.selected_unit == enemy_objects[3]);
    }

    [Test]
    public void AnnihilatorTowerAimTest()
    {
        List<GameObject> expected_towers = new List<GameObject>();
        enemy_objects = new List<GameObject>();
        enemy = new GameObject();
        //enemy1
        enemy.transform.position = new Vector3(2, 30, 0);
        enemy_objects.Add(enemy);
        //enemy2
        GameObject enemy1 = new GameObject();
        enemy1.transform.position = new Vector3(13, 5, 0);
        enemy_objects.Add(enemy1);
        expected_towers.Add(enemy1);
        //enemy3
        GameObject enemy2 = new GameObject();
        enemy2.transform.position = new Vector3(12, 5, 0);
        enemy_objects.Add(enemy2);
        expected_towers.Add(enemy2);

        Vector3 tower_pos = new Vector3(15, 5, 0);
        Annihilator test_tower = new Annihilator(tower_pos, false);

        test_tower.TargetRow(enemy_objects);
        List<GameObject> returned_towers = test_tower.TargetedTowers;

        Assert.That(test_tower.TargetedTowers.SequenceEqual(expected_towers));
    }

    // A UnityTest behaves like a coroutine in PlayMode
    // and allows you to yield null to skip a frame in EditMode
    [UnityTest]
    public IEnumerator TowerTestsWithEnumeratorPasses() {
        // Use the Assert class to test conditions.
        // yield to skip a frame
        yield return null;
    }
}

public class TestFire
{
    [Test]
    public void ScannerTestOutsideRange()
    {
        GameObject enemy = new GameObject();
        enemy.transform.position = new Vector3(10, 5, 0);

        List<GameObject> enemy_list = new List<GameObject>();
        enemy_list.Add(enemy);

        Scanner test_tower = new Scanner(new Vector3(7, 28, 0));

        Assert.That(!test_tower.Fire(enemy_list));
    }
    
    [Test]
    public void ScannerTestOnRangeBoarder()
    {
        GameObject enemy = new GameObject();
        enemy.transform.position = new Vector3(10, 28, 0);

        List<GameObject> enemy_list = new List<GameObject>();
        enemy_list.Add(enemy);

        Scanner test_tower = new Scanner(new Vector3(7, 28, 0));

        Assert.That(test_tower.Fire(enemy_list));
    }

    [Test]
    public void ScannerTestClearlyInsideRange()
    {
        GameObject enemy = new GameObject();
        enemy.transform.position = new Vector3(7, 30, 0);

        List<GameObject> enemy_list = new List<GameObject>();
        enemy_list.Add(enemy);

        Scanner test_tower = new Scanner(new Vector3(7, 28, 0));

        Assert.That(test_tower.Fire(enemy_list));
    }

    [Test]
    public void DefenderTestOutsideRange()
    {
        GameObject enemy = new GameObject();
        enemy.transform.position = new Vector3(10, 5, 0);

        List<GameObject> enemy_list = new List<GameObject>();
        enemy_list.Add(enemy);

        Defender test_tower = new Defender(new Vector3(4, 28, 0));

        Assert.That(!test_tower.Fire(enemy_list));
    }

    [Test]
    public void DefenderTestOnRangeBoarder()
    {
        GameObject enemy = new GameObject();
        enemy.transform.position = new Vector3(10, 28, 0);

        List<GameObject> enemy_list = new List<GameObject>();
        enemy_list.Add(enemy);

        Defender test_tower = new Defender(new Vector3(4, 28, 0));

        Assert.That(test_tower.Fire(enemy_list));
    }

    [Test]
    public void DefenderTestClearlyInsideRange()
    {
        GameObject enemy = new GameObject();
        enemy.transform.position = new Vector3(4, 30, 0);

        List<GameObject> enemy_list = new List<GameObject>();
        enemy_list.Add(enemy);

        Defender test_tower = new Defender(new Vector3(4, 28, 0));

        Assert.That(test_tower.Fire(enemy_list));
    }

    //only create line!
    [Test]
    public void TestAimLinePositions()
    {
        GameObject enemy = new GameObject();
        enemy.AddComponent<AIController>();
        enemy.GetComponent<AIController>().data = new DDoS();

        enemy.transform.position = new Vector3(4, 28, 0);

        List<GameObject> enemy_list = new List<GameObject>();
        enemy_list.Add(enemy);

        Defender test_tower = new Defender(new Vector3(4, 28, 0));

        test_tower.Fire(enemy_list);

        Debug.Log(test_tower.AimLine.GetComponent<LineRenderer>().GetPosition(1));
        Debug.Log(enemy.transform.position);

        Assert.That(test_tower.AimLine.GetComponent<LineRenderer>().GetPosition(0) == test_tower.Position && test_tower.AimLine.GetComponent<LineRenderer>().GetPosition(1) == enemy.transform.position);
    }
}

public class TestTowerDataManipulation
{
    //might not be able to test
    [Test]
    public void TestSetTowerInactive()
    {
        
    }


    [Test]
    public void TestSetTowerActive()
    {

    }
}

public class TestTowerPlacement
{
    [Test]
    public void MousePointConversionToGrid()
    {
        Vector3 input_pos = new Vector3(1.2f, 1.7f, 0);
        Vector3 expected_pos = new Vector3(1, 2, 0);

        GameObject temp_grid = new GameObject();
        temp_grid.AddComponent<Grid_Setup>();

        temp_grid.transform.position = new Vector3(0, 0, 0);

        Vector3 actual_pos = temp_grid.GetComponent<Grid_Setup>().GetNearestPointOnGrid(input_pos);
        Assert.That(actual_pos == expected_pos);
    }

    //tower placement
    [Test]
    public void PlaceTowerOnBlank()
    {
        GameObject temp_grid = new GameObject();
        temp_grid.AddComponent<Grid_Setup>();

        temp_grid.transform.position = new Vector3(0, 0, 0);


        Map test_map = new Map(4, 2, 2);

        MapTile tile1 = new MapTile(new Vector3(0, 0, 0));
        tile1.Type = TileType.path;

        MapTile tile2 = new MapTile(new Vector3(1, 0, 0));
        tile2.Type = TileType.turret;

        MapTile tile3 = new MapTile(new Vector3(0, 1, 0));
        tile3.Type = TileType.empty;

        MapTile tile4 = new MapTile(new Vector3(1, 1, 0));
        tile4.Type = TileType.terrain;

        test_map.Map_Tiles.Add(tile1);
        test_map.Map_Tiles.Add(tile2);
        test_map.Map_Tiles.Add(tile3);
        test_map.Map_Tiles.Add(tile4);

        temp_grid.GetComponent<Grid_Setup>().GameMap = test_map;

        GameObject deploy_test_obj = new GameObject();
        deploy_test_obj.AddComponent<Deploy>();
        deploy_test_obj.GetComponent<Deploy>().Grid = temp_grid.GetComponent<Grid_Setup>();
        
        Assert.That(deploy_test_obj.GetComponent<Deploy>().TileEmpty(tile3.Position));
    }

    [Test]
    public void PlaceTowerOnPath()
    {
        GameObject temp_grid = new GameObject();
        temp_grid.AddComponent<Grid_Setup>();

        temp_grid.transform.position = new Vector3(0, 0, 0);


        Map test_map = new Map(4, 2, 2);

        MapTile tile1 = new MapTile(new Vector3(0, 0, 0));
        tile1.Type = TileType.path;

        MapTile tile2 = new MapTile(new Vector3(1, 0, 0));
        tile2.Type = TileType.turret;

        MapTile tile3 = new MapTile(new Vector3(0, 1, 0));
        tile3.Type = TileType.empty;

        MapTile tile4 = new MapTile(new Vector3(1, 1, 0));
        tile4.Type = TileType.terrain;

        test_map.Map_Tiles.Add(tile1);
        test_map.Map_Tiles.Add(tile2);
        test_map.Map_Tiles.Add(tile3);
        test_map.Map_Tiles.Add(tile4);

        temp_grid.GetComponent<Grid_Setup>().GameMap = test_map;

        GameObject deploy_test_obj = new GameObject();
        deploy_test_obj.AddComponent<Deploy>();
        deploy_test_obj.GetComponent<Deploy>().Grid = temp_grid.GetComponent<Grid_Setup>();

        Assert.That(!deploy_test_obj.GetComponent<Deploy>().TileEmpty(tile1.Position));
    }

    [Test]
    public void PlaceTowerOnTurret()
    {
        GameObject temp_grid = new GameObject();
        temp_grid.AddComponent<Grid_Setup>();

        temp_grid.transform.position = new Vector3(0, 0, 0);


        Map test_map = new Map(4, 2, 2);

        MapTile tile1 = new MapTile(new Vector3(0, 0, 0));
        tile1.Type = TileType.path;

        MapTile tile2 = new MapTile(new Vector3(1, 0, 0));
        tile2.Type = TileType.turret;

        MapTile tile3 = new MapTile(new Vector3(0, 1, 0));
        tile3.Type = TileType.empty;

        MapTile tile4 = new MapTile(new Vector3(1, 1, 0));
        tile4.Type = TileType.terrain;

        test_map.Map_Tiles.Add(tile1);
        test_map.Map_Tiles.Add(tile2);
        test_map.Map_Tiles.Add(tile3);
        test_map.Map_Tiles.Add(tile4);

        temp_grid.GetComponent<Grid_Setup>().GameMap = test_map;

        GameObject deploy_test_obj = new GameObject();
        deploy_test_obj.AddComponent<Deploy>();
        deploy_test_obj.GetComponent<Deploy>().Grid = temp_grid.GetComponent<Grid_Setup>();

        Assert.That(!deploy_test_obj.GetComponent<Deploy>().TileEmpty(tile2.Position));
    }

    [Test]
    public void PlaceTowerOnTerrain()
    {
        GameObject temp_grid = new GameObject();
        temp_grid.AddComponent<Grid_Setup>();

        temp_grid.transform.position = new Vector3(0, 0, 0);


        Map test_map = new Map(4, 2, 2);

        MapTile tile1 = new MapTile(new Vector3(0, 0, 0));
        tile1.Type = TileType.path;

        MapTile tile2 = new MapTile(new Vector3(1, 0, 0));
        tile2.Type = TileType.turret;

        MapTile tile3 = new MapTile(new Vector3(0, 1, 0));
        tile3.Type = TileType.empty;

        MapTile tile4 = new MapTile(new Vector3(1, 1, 0));
        tile4.Type = TileType.terrain;

        test_map.Map_Tiles.Add(tile1);
        test_map.Map_Tiles.Add(tile2);
        test_map.Map_Tiles.Add(tile3);
        test_map.Map_Tiles.Add(tile4);

        temp_grid.GetComponent<Grid_Setup>().GameMap = test_map;

        GameObject deploy_test_obj = new GameObject();
        deploy_test_obj.AddComponent<Deploy>();
        deploy_test_obj.GetComponent<Deploy>().Grid = temp_grid.GetComponent<Grid_Setup>();

        Assert.That(!deploy_test_obj.GetComponent<Deploy>().TileEmpty(tile4.Position));
    }
}