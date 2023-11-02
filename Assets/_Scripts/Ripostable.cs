using System;
using System.Collections;
using System.Collections.Generic;
using StarterAssets;
using UnityEngine;

public class Ripostable : Interactable
{
    [SerializeField] private Enemy _enemyScript;

    public void Riposte()
    {
        interactor.isInteracting = true;
        interactor.GetComponent<FirstPersonController>().EquipedWeapon.gameObject.SetActive(false);
        _enemyScript.isImmune = true;
        _enemyScript.GetComponent<Collider>().enabled = false;
        _enemyScript.GetComponent<EnemyAI>().gun.gameObject.SetActive(false);
        _enemyScript.GetComponent<EnemyAI>().enabled = false;
        _enemyScript.GetComponent<Animator>().SetTrigger("Thrown");
    }

    public void RiposteEnd()
    {
        Debug.Log("Riposte Ends");
        interactor.GetComponent<FirstPersonController>().EquipedWeapon.gameObject.SetActive(true);
        interactor.isInteracting = false;
        _enemyScript.Die();
    }

}
