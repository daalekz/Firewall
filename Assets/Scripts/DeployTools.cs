using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//class is a number of static methods that can used for data processing (i.e. not object specific) 
public class DeployTools
{
    //searches through all all of the towers in the game, until one with a match position (or a null) is returned
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

    //helps to modify the tile types of specific user selected tiles
    public static void Manage_Tile_Type(Vector3 Search_Position, TileType change_type, List<MapTile> Search_List)
    {
        foreach (MapTile tile in Search_List)
        {
            if (tile.Position == Search_Position)
            if (tile.Position == Search_Position)
            {
                tile.Type = change_type;
                return;
            }
        }
    }

    //selects a tower, with a inputted co-ordinates
    public static Tower SelectTower(List<Tower> game_towers, Vector3 Search_Position)
    {
        foreach (Tower tower in game_towers)
        {
            if (tower.Position.x == Search_Position.x && tower.Position.y == Search_Position.y)
            {
                return tower;
            }

        }
        return null;
    }

    //gets the actuall tower from a list, given a tower input
    //this is used for directly referencing a specific tower utilized by the game
    //not modifying a copy of it, but the data for the tower's given memory address
    public static Tower GetTower(List<Tower> towers, Tower selected_towers_tower)
    {
        foreach (Tower tower in towers)
        {
            if (selected_towers_tower == tower)
            {
                return tower;
            }
        }
        return null;
    }
}