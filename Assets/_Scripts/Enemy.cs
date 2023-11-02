using System;
using StarterAssets;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Transform aimPoint;
    public Ragdoll ragdoll;
    public GameObject droppedWeapon;
    public bool isImmune = false;
    
    [SerializeField] private int _hp = 1;
    private Camera _camera;
    private Plane[] _cameraFrustrum;
    private Collider _collider;
    private Renderer _renderer;
    private bool _inCamera = false;
    private bool _isVisible;
    
    private void OnBecameVisible()
    {
        Debug.Log("Visible:" + _isVisible);
        _isVisible = true;
    }

    private void OnBecameInvisible()
    {
        Debug.Log("Visible:" + _isVisible);
        _isVisible = false;
    }

    private void Start()
    {
        _camera = Camera.main;
        _collider = GetComponent<Collider>();
        _renderer = GetComponentInChildren<Renderer>();
    }

    private void Update()
    {
        TestVisibility();
    }


    public void TestVisibility()
    {
        var bounds = _collider.bounds;
        _cameraFrustrum = GeometryUtility.CalculateFrustumPlanes(_camera);
        
        if (GeometryUtility.TestPlanesAABB(_cameraFrustrum, bounds))
        {
            if(Physics.Linecast(_camera.transform.position, _collider.GetComponentInChildren<Renderer>().bounds.center, out var hit))
            {
                if(hit.transform.gameObject == _collider.gameObject)
                {
                    // Debug.Log("Now you see me!");
                    if (!_inCamera)
                    {
                        _inCamera = true;
                        //_renderer.materials[0].color = Color.red;
                        AimController.Instance.AddEnemy(this);
                    }
                    return;
                }
                else
                {
                    // Debug.Log("There is some obstacle");
                }
            }
        }
        
        if (_inCamera)
        {
            _inCamera = false;
            //_renderer.materials[0].color = Color.white;
            AimController.Instance.RemoveEnemy(this);
        }
        
    }

    public void Damage(int amount)
    {
        if(isImmune)
            return;
        
        _hp -= amount;
        if (_hp <= 0)
        {
            Die();
        }
    }
    
    public void Die()
    {
        GetComponent<Collider>().enabled = false;
        GetComponent<EnemyAI>().gun.gameObject.SetActive(false);
        GetComponent<EnemyAI>().enabled = false;
        GetComponent<Animator>().enabled = false;
            
        // Death operations
        ragdoll.isEnabled = true;
        var drop = Instantiate(droppedWeapon);
        drop.transform.position = transform.position + transform.up * 2 + transform.forward * 2;
        drop.transform.rotation = transform.rotation;
        drop.GetComponent<Gun>().Drop();

        AimController.Instance.RemoveEnemy(this);
        enabled = false;
    }
}
