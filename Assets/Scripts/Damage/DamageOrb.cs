using UnityEngine;

public class DamageOrb : MonoBehaviour
{
    [SerializeField] private float speed = 2f;
    [SerializeField] private int damage = 10;
    [SerializeField] private ParticleSystem hitVFX;
    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        rb.MovePosition(transform.position + transform.forward * speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        PlayerCharacter character = other.gameObject.GetComponent<CharacterBase>() as PlayerCharacter;
        if (character != null)
        {
            character.ApplyDamage(damage, transform.position);
        }
        Instantiate(hitVFX, transform.position, Quaternion.identity);
        gameObject.SetActive(false);
    }
}
