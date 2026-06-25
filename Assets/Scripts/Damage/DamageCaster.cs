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

                RaycastHit hit;
                Vector3 orignalPos = transform.position + (-damageCasterCollider.bounds.extents.z)
                    * transform.forward;
                bool isHit = Physics.BoxCast(orignalPos, damageCasterCollider.bounds.extents / 2,
                    transform.forward, out hit, transform.rotation, damageCasterCollider.bounds.extents.z,
                    1 << 6);

                if (isHit)
                {
                    PlayerVFXManager.Instance.PlaySlash(hit.point + new Vector3(0.5f, 0));
                }

            }
            damagedTargetList.Add(other);
        }
    }
    public void EnableDamageCaster()
    {
        damagedTargetList.Clear();
        if( damageCasterCollider != null )
        {
            damageCasterCollider.enabled = true;
        }
    }
    public void DisableDamageCaster()
    {
        damagedTargetList.Clear();
        if (damageCasterCollider != null)
        {
            damageCasterCollider.enabled = false;
        }
    }
    private void OnDrawGizmos()
    {
        if(damageCasterCollider == null)
        {
            damageCasterCollider = GetComponent<Collider>();
        }
        RaycastHit hit;
        Vector3 orignalPos = transform.position + (-damageCasterCollider.bounds.extents.z)
            * transform.forward;
        bool isHit = Physics.BoxCast(orignalPos, damageCasterCollider.bounds.extents / 2,
            transform.forward, out hit, transform.rotation, damageCasterCollider.bounds.extents.z,
            1 << 6);
        if (isHit)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(hit.point, 0.3f);
        }
    }
}
