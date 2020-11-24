using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class Player_controller : MonoBehaviour
{
    private ServiceManager serviceManager;

    [SerializeField] private int maxHP;
    private int currentHP;
    [SerializeField] private int maxMP;
    [SerializeField] private int MPPerSecond;
    private int currentMP;
    private bool caroutineWorking = false;

    [SerializeField] Slider hpBar;
    [SerializeField] Slider mpBar;
    [SerializeField] TMP_Text scoreText;

    private int score = 0;

    Movement_controller playerMovement;
    private bool canBeDamaged = true;

    void Start()
    {
        playerMovement = GetComponent<Movement_controller>();
        playerMovement.OnGetHurt += OnGetHurt;
        currentHP = maxHP;
        currentMP = maxMP;
        hpBar.maxValue = maxHP;
        hpBar.value = maxHP;
        mpBar.maxValue = maxMP;
        mpBar.value = maxMP;
        serviceManager = ServiceManager.Instanse;
    }

    public void TakeDamage(int damage, DamageType type = DamageType.Casual, Transform enemy = null)
    {
        if (!canBeDamaged)
            return;

        currentHP -= damage;
        if (currentHP <= 0)
        {
            OnDeath();
        }

        switch (type)
        {
            case DamageType.PowerStrike:
                playerMovement.GetHurt(enemy.position);
                break;
        }
        hpBar.value = currentHP;
    }

    public void RestoreHP(int hp)
    {
        currentHP += hp;
        if (currentHP > maxHP)
        {
            currentHP = maxHP;
        }
        hpBar.value = currentHP;
    }

    private void OnGetHurt(bool _canBeDamaged)
    {
        canBeDamaged = _canBeDamaged;
    }

    public bool ChangeMP (int value)
    {
        if (value < 0 && currentMP < Mathf.Abs(value))
            return false;

        currentMP += value;
        if (currentMP > maxMP)
            currentMP = maxMP;
        mpBar.value = currentMP;
        if (!caroutineWorking && value < 0)
        {
            caroutineWorking = true;
            StartCoroutine("RestoreMP");
        }
        
        return true;

    }

    public void ChangeScoreValue (int value)
    {
        score += value;
        scoreText.text = score.ToString();
        Debug.Log(score);
    }

    public IEnumerator RestoreMP()
    {
        while (currentMP < maxMP)
        {
            currentMP += MPPerSecond;

            if (currentMP > maxMP)
                currentMP = maxMP;
            mpBar.value = currentMP;
            yield return new WaitForSeconds(1f);
        }
        caroutineWorking = false;
    }

    public void OnDeath()
    {
        serviceManager.Restart();
    }
}
