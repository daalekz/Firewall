using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Defender : Tower
{

    public Defender(Vector3 position) : base (position, TowerType.Defender)
    {
        FireRate = 0.5f;
        Range = 6;
        Damage = 5;

        rendered_tower = TowerTools.Instantiate(deploy_instance.tower_defender, Position, Quaternion.identity);
        Color = rendered_tower.GetComponent<SpriteRenderer>().color;

        rendered_tower.transform.position = Position;
        //rendered_tower.GetComponent<Renderer>().sortingOrder = 2;
        rendered_tower.transform.position = new Vector3
            (
                rendered_tower.transform.position.x,
                rendered_tower.transform.position.y,
                -2
            );

        AimLine = new GameObject();
        AimLine.AddComponent<LineRenderer>();
        AimLine.name = "Tower Aim Line";
        this.TowerObj.name = "Defender Tower";

        //aim_line.transform.parent = this.TowerObj.transform;
    }
}
