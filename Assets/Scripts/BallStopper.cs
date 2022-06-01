using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BallStopper : MonoBehaviour
{
    [SerializeField] float minSpeed = 0.1f;

    Rigidbody2D rb;

    public UnityEvent OnStopped;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void LateUpdate()
    {
        if (rb.velocity != Vector2.zero && rb.velocity.sqrMagnitude < minSpeed)
        {
            rb.velocity = Vector2.zero;
            OnStopped.Invoke();
        }
    }
}
