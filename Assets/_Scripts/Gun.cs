using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class Gun : MonoBehaviour
{
    public int damage = 1;
    public float range = 100f;
    public bool isEquipped = true;
    public int ammoCount = 7;
    public float throwForce = 5;
    
    [SerializeField] protected Transform head;
    [SerializeField] protected LineRenderer laserLine;
    [SerializeField] private Collider collider;
    [SerializeField] private Collider pickupTrigger;

    protected RaycastHit _hit;
    private AimController _aimController;
    private Rigidbody _rigidbody;

    protected virtual void Update()
    {
        if (isEquipped)
        {
            if(_aimController.hasEnemy)
                transform.LookAt(_aimController.CurrentEnemyTransform);
            else
                transform.rotation = _aimController.transform.rotation;
        }
    }

    private void Awake() => _rigidbody = GetComponent<Rigidbody>();

    public Gun PickUp(Transform hand)
    {
        isEquipped = true;
        _aimController = AimController.Instance;
        
        // Disable Physics
        _rigidbody.isKinematic = true;
        _rigidbody.useGravity = false;
        collider.enabled = false;
        pickupTrigger.enabled = false;
            
        transform.SetParent(hand);
        transform.localPosition = Vector3.zero;

        return this;
    }

    public void Drop()
    {
        isEquipped = false;
        _aimController = null;
        
        // Throw Weapon
        _rigidbody.isKinematic = false;
        _rigidbody.useGravity = true;
        collider.enabled = true;
        pickupTrigger.enabled = true;

        transform.SetParent(null);
        _rigidbody.AddForce((transform.forward + transform.up * .5f) * throwForce, ForceMode.Impulse);
        _rigidbody.AddTorque(Random.onUnitSphere * 5, ForceMode.Impulse);
    }

    public virtual void Shoot()
    {
        if (ammoCount <= 0)
        {
            // TODO: Empty Gun Sound
            return;
        }
        
        laserLine.SetPosition(0, head.position);

        if (Physics.Raycast(head.position, head.forward, out _hit, range))
        {
            laserLine.SetPosition(1, _hit.point);
            Debug.Log(_hit.transform.name + " with tag: " + _hit.transform.name);
            if (_hit.transform.CompareTag("Invulnerable"))
            {
                var enemy = FindEnemyScript(_hit.transform);
                if(enemy) enemy.Damage(damage);
            }
        }
        else
        {
            laserLine.SetPosition(1, head.position + head.forward * range);
        }

        StartCoroutine(ShowRay());
        ammoCount--;
    }

    protected IEnumerator ShowRay()
    {
        laserLine.enabled = true;
        yield return new WaitForSeconds(0.1f);
        laserLine.enabled = false;
    }

    private Enemy FindEnemyScript(Transform hitTransform)
    {
        if (hitTransform.GetComponent<Enemy>())
        {
            return hitTransform.GetComponent<Enemy>();
        }
        if (hitTransform.parent)
            return FindEnemyScript(hitTransform.parent);
        // Else
        return null;
    }
}
