using UnityEngine;

public class PickUp : MonoBehaviour
{
    public enum PickUpType
    {
        Heal,
        Coin,
    }
    public PickUpType type;
    public int value = 20;

    public ParticleSystem collertecdVFX;

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            other.gameObject.GetComponent<CharacterBase>().PickUpItem(this);

            if(collertecdVFX != null)
            {
                Instantiate(collertecdVFX, transform.position, Quaternion.identity);    
            }

            Destroy(gameObject);
        }
    }
}
