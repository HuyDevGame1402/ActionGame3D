using UnityEngine;
using System.Collections.Generic;

public class DropWeapons : MonoBehaviour
{
    public List<GameObject> weapons;

    public void DropSwords()
    {
        foreach (GameObject weapon in weapons)
        {
            weapon.AddComponent<Rigidbody>();
            weapon.AddComponent<BoxCollider>();
            weapon.transform.parent = null;
        }
    }
}
