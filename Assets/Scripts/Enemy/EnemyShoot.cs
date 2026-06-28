using UnityEngine;

public class EnemyShoot : MonoBehaviour
{
    [SerializeField] private Transform shootingPoint;
    [SerializeField] private GameObject damageOrb;

    [SerializeField] private EnemyCharacter enemyCharacter;

    private void Awake()
    {
        enemyCharacter = GetComponent<CharacterBase>() as EnemyCharacter;
    }

    public void ShootTheDamageOrb()
    {
        Instantiate(damageOrb, shootingPoint.position, Quaternion.identity);
    }
    private void Update()
    {
        enemyCharacter.RotateToTarget();
    }
}
