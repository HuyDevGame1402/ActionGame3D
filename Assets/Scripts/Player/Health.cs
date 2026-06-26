using UnityEngine;
using System;

public class Health : MonoBehaviour
{
    public EventHandler OnDead;

    [SerializeField] private int maxHealth = 100;
    [SerializeField] private int currentHealth;
    private CharacterBase character;

    private void Awake()
    {
        currentHealth = maxHealth;
        character = GetComponent<CharacterBase>();
    }

    public void ApplyDamage(int damage)
    {
        currentHealth -= damage;
        CheckHealth();
    }
    private void CheckHealth()
    {
        if(currentHealth <= 0)
        {
            OnDead?.Invoke(this, EventArgs.Empty);  
        }
    }
}
