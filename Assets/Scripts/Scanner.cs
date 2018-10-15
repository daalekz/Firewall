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
        AimLine.GetComponent<LineRenderer>().SetPosition(1, Position);
        AimLine.GetComponent<LineRenderer>().SetPosition(0, Position);


        AimLine.name = "Tower Aim Line";
        //aim_line.transform.parent = this.TowerObj.transform;
        this.TowerObj.name = "Scanner Tower";
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

    public override void Fire(List<GameObject> enemy_queue)
    {
        //local fields store the closest game object and the distance of the object from the turret
        GameObject select_unit = null;
        float closest_distance = 0;
        TargetEnd = false;
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
            //goes through all of the enemy currently present on the board
            foreach (GameObject enemy in enemy_queue)
            {
                //if the distance hasn't been set, then closest enemy is set to the currently selected enemy in the list
                if (closest_distance == 0)
                {
                    closest_distance = Vector3.Distance(Position, enemy.transform.position);
                    select_unit = enemy;
                    AimLine.GetComponent<LineRenderer>().SetPosition(1, Position);
                    AimLine.GetComponent<LineRenderer>().SetPosition(0, Position);


                }
                else
                {
                    //if the previously closest enemy HAS been set, then it is checked if there is a closer enemy
                    if (Vector3.Distance(Position, enemy.transform.position) < closest_distance)
                    {
                        select_unit = enemy;
                        closest_distance = Vector3.Distance(Position, enemy.transform.position);
                    }
                }
            }
            //if the select_unit has been set to an actually gameobject instance
            //then a line is drawn between the turret and the gameobject

            if (select_unit != null)
            {
                if (closest_distance <= Range)
                {
                    DrawLine(Position, select_unit.transform.position, Color.green);
                    Shoot_Wait_Remaining = 1 / FireRate;

                    Vector3 temp;
                    temp = new Vector3(select_unit.transform.position.x, select_unit.transform.position.y, Position.z);

                    TowerRotation = temp - Position;

                    if (select_unit.GetComponent<AIController>().data.Speed > 0.25)
                    {
                        select_unit.GetComponent<AIController>().data.Speed = select_unit.GetComponent<AIController>().data.Speed / 2;
                    }

               
                    closest_distance = 0;
                }
                else
                {
                    select_unit = null;
                    closest_distance = 0;
                    AimLine.GetComponent<LineRenderer>().SetPosition(1, Position);
                }
            }
        }
    }

}
