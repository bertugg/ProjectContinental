using System.Collections;
using System.Collections.Generic;
using StarterAssets;
using UnityEngine;

public class EnemyGun : Gun
{
    // Start is called before the first frame update
    public void Shoot(Vector3 target)
    {
        laserLine.SetPosition(0, head.position);
        if (Physics.Raycast(head.position, target - head.position, out _hit, range))
        {
            laserLine.SetPosition(1, _hit.point);
            Debug.Log(_hit.transform.name + " with tag: " + _hit.transform.name);
            if (_hit.transform.CompareTag("Player"))
            {
                var player = FindPlayerScript(_hit.transform);
                if(player != null) player.Damage(damage);
            }
        }
        else
        {
            laserLine.SetPosition(1, target * range);
        }

        StartCoroutine(ShowRay());
    }


    protected override void Update() { }
    
    private FirstPersonController FindPlayerScript(Transform hitTransform)
    {
        if (hitTransform.GetComponent<FirstPersonController>())
        {
            return hitTransform.GetComponent<FirstPersonController>();
        }
        if (hitTransform.parent)
            return FindPlayerScript(hitTransform.parent);
        // Else
        return null;
    }
}
