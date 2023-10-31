using System;
using UnityEngine;

public class Ragdoll : MonoBehaviour
{
    public Rigidbody[] ragdollParts;
    
    public bool isEnabled
    {
        set
        {
            for (int i = 0; i < ragdollParts.Length; i++)
                ragdollParts[i].isKinematic = !value;
        }
    }

    private void Awake() => isEnabled = false;
}

