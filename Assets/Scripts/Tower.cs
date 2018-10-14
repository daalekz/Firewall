using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Summary: 
 * Class is used for the different elements that will be placed on the screen and fire at enemies
*/
public class Tower
{
    private Vector3 _position;
    private GameObject rendered_tower;
    private bool _selected;
    private float range;
    private float fire_rate;
    private float radius;
    GameObject aim_line;
    GameController gc;

    //two different constructors to allow for greater user flexibility
    public Tower(int x, int y)
    {
        rendered_tower = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        _position.x = x;
        _position.y = y;
        _position.z = 0;
        var shape_color = rendered_tower.GetComponent<Renderer>().material.color;
        shape_color.a = 0;

        //aim line is the line showing which object the turret is targeting
        aim_line = new GameObject();
        aim_line.AddComponent<LineRenderer>();
    }

    public Tower(Vector3 position)
    {
        rendered_tower = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        _position = position;
        var shape_color = rendered_tower.GetComponent<Renderer>().material.color;
        shape_color.a = 0;
        aim_line = new GameObject();
        aim_line.AddComponent<LineRenderer>();
    }

    //tell the program how the tower should be drawn
    public void Render()
    {
        rendered_tower.transform.position = _position;
        rendered_tower.GetComponent<Renderer>().sortingOrder = 2;


        if (_selected)
        {
            rendered_tower.GetComponent<Renderer>().material.color = Color.yellow;
        }
        else
        {
            rendered_tower.GetComponent<Renderer>().material.color = Color.cyan;
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
    public void Shoot(List<GameObject> enemy_queue)
    {
        //local fields store the closest game object and the distance of the object from the turret
        GameObject closest_unit = null;
        float closet_distance = 0;
       
        //goes through all of the enemy currently present on the board
        foreach(GameObject enemy in enemy_queue)
        {
            //if the distance hasn't been set, then closest enemy is set to the currently selected enemy in the list
            if (closet_distance == 0)
            {
                closet_distance = Vector3.Distance(Position, enemy.transform.position);
                closest_unit = enemy;
            }
            else 
            {
                //if the previously closest enemy HAS been set, then it is checked if there is a closer enemy
                if (Vector3.Distance(Position, enemy.transform.position) < closet_distance)
                {
                    closest_unit = enemy;
                    closet_distance = Vector3.Distance(Position, enemy.transform.position);
                }
            }
        }

        //if the closest_unit has been set to an actually gameobject instance
        //then a line is drawn between the turret and the gameobject
        if (closest_unit != null)
        {
            DrawLine(Position, closest_unit.transform.position, Color.green);
        }
    }
}
