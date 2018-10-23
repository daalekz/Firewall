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

        AimLine = new GameObject();
        AimLine.AddComponent<LineRenderer>();
        AimLine.GetComponent<LineRenderer>().SetPosition(1, Position);
        AimLine.GetComponent<LineRenderer>().SetPosition(0, Position);

        AimLine.name = "Tower Aim Line";
        //aim_line.transform.parent = this.TowerObj.transform;
    }

    public override void Render()
    {
        if (selected_unit == null)
        {
            AimLine.GetComponent<LineRenderer>().SetPosition(1, new Vector3(0, 0, 0));
            AimLine.GetComponent<LineRenderer>().SetPosition(0, new Vector3(0, 0, 0));
        }

        if (rendered_tower == null)
        {
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
            this.TowerObj.name = "Scanner Tower";
        }

        if (tower_gun == null)
        {
            tower_gun = TowerTools.Instantiate(deploy_instance.tower_scanner_turret);
            tower_gun.transform.position = Position;
            //rendered_tower.GetComponent<Renderer>().sortingOrder = 2;
            tower_gun.transform.position = new Vector3
                (
                    tower_gun.transform.position.x,
                    tower_gun.transform.position.y,
                    -3
                );
        }

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

    public override bool Fire(List<GameObject> enemy_queue)
    {
        //local fields store the closest game object and the distance of the object from the turret
        selected_unit = null;

        SelectLast = false;
        GameObject[] navPoints = new GameObject[0];

        Shoot_Wait_Remaining -= Time.deltaTime;
        Display_Shot_Remaining-= Time.deltaTime;

        if (Display_Shot_Remaining <= 0)
        {
            AimLine.GetComponent<LineRenderer>().SetPosition(1, new Vector3(0, 0, 0));
            AimLine.GetComponent<LineRenderer>().SetPosition(0, new Vector3(0, 0, 0));

            Display_Shot_Remaining = display_shot;
        }

        if (Shoot_Wait_Remaining <= 0f)
        {
            Target(enemy_queue);
            //if the selected_towers_unit has been set to an actually gameobject instance
            //then a line is drawn between the turret and the gameobject

            if (selected_unit != null)
            {
                if (closest_distance <= Range)
                {
                    if (Vector3.Distance(Position, selected_unit.transform.position) != closest_distance)
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        return false;
    }

    public override void Attack(List<GameObject> enemy_queue)
    {
        if (Fire(enemy_queue) && Active)
        {
            DrawLine(Position, selected_unit.transform.position, Color.green);
            Shoot_Wait_Remaining = 1 / FireRate;

            Vector3 temp;
            temp = new Vector3(selected_unit.transform.position.x, selected_unit.transform.position.y, Position.z);

            TowerRotation = temp - Position;

            if (selected_unit.GetComponent<AIController>().data.Speed > 0.25)
            {
                selected_unit.GetComponent<AIController>().data.ReduceSpeed(2);
            }

            closest_distance = 0;
        }
        else
        {
            selected_unit = null;
            closest_distance = 0;
            AimLine.GetComponent<LineRenderer>().SetPosition(1, Position);
        }
    }
}
