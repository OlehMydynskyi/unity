using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetCollectables : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D info)
    {
        info.GetComponent<Player_controller>().ChangeScoreValue(10);
        Destroy(gameObject);
    }
}
