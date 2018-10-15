using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Isolator : Tower
{
    public Isolator(Vector3 position) : base(position, TowerType.Isolator)
    {
        TargetEnd = true;
        Damage = 30;
        Range = 0;
        FireRate = 1;

        rendered_tower = TowerTools.Instantiate(deploy_instance.tower_isolator, Position, Quaternion.identity);
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
        this.TowerObj.name = "Isolator Tower";

        //aim_line.transform.parent = this.TowerObj.transform;
    }

    public override void Fire(List<GameObject> enemy_queue)
    {
        //local fields store the closest game object and the distance of the object from the turret
        GameObject select_unit = null;
        float closest_distance = 0;
        TargetEnd = true;
        List<GameObject[]> navPoints = null;
        int NavPathNum;

        Shoot_Wait_Remaining -= Time.deltaTime;
        Display_Shot_Remaining -= Time.deltaTime;

        if (Display_Shot_Remaining <= 0)
        {
            AimLine.GetComponent<LineRenderer>().SetPosition(1, new Vector3(0, 0, 0));
            AimLine.GetComponent<LineRenderer>().SetPosition(0, new Vector3(0, 0, 0));

            Display_Shot_Remaining = display_shot;
        }

        if (Shoot_Wait_Remaining <= 0f)
        {
            //get nodeindex
            gc = GameController.instance;

            navPoints = gc.navPoints;
            int HighNavPoint = 0;
        

            //goes through all of the enemy currently present on the board
            foreach (GameObject enemy in enemy_queue)
            {
                if (enemy_queue[0] == enemy)
                {
                    select_unit = enemy;
                    NavPathNum = enemy.GetComponent<AIController>().AIPathNum;
                    HighNavPoint = enemy.GetComponent<AIController>().NavPointNum;
                    closest_distance = Vector3.Distance(enemy.transform.position, navPoints[NavPathNum][HighNavPoint + 1].transform.position);

                }
                else
                {
                    NavPathNum = enemy.GetComponent<AIController>().AIPathNum;

                    if (enemy.GetComponent<AIController>().NavPointNum > HighNavPoint)
                    {
                        select_unit = enemy;
                        NavPathNum = enemy.GetComponent<AIController>().AIPathNum;

                        HighNavPoint = enemy.GetComponent<AIController>().NavPointNum;
                        closest_distance = Vector3.Distance(enemy.transform.position, navPoints[NavPathNum][HighNavPoint + 1].transform.position);
                    }
                    else if (enemy.GetComponent<AIController>().NavPointNum == HighNavPoint && Vector3.Distance(enemy.transform.position, navPoints[NavPathNum][HighNavPoint + 1].transform.position) < closest_distance)
                    {
                        select_unit = enemy;
                        closest_distance = Vector3.Distance(enemy.transform.position, navPoints[NavPathNum][HighNavPoint + 1].transform.position);
                    }
                }
            }

            if (select_unit != null)
            {
                switch (Tower_Type)
                {
                    case TowerType.Scanner:
                        select_unit.GetComponent<AIController>().data.Speed = select_unit.GetComponent<AIController>().data.Speed / 2;
                        break;

                    default:
                        select_unit.GetComponent<AIController>().data.ApplyDamage(Convert.ToInt32(Damage));

                        if (select_unit.GetComponent<AIController>().data.Health <= 0)
                        {
                            TowerTools.DestroyGameObj(select_unit);
                            enemy_queue.Remove(select_unit);
                            closest_distance = 0;
                        }
                        break;

                }

                DrawLine(Position, select_unit.transform.position, Color.green);
                Shoot_Wait_Remaining = 1 / FireRate;
            }
        }

    }
}