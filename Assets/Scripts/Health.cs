using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum HealthState
{
    Human,
    Sick,
    Zombie
}

public class Health : MonoBehaviour
{
    public HealthState state = HealthState.Human;
    [SerializeField] float infectionRadius = 3f;
    [SerializeField] LayerMask layers;
    [SerializeField] Sprite humanSprite;
    [SerializeField] Sprite sickSprite;
    [SerializeField] Sprite zombieSprite;

    [SerializeField] ParticleSystem infectionEffect;
    SpriteRenderer graphics;

    private void Awake()
    {
        graphics = GetComponent<SpriteRenderer>();
    }

    public void InfectNearby()
    {
        if (state == HealthState.Sick)
        {
            infectionEffect.Play();

            var infected = Physics2D.OverlapCircleAll(transform.position, infectionRadius, layers);

            foreach (var i in infected)
            {
                Health h = i.GetComponent<Health>();
                if (h.state == HealthState.Human)
                {
                    h.state = HealthState.Sick;
                    h.StateChanged();
                }
            }

            state = HealthState.Zombie;
            StateChanged();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Wall")) return;

        if (collision.transform.GetComponent<Health>().state == HealthState.Zombie)
        {
            if (state == HealthState.Human)
            {
                state = HealthState.Sick;

                StateChanged();
            }
        }
    }

    public void StateChanged()
    {
        switch (state)
        {
            case HealthState.Human:
                graphics.sprite = humanSprite;
                break;
            case HealthState.Sick:
                graphics.sprite = sickSprite;
                break;
            case HealthState.Zombie:
                graphics.sprite = zombieSprite;
                break;
            default:
                graphics.sprite = humanSprite;
                break;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, infectionRadius);
    }
}
