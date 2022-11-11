using System;
using System.Collections;
using System.Linq;
using Photon.Pun;
using UnityEngine;

public class CharacterModel : MonoBehaviour
{
    [SerializeField] private ActorStats stats;
    [SerializeField] private Transform attackPoint;
    private int _currentJumps;
    private Rigidbody _rb;
    private LifeController _lifeController;
    private Coroutine _attackCoroutine;
    private CharacterView _view;
    private bool canAttack;
    private bool isMoving;


    private void Awake()
    {
        _view = GetComponent<CharacterView>();
        _lifeController = GetComponent<LifeController>();
        _rb = GetComponent<Rigidbody>();
        _lifeController.AssignMaxLife(stats.MaxLife);
    }

    private void Start()
    {
        _currentJumps = 0;
        _lifeController.OnDie += Die;
    }

    private void Update()
    {
        CheckJumps();
        CheckTreshHold();
        CheckRotation();
        CheckMoveAnim();
    }

    public void CheckMoveAnim()
    {
        if (_rb.velocity.x != 0)
        {
            isMoving = true;
        }
        else
        {
            isMoving = false;
        }
        _view.MoveAnimation(isMoving);

    }
    public void Move(Vector3 dir)
    {
        dir *= stats.Speed;
        dir.z = 0;
        dir.y = _rb.velocity.y;
        _rb.velocity = dir;
    }

    public void Jump()
    {
        if (_currentJumps == stats.MaxJumps)
        {
            return;
        }
        _rb.AddForce(Vector3.up * stats.JumpForce, ForceMode.Impulse);
        _currentJumps++;
    }

    private void CheckTreshHold()
    {
        if (_rb.velocity.y > stats.MaxForce)
        {
            _rb.velocity = Vector3.up * stats.MaxForce;
        }
    }

    private void CheckJumps()
    {
        if (_rb.velocity.y == 0 && _currentJumps != 0)
        {
            _currentJumps = 0;
        }
    }

    private void CheckRotation()
    {
        if (_rb.velocity.x > 0)
        {
            transform.rotation = Quaternion.identity;
        }else if (_rb.velocity.x < 0)
        {
            transform.rotation = Quaternion.Euler(0,180,0); 
        }
    }

    public void Attack()
    {
        if (_attackCoroutine != null)
        {
            return;
        }
        _view.AttackAnimation(true);
        
        Collider[] contacts  = Physics.OverlapSphere(attackPoint.position, 0.3f, stats.ContactLayers);
        _attackCoroutine = StartCoroutine(AttackCooldown());
        if (contacts.Length <= 0)
        {
            return;
        }
        foreach (var item in contacts)
        {
            if (item.gameObject != gameObject)
            {
                var itemModel = item.GetComponent<CharacterModel>();
                itemModel.OnHitAction(stats.DamageAmount,transform.right);

            }
        }
    }
    public void Die()
    {
        StopAllCoroutines();
        _view.StopAllCoroutines();
        RequestManager.Instance.RemoveModel(this);
        PhotonNetwork.Destroy(gameObject);
    }

    public void OnHitAction(int damage, Vector3 dir)
    {
        _lifeController.TakeDamage(damage);
        _view.GetHitAnimation(true);
        _rb.AddForce((dir + Vector3.up) * stats.AttackForce,ForceMode.Impulse);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (LayerCompare.IsGoInLayerMask(other.gameObject, stats.KillZoneLayer))
        {
           Die();
        }
    }

    private IEnumerator AttackCooldown()
    {
        yield return new WaitForSeconds(stats.AttackCooldown);
        _view.AttackAnimation(false);
        _attackCoroutine = null;
    }
    

}