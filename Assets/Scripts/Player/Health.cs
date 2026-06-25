using UnityEngine;

public class Health : MonoBehaviour
{
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
    }

}
