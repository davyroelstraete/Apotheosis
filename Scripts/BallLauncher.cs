using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallLauncher : MonoBehaviour
{
    [SerializeField] LineRenderer lr;
    [SerializeField] float power = 10f;
    [SerializeField] float minPower;
    [SerializeField] float maxPower;
    [SerializeField] float dragThreshold = 0.3f;
    Rigidbody2D rb;
    GameManager gm;

    Camera cam;

    Vector2 force;
    Vector3 startPoint = Vector3.negativeInfinity;
    Vector3 endPoint = Vector3.negativeInfinity;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        cam = Camera.main;
    }

    private void Start()
    {
        gm = GameObject.FindObjectOfType<GameManager>();
    }

    private void OnMouseDown()
    {
        lr.SetPosition(0, Vector3.zero);
        lr.SetPosition(1, Vector3.zero);
        startPoint = cam.ScreenToWorldPoint(Input.mousePosition);
        startPoint.z = 0;
        //print(startPoint.ToString());
    }

    private void OnMouseDrag()
    {
        if (gm.CanAcceptPlayerInput())
        {
            endPoint = cam.ScreenToWorldPoint(Input.mousePosition);
            endPoint.z = 0;

            lr.SetPosition(0, startPoint);
            lr.SetPosition(1, endPoint);
        }
    }

    private void OnMouseUp()
    {
        lr.SetPosition(1, Vector3.zero);
        lr.SetPosition(0, Vector3.zero);
        endPoint = cam.ScreenToWorldPoint(Input.mousePosition);
        //print(endPoint.ToString());

        if (gm.CanAcceptPlayerInput()) ProcessClick();
    }

    private void ProcessClick()
    {
        float dist = Vector3.Distance(startPoint, endPoint);
        if (dist > dragThreshold)
        {
            print(dist);

            force = (startPoint - endPoint).normalized * dist * power;

            rb.AddForce(force, ForceMode2D.Impulse);

            gm.StartMovementPhase();
        }
        endPoint = Vector3.negativeInfinity;
        startPoint = Vector3.negativeInfinity;
    }
}
