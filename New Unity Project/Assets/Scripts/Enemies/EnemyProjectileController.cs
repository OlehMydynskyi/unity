using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectileController : MonoBehaviour
{
    [SerializeField] private int _damage;
    private float _lastEncounter;
    private void OnTriggerEnter2D(Collider2D info)
    {
        
        if (Time.time - _lastEncounter < 0.2f)
            return;

        _lastEncounter = Time.time;
        Player_controller player = info.GetComponent<Player_controller>();
        if (player != null)
            player.TakeDamage(_damage);
        Destroy(gameObject);
    }
}