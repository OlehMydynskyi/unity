using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mover : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float range;
    [SerializeField] private Direction _direction;
    private Vector2 startPoint;
    private int direction = 1;
    void Start()
    {
        startPoint = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (_direction == Direction.Vertical)
        {
            if (transform.position.y - startPoint.y > range && direction > 0)
            {
                direction *= -1;
            }
            else if (startPoint.y - transform.position.y > range && direction < 0)
            {
                direction *= -1;
            }

            transform.Translate(0, speed * direction * Time.deltaTime, 0);
        }
        else
        {
            if (transform.position.x - startPoint.x > range && direction > 0)
            {
                direction *= -1;
            }
            else if (startPoint.x - transform.position.x > range && direction < 0)
            {
                direction *= -1;
            }

            transform.Translate(speed * direction * Time.deltaTime, 0, 0);
        }

    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(range * 2, range * 2, 0));
        //Gizmos.DrawWireSphere(transform.position, range * 2);
    }

    public enum Direction
    {
        Vertical,
        Horisontal,
    }

}
