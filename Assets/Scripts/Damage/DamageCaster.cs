using UnityEngine;
using System.Collections.Generic;

public class DamageCaster : MonoBehaviour
{
    private Collider damageCasterCollider;
    private int damage = 30;
    [SerializeField] private string targetTag;

    private List<Collider> damagedTargetList;

    private void Awake()
    {
        damageCasterCollider = GetComponent<Collider>();
        damageCasterCollider.enabled = false;
        damagedTargetList = new List<Collider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == targetTag && !damagedTargetList.Contains(other))
        {
            CharacterBase targetCharacter = other.GetComponent<CharacterBase>();
            if(targetCharacter != null )
            {
                targetCharacter.ApplyDamage(damage);
            }
            damagedTargetList.Add(other);
        }
    }
    private void EnableDamageCaster()
    {
        damagedTargetList.Clear();
        if( damageCasterCollider != null )
        {
            damageCasterCollider.enabled = true;
        }
    }
    private void DisableDamageCaster()
    {
        damagedTargetList.Clear();
        if (damageCasterCollider != null)
        {
            damageCasterCollider.enabled = false;
        }
    }
}
