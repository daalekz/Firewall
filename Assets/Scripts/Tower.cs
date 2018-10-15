using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/*
 * Summary: 
 * Class is used for the different elements that will be placed on the screen and fire at enemies
*/
public class Tower
{
    private Deploy deploy;

    private Vector3 _position;
    public GameObject rendered_tower;
    public GameObject tower_gun;
    private bool _selected;
    private float fire_rate;
    private float radius;
    private float damage;
    private bool target_end = false;
    private bool vert = false;
    private Vector3 tower_rotation;

    Color shape_color;

    GameObject aim_line;
    public GameController gc;
    private float shoot_wait_remaining;
    public float display_shot = 0.2f;
    private float display_shot_remaining = 0.2f;
    private TowerType _tower_type;
    private Sprite graphical_tower;
    
    public Tower(Vector3 position, TowerType type)
    {
        deploy = Deploy.instance;
        _tower_type = type;
        _position = position;
        shoot_wait_remaining = 0;
        gc = GameController.instance;
    }


    //tell the program how the tower should be drawn
    public virtual void Render()
    {



        if (_selected)
        {
            //rendered_tower.GetComponent<Renderer>().material.color = Color.yellow;
            rendered_tower.GetComponent<SpriteRenderer>().color = Color.yellow;
        }
        else
        {
            //rendered_tower.GetComponent<Renderer>().material.color = Color.cyan;
            rendered_tower.GetComponent<SpriteRenderer>().color = Color.cyan;
        }
    }

    //returns if the given tower has been selected
    public bool Selected
    {
        get
        {
            return _selected;
        }

        set
        {
            _selected = value;
        }
    }


    //returns the object component on the class
    public GameObject TowerObj
    {
        get
        {
            return rendered_tower;
        }

        set
        {
            rendered_tower = value;
        }
    }

    public Deploy deploy_instance
    {
        get
        {
            return deploy;
        }

        set
        {
            deploy = value;
        }
    }

    public Color Color
    {
        get
        {
            return shape_color;
        }

        set
        {
            shape_color = value;
        }
    }

    //return the position of the object
    public Vector3 Position
    {
        get
        {
            return _position;
        }

        //if you want to move tower position!
        set
        {
            _position = value;
        }
    }
    
    public TowerType Tower_Type
    {
        get
        {
            return _tower_type;
        }

        set
        {
            _tower_type = value;
        }
    }

    public float Range
    {
        set
        {
            radius = value;
        }

        get
        {
            return radius;
        }
    }

    public float Damage
    {
        set
        {
            damage = value;
        }

        get
        {
            return damage;
        }
    }

    public bool Vert
    {
        get
        {
            return vert;
        }

        set
        {
            vert = value;
        }
    }

    public float FireRate
    {
        get
        {
            return fire_rate;
        }

        set
        {
            fire_rate = value;
        }
    }

    public float Display_Shot_Remaining
    {
        get
        {
            return display_shot_remaining;
        }

        set
        {
            display_shot_remaining = value;
        }
    }

    public float Shoot_Wait_Remaining
    {
        get
        {
            return shoot_wait_remaining;
        }

        set
        {
            shoot_wait_remaining = value;
        }
    }

    public bool TargetEnd
    {
        get
        {
            return target_end;
        }

        set
        {
            target_end = value;
        }
    }

    public GameObject AimLine
    {
        get
        {
            return aim_line;
        }

        set
        {
            aim_line = value;
        }
    }

    public Vector3 TowerRotation
    {
        get
        {
            return tower_rotation;
        }

        set
        {
            tower_rotation = value;
        }
    }

    //draws a line between two points
    public void DrawLine(Vector3 start, Vector3 end, Color color)
    {
        LineRenderer lr = aim_line.GetComponent<LineRenderer>();
        lr.material = new Material(Shader.Find("Particles/Alpha Blended Premultiply"));
        lr.SetColors(color, color);
        lr.SetWidth(0.1f, 0.1f);
        lr.SetPosition(0, start);
        lr.SetPosition(1, end);
    }

    //targets and shoots the closest enemy
    //should be within the turret's radius
    //but not implemented quite yet!
    public virtual void Fire(List<GameObject> enemy_queue)
    {
        //local fields store the closest game object and the distance of the object from the turret
        GameObject select_unit = null;
        float closest_distance = 0;
        target_end = true;
        GameObject[] navPoints = new GameObject[0];

        shoot_wait_remaining -= Time.deltaTime;
        display_shot_remaining -= Time.deltaTime;

        if (display_shot_remaining <= 0)
        {
            aim_line.GetComponent<LineRenderer>().SetPosition(1, new Vector3(0,0,0));
            aim_line.GetComponent<LineRenderer>().SetPosition(0, new Vector3(0,0,0));

            display_shot_remaining = display_shot;
        }

        if (shoot_wait_remaining <= 0f) {
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
                if (closest_distance <= radius)
                {
                    DrawLine(Position, select_unit.transform.position, Color.green);
                    shoot_wait_remaining = 1 / fire_rate;

                    Vector3 temp;
                    temp = new Vector3(select_unit.transform.position.x, select_unit.transform.position.y, Position.z);

                    tower_rotation = temp - Position;
                    
                    select_unit.GetComponent<AIController>().data.ApplyDamage(Convert.ToInt32(Damage));
                    if (select_unit.GetComponent<AIController>().data.Health <= 0)
                    {
                        TowerTools.DestroyGameObj(select_unit);

                        enemy_queue.Remove(select_unit);
                    }
                    
                    closest_distance = 0;
                }
                else
                {
                    select_unit = null;
                    closest_distance = 0;
                    aim_line.GetComponent<LineRenderer>().SetPosition(1, Position);
                }
            }
        }
    }
}
