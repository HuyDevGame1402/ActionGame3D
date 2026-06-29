using UnityEngine;
using System;

public class Health : MonoBehaviour
{
    public EventHandler OnDead;

    [SerializeField] private int maxHealth = 100;
    [SerializeField] private int currentHealth;
    private CharacterBase character;

    public event EventHandler<OnDamageClass> OnDamage;

    public class OnDamageClass : EventArgs
    {
        public float healthPercentage;
    }

    private void Awake()
    {
        currentHealth = maxHealth;
        character = GetComponent<CharacterBase>();
    }

    public void ApplyDamage(int damage)
    {
        currentHealth -= damage;
        CheckHealth();

        InvokeEventChangeHealth();
    }
    private void CheckHealth()
    {
        if(currentHealth <= 0)
        {
            OnDead?.Invoke(this, EventArgs.Empty);  
        }
    }
    public void AddHealth(int health)
    {
        if (currentHealth <= 0) return;
        currentHealth += health;
        if(currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
        InvokeEventChangeHealth();
    }

    private float GetCurrentHealthPercentage()
    {
        return (float)currentHealth / (float)maxHealth;
    }

    private bool CheckPlayer()
    {
        return character as PlayerCharacter != null;
    }

    private void InvokeEventChangeHealth()
    {
        if (CheckPlayer())
        {
            OnDamage?.Invoke(this, new OnDamageClass
            {
                healthPercentage = GetCurrentHealthPercentage()
            });
        }
    }
}
