using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MovementScript : MonoBehaviour {

    public float speed = 3.0f;
    //public float catchability = .3f;
    //let's say start out with 3 points a fish can swim between
    Vector2 point1;
    Vector2 point2;
    Vector2 point3;

    Vector2 target;

    Vector2 Mpoint1;
    Vector2 Mpoint2;
    Vector2 Mpoint3;

    Vector2 Mtarget;

    Vector2 leftPoint;
    Vector2 rightPoint;

    public bool move = true;

    public string movementType;

    public bool isBeingSnooted = false;

    // Use this for initialization
    void Start ()
    {

        //lets calculate the random values when the fish is instantiated

        point1 = new Vector2(Random.Range(Camera.main.ScreenToWorldPoint(new Vector2(0, 0)).y, Camera.main.ScreenToWorldPoint(new Vector2(0, Screen.height)).y),
            Random.Range(Camera.main.ScreenToWorldPoint(new Vector2(0, 0)).y, Camera.main.ScreenToWorldPoint(new Vector2(0, Screen.height)).y));
        point2 = new Vector2(Random.Range(Camera.main.ScreenToWorldPoint(new Vector2(0, 0)).y, Camera.main.ScreenToWorldPoint(new Vector2(0, Screen.height)).y),
            Random.Range(Camera.main.ScreenToWorldPoint(new Vector2(0, 0)).y, Camera.main.ScreenToWorldPoint(new Vector2(0, Screen.height)).y));
        point3 = new Vector2(Random.Range(Camera.main.ScreenToWorldPoint(new Vector2(0, 0)).y, Camera.main.ScreenToWorldPoint(new Vector2(0, Screen.height)).y),
            Random.Range(Camera.main.ScreenToWorldPoint(new Vector2(0, 0)).y, Camera.main.ScreenToWorldPoint(new Vector2(0, Screen.height)).y));

        target = point2;

        //For minimal movement AI pathing
        Mpoint1 = new Vector2(Random.Range(Camera.main.ScreenToWorldPoint(new Vector2(0, 0)).y, Camera.main.ScreenToWorldPoint(new Vector2(0, Screen.height)).y),
            Random.Range(Camera.main.ScreenToWorldPoint(new Vector2(0, 0)).y, Camera.main.ScreenToWorldPoint(new Vector2(0, Screen.height)).y));
        Mpoint2 = new Vector2(Mpoint1.x+ Mpoint1.x/3, Mpoint1.y+ Mpoint1.y/3);
        Mpoint3 = new Vector2(Mpoint1.x + Mpoint1.x / 3, Mpoint1.y - Mpoint1.y / 3);

        Mtarget = Mpoint2;

        leftPoint = new Vector2(Camera.main.ScreenToWorldPoint(new Vector2(-30, 0)).x,
            Random.Range(Camera.main.ScreenToWorldPoint(new Vector2(0, 0)).y, Camera.main.ScreenToWorldPoint(new Vector2(0, Screen.height)).y));

        rightPoint = new Vector2(Camera.main.ScreenToWorldPoint(new Vector2(Screen.width+30, 0)).x,
            Random.Range(Camera.main.ScreenToWorldPoint(new Vector2(0, 0)).y, Camera.main.ScreenToWorldPoint(new Vector2(0, Screen.height)).y));

        switch(movementType)
        {
            case "SlideToRight":
                transform.position = leftPoint;
                break;
            case "SlideToLeft":
                transform.position = rightPoint;
                break;
        }
    }
	
	// Update is called once per frame
	void Update ()
    {
        switch (movementType)
        {
            case "3PointSnoot":
                ThreePointSnoot();
                break;
            case "WaitingGame":
                WaitingGame();
                break;
            case "MinimalSnoot":
                MinimalSnootPlease();
                break;
            case "SlideToRight":
                SlideToTheRight();
                break;
            case "SlideToLeft":
                SlideToTheLeft();
                break;
        }

    }
    public void setMove(bool b)
    {
        move = b;
    }
    public void setSpeed(float s)
    {
        speed = s;
    }
    void WaitingGame()
    {

    }
    void ThreePointSnoot()
    {
        if (move)
        {
            float step = speed * Time.deltaTime;


            // move sprite towards the target location
            transform.position = Vector2.MoveTowards(transform.position, target, step);

            //Rotate sprite towards target location
            float rotationSpeed = 10f;
            float offset = -90f;
            //directional vector
            Vector3 direction = new Vector3(target.x, target.y, 0) - transform.position;
            direction.Normalize();
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            Quaternion rotation = Quaternion.AngleAxis(angle + offset, Vector3.forward);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, rotationSpeed * Time.deltaTime);

            if (transform.position.Equals(point1) || transform.position.Equals(point2) || transform.position.Equals(point3))
            {
                int i = Random.Range(0, 3);
                int rolldice = Random.Range(0, 200);

                if (rolldice < 20)
                {
                    switch (i)
                    {
                        case 0:
                            target = point1;
                            break;
                        case 1:
                            target = point2;
                            break;
                        case 2:
                            target = point3;
                            break;
                        default:
                            target = point1;
                            break;
                    }
                }
            }
        }
    }
    void MinimalSnootPlease()
    {
        if (move)
        {

            speed = .5f;
            float step = speed * Time.deltaTime;


            // move sprite towards the target location
            transform.position = Vector2.MoveTowards(transform.position, Mtarget, step);

            //Rotate sprite towards target location
            float rotationSpeed = 10f;
            float offset = -90f;
            //directional vector
            Vector3 direction = new Vector3(Mtarget.x, Mtarget.y, 0) - transform.position;
            direction.Normalize();
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            Quaternion rotation = Quaternion.AngleAxis(angle + offset, Vector3.forward);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, rotationSpeed * Time.deltaTime);
            if (transform.position.Equals(Mpoint1) || transform.position.Equals(Mpoint2) || transform.position.Equals(Mpoint3))
            {
                int i = Random.Range(0, 3);
                //note this random range is roughly the time the fish may be sitting in one place
                int rolldice = Random.Range(0, 500);

                if (rolldice < 20)
                {
                    switch (i)
                    {
                        case 0:
                            Mtarget = Mpoint1;
                            break;
                        case 1:
                            Mtarget = Mpoint2;
                            break;
                        case 2:
                            Mtarget = Mpoint3;
                            break;
                        default:
                            Mtarget = Mpoint1;
                            break;
                    }
                }
            }
        }
    }
    void SlideToTheRight()
    {
        if (move)
        {
            float step = speed * Time.deltaTime;


            // move sprite towards the target location
            transform.position = Vector2.MoveTowards(transform.position, rightPoint, step);

            //Rotate sprite towards target location
            float rotationSpeed = 10f;
            float offset = -90f;
            //directional vector
            Vector3 direction = new Vector3(rightPoint.x, rightPoint.y, 0) - transform.position;
            direction.Normalize();
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            Quaternion rotation = Quaternion.AngleAxis(angle + offset, Vector3.forward);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, rotationSpeed * Time.deltaTime);

            if (transform.position.Equals(rightPoint))
            {
                this.GetComponent<Item>().shouldBeDestroyed = true;
            }
        }
    }
    void SlideToTheLeft()
    {
        if (move)
        {
            float step = speed * Time.deltaTime;


            //Rotate sprite towards target location
            transform.position = Vector2.MoveTowards(transform.position, leftPoint, step);

            //trying to rotate sprite
            float rotationSpeed = 10f;
            float offset = -90f;
            //directional vector
            Vector3 direction = new Vector3(leftPoint.x, leftPoint.y, 0) - transform.position;
            direction.Normalize();
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            Quaternion rotation = Quaternion.AngleAxis(angle + offset, Vector3.forward);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, rotationSpeed * Time.deltaTime);

            if (transform.position.Equals(leftPoint))
            {
                this.GetComponent<Item>().shouldBeDestroyed = true;
            }
        }
    }
}
