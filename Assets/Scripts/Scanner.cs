using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scanner : Tower
{

    public Scanner(Vector3 position) : base(position, TowerType.Scanner)
    {
        Damage = 10;
        FireRate = 0.33f;
        Range = 3;

        rendered_tower = TowerTools.Instantiate(deploy_instance.tower_scanner, Position, Quaternion.identity);
        Color = rendered_tower.GetComponent<SpriteRenderer>().color;
        this.TowerObj.name = "Tower";

        rendered_tower.transform.position = Position;
        //rendered_tower.GetComponent<Renderer>().sortingOrder = 2;
        rendered_tower.transform.position = new Vector3
            (
                rendered_tower.transform.position.x,
                rendered_tower.transform.position.y,
                -2
            );

        tower_gun = TowerTools.Instantiate(deploy_instance.tower_scanner_tower);
        tower_gun.transform.position = Position;
        //rendered_tower.GetComponent<Renderer>().sortingOrder = 2;
        tower_gun.transform.position = new Vector3
            (
                tower_gun.transform.position.x,
                tower_gun.transform.position.y,
                -3
            );

        AimLine = new GameObject();
        AimLine.AddComponent<LineRenderer>();
        AimLine.name = "Tower Aim Line";
        //aim_line.transform.parent = this.TowerObj.transform;
        this.TowerObj.name = "Scanner Tower";
    }

}
