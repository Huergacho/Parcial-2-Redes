using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingObject : MonoBehaviour
{
    [SerializeField] private Transform moveObject;
    [SerializeField,Range(0,1)] private float UpTime;
    [SerializeField]private bool pingPong;
    [SerializeField] private Vector3 moveTop;
    [SerializeField] private Transform[] movePoints;
    [SerializeField]private int startPoint;
    private int currentPoint;
    void Start()
    {
        currentPoint = startPoint;
        GoFoward();
    }
    void Update()
    {
        MovePlatform();
    }

    void MovePlatform()
    {
        moveObject.position = Vector3.Lerp(moveObject.position, movePoints[currentPoint].position, UpTime * Time.deltaTime);
    }

    void GoBackWards()
    {
        if (moveObject.position == movePoints[currentPoint].position)
        {
            if (currentPoint > 0)
            {
                currentPoint--;
            }
        }
    }

    void GoFoward()
    {
        if (moveObject.position == movePoints[currentPoint].position)
        {
            if (currentPoint <= movePoints.Length)
            { 
                currentPoint++;
            }
        }
    }
}
