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

        tower_gun = TowerTools.Instantiate(deploy_instance.tower_defender_turret);
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
        this.TowerObj.name = "Defender Tower";

        //aim_line.transform.parent = this.TowerObj.transform;
    }

    public override void Render()
    {
        base.Render();

        float speed = 5;
        // The step size is equal to speed times frame time.
        Vector3 dir, current, target;

        if (TowerRotation != null)
        {
            //code taken and modified from: https://answers.unity.com/questions/650460/rotating-a-2d-sprite-to-face-a-target-on-a-single.html

            float angle = Mathf.Atan2(-TowerRotation.x, TowerRotation.y) * Mathf.Rad2Deg;
            
            Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
            tower_gun.transform.rotation = Quaternion.Slerp(tower_gun.transform.rotation, q, Time.deltaTime * speed);

        }
        else
        {
            tower_gun.transform.rotation = Quaternion.LookRotation(Position - Position, Vector3.up);
        }
    }
}
