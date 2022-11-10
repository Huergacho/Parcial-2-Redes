using System;
using System.Linq;
using Photon.Pun;
using UnityEngine;

public class CharacterModel : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float jumpForce;
    [SerializeField] private int maxJumps = 1;
    [SerializeField] private float maxForce;
    [SerializeField] private Transform attackPoint;
    [SerializeField] private LayerMask contactLayers;
    [SerializeField] private float attackForce;
    [SerializeField] private int damageAmount = 1;
    private int currentJumps;
    private Rigidbody rb;
    private LifeController _lifeController;
    [SerializeField]private LayerMask killZoneLayer;


    private void Awake()
    {
        _lifeController = GetComponent<LifeController>();
        rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        currentJumps = 0;
    }

    private void Update()
    {
        CheckJumps();
        CheckTreshHold();
        CheckRotation();
    }

    public void Move(Vector3 dir)
    {
        dir *= speed;
        dir.z = 0;
        dir.y = rb.velocity.y;
        rb.velocity = dir;
    }

    public void Jump()
    {
        if (currentJumps == maxJumps)
        {
            return;
        }
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        currentJumps++;
    }

    private void CheckTreshHold()
    {
        if (rb.velocity.y > maxForce)
        {
            rb.velocity = Vector3.up * maxForce;
        }
    }

    private void CheckJumps()
    {
        if (rb.velocity.y == 0 && currentJumps != 0)
        {
            currentJumps = 0;
        }
    }

    private void CheckRotation()
    {
        if (rb.velocity.x > 0)
        {
            transform.rotation = Quaternion.identity;
        }else if (rb.velocity.x < 0)
        {
            transform.rotation = Quaternion.Euler(0,180,0); 
        }
    }

    public void Attack()
    {
       Collider[] contacts  = Physics.OverlapSphere(attackPoint.position, 0.3f, contactLayers);

       if (contacts.Length <= 0)
       {
           return;
       }
       foreach (var item in contacts)
       {
           if (item.gameObject != this.gameObject)
           {
               var itemModel = item.GetComponent<CharacterModel>();
               itemModel.OnHitAction(damageAmount);

           }
       }
    }
    public void Die()
    {
        PhotonNetwork.Destroy(gameObject);
    }

    public void OnHitAction(int damage)
    {
        _lifeController.TakeDamage(damage);
        rb.AddForce((Vector3.right + Vector3.up) * attackForce,ForceMode.Impulse);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (LayerCompare.IsGoInLayerMask(other.gameObject, killZoneLayer))
        {
            RequestManager.Instance.RPCMaster("RequestDie",PhotonNetwork.LocalPlayer);
        }
    }
}