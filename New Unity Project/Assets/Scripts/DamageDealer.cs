using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DamageDealer : MonoBehaviour
{
    [SerializeField] private int damage;
    [SerializeField] private float timeDelay;
    private Player_controller player;
    private DateTime lastEncounter;

    private void OnTriggerEnter2D(Collider2D info)
    {
        if ((DateTime.Now - lastEncounter).TotalSeconds < 0.02f) return;

        lastEncounter = DateTime.Now;
        player = info.GetComponent<Player_controller>();

        if (player != null)
            player.TakeDamage(damage, DamageType.PowerStrike, gameObject.transform);
    }

    private void OnTriggerExit2D(Collider2D info)
    {
        if (player == info.GetComponent<Player_controller>())
            player = null;
    }

    private void Update()
    {
        if (player != null && (DateTime.Now - lastEncounter).TotalSeconds > timeDelay)
        {
            player.TakeDamage(damage);
            lastEncounter = DateTime.Now;
        }
    }
}
