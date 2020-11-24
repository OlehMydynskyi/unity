using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;

[RequireComponent(typeof(Movement_controller))]
public class PC_input_controller : MonoBehaviour
{
    Movement_controller playerMovement;
    DateTime strikeClickTime;

    float move;
    bool jump;
    bool crawling;
    bool canAtack;

    private void Start()
    {
        playerMovement = GetComponent<Movement_controller>();
    }

    void Update()
    {
        move = Input.GetAxisRaw("Horizontal");

        if (Input.GetButtonDown("Jump")) jump = true;
        else jump = false;

        crawling = Input.GetKey(KeyCode.C);

        if (Input.GetKey(KeyCode.E))
            playerMovement.StartCasting();

        if (!IsPointerOverUI())
        {
            if (Input.GetButtonDown("Fire1"))
            {
                strikeClickTime = DateTime.Now;
                canAtack = true;
            }

            if (Input.GetButtonUp("Fire1"))
            {
                float holdTime = (float)(DateTime.Now - strikeClickTime).TotalSeconds;
                if (canAtack)
                    playerMovement.StartSwordAtack(holdTime);
                canAtack = false;
            }

            if (Input.GetButtonUp("Fire3"))
            {
                playerMovement.Blink();
            }
        }
        
        if ((DateTime.Now - strikeClickTime).TotalSeconds >= playerMovement.ChargeTime * 2 && canAtack)
        {
            playerMovement.StartSwordAtack(playerMovement.ChargeTime);
            canAtack = false;
        }



        playerMovement.Move(move, jump, crawling);
    }

    private bool IsPointerOverUI() => EventSystem.current.IsPointerOverGameObject();
}
