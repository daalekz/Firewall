using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//class is a number of static methods that can used for data processing (i.e. not object specific) 
public class DeployTools
{
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


    public static void Manage_Tile_Type(Vector3 Search_Position, TileType change_type, List<MapTile> Search_List)
    {
        foreach (MapTile tile in Search_List)
        {
            if (tile.Position == Search_Position)
            {
                tile.Type = change_type;
                return;
            }
        }
    }

    public static Tower SelectTower(List<Tower> towers, Vector3 Search_Position)
    {
        foreach (Tower tower in towers)
        {
            if (tower.Position.x == Search_Position.x && tower.Position.y == Search_Position.y)
            {
                return tower;
            }
        }

        return null;
    }

    public static Tower GetTower(List<Tower> towers, Tower selected_tower)
    {
        foreach (Tower tower in towers)
        {
            if (selected_tower == tower)
            {
                return tower;
            }
        }

        return null;
    }
}