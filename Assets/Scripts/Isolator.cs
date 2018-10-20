using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Isolator : Tower
{
    private List<GameObject[]> navPoints = null;
    private int NavPathNum;
    private int HighNavPoint = 0;

    public Isolator(Vector3 position) : base(position, TowerType.Isolator)
    {
        SelectLast = true;
        Damage = 30;
        Range = 0;
        FireRate = 0.11f;
        
        AimLine = new GameObject();
        AimLine.AddComponent<LineRenderer>();
        AimLine.name = "Tower Aim Line";

    }
    

    public override void Target(List<GameObject> enemy_queue)
    {
        //goes through all of the enemy currently present on the board
        foreach (GameObject enemy in enemy_queue)
        {
            if (enemy_queue[0] == enemy)
            {
                AimLine.GetComponent<LineRenderer>().SetPosition(1, Position);
                AimLine.GetComponent<LineRenderer>().SetPosition(0, Position);
                selected_unit = enemy;
                NavPathNum = enemy.GetComponent<AIController>().AIPathNum;
                HighNavPoint = enemy.GetComponent<AIController>().NavPointNum;
                closest_distance = Vector3.Distance(enemy.transform.position, navPoints[NavPathNum][HighNavPoint + 1].transform.position);

            }
            else
            {
                NavPathNum = enemy.GetComponent<AIController>().AIPathNum;

                if (enemy.GetComponent<AIController>().NavPointNum > HighNavPoint)
                {
                    selected_unit = enemy;
                    HighNavPoint = enemy.GetComponent<AIController>().NavPointNum;
                    closest_distance = Vector3.Distance(enemy.transform.position, navPoints[NavPathNum][HighNavPoint + 1].transform.position);
                }
                else if (enemy.GetComponent<AIController>().NavPointNum == HighNavPoint && Vector3.Distance(enemy.transform.position, navPoints[NavPathNum][HighNavPoint + 1].transform.position) < closest_distance)
                {
                    selected_unit = enemy;
                    closest_distance = Vector3.Distance(enemy.transform.position, navPoints[NavPathNum][HighNavPoint + 1].transform.position);
                }
            }
        }
    }

    public override bool Fire(List<GameObject> enemy_queue)
    {
        //local fields store the closest game object and the distance of the object from the turret
        SelectLast = true;

        Shoot_Wait_Remaining -= Time.deltaTime;
        Display_Shot_Remaining -= Time.deltaTime;

        if (Display_Shot_Remaining <= 0)
        {
            AimLine.GetComponent<LineRenderer>().SetPosition(0, new Vector3(0, 0, 0));
            AimLine.GetComponent<LineRenderer>().SetPosition(1, new Vector3(0, 0, 0));

            Display_Shot_Remaining = display_shot;
        }

        if (Shoot_Wait_Remaining <= 0f)
        {
            //get nodeindex
            gc = GameController.instance;

            navPoints = gc.navPoints;
            Target(enemy_queue);
        
            //to prevent any errors, the isolator can only shoot object once they pass the first NAVPOINT
            if (selected_unit != null && gc.WaveController.WaveObjSpawned > 1 && HighNavPoint > 0)
            {
                return true;
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
            Vector3 temp;
            temp = new Vector3(selected_unit.transform.position.x, selected_unit.transform.position.y, Position.z);

            TowerRotation = temp - Position;

            selected_unit.GetComponent<AIController>().data.ApplyDamage(Convert.ToInt32(Damage));
            if (selected_unit.GetComponent<AIController>().data.Health <= 0)
            {
                TowerTools.DestroyGameObj(selected_unit);
                enemy_queue.Remove(selected_unit);
                closest_distance = 0;
            }

            DrawLine(Position, selected_unit.transform.position, Color.green);
            Shoot_Wait_Remaining = 1 / FireRate;
        }
    }

    public override void Render()
    {
        if (rendered_tower == null)
        {
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
        }

        if (tower_gun == null) { 

            tower_gun = TowerTools.Instantiate(deploy_instance.tower_isolator_turret);
            tower_gun.transform.position = Position;
            //rendered_tower.GetComponent<Renderer>().sortingOrder = 2;
            tower_gun.transform.position = new Vector3
                (
                    tower_gun.transform.position.x,
                    tower_gun.transform.position.y,
                    -3
                );
            //aim_line.transform.parent = this.TowerObj.transform;

            this.TowerObj.name = "Isolator Tower";

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
}