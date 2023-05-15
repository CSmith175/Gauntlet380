using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobberProjectile : MonoBehaviour
{
    public Vector3 destinationPoint;
    public float lobHeight = 3.0f;

    public EnemyStats stats;
    private bool startLob = false;
    private bool lobbing = false;
    private float u, timeStart;
    private float jumpTime = 0.7f;
    private Vector3 p012, startingPoint, jumpPoint;

    public void IntializeLob(Vector3 destination)
    {
        startingPoint = transform.localPosition;
        jumpPoint = (destination + transform.localPosition)/2 + (transform.up * lobHeight);
        destinationPoint = destination;
        startLob = true;
    }


    private void Update()
    {
        if (startLob)
        {
            startLob = false;
            lobbing = true;
            timeStart = Time.time;
        }

        if (lobbing)
        {
            Debug.Log("Moving to target");
            LobToTarget();
        }
    }
    private void LobToTarget()
    {
        u = (Time.time - timeStart) / jumpTime;
        if (u >= 1)
        {
            Debug.Log("Reached target");
            u = 1;
            lobbing = false;
            gameObject.SetActive(false);
        }

        Vector3 p01, p12;
        p01 = (1 - u) * startingPoint + u * jumpPoint;
        p12 = (1 - u) * jumpPoint + u * destinationPoint;

        p012 = (1 - u) * p01 + u * p12;

        transform.localPosition = p012;
    }

    public void AssignDestinationPoint(Vector3 endPos)
    {
        destinationPoint = endPos;
    }
}
