using System;
using System.Collections;
using System.Linq;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Serialization;

public class CharacterModel : MonoBehaviour
{
    [SerializeField] private Transform attackPoint;
    private Transform _spawnPoint;
    [SerializeField ]private ActorStats stats;
    private int _currentJumps;
    private Rigidbody _rb;
    private LifeController _lifeController;
    private Coroutine _attackCoroutine;
    private CharacterView _view;
    private bool _isMoving;
    private bool _isDead = false;
    public bool IsDead => _isDead;
    private void Awake()
    {
        _view = GetComponentInChildren<CharacterView>();
        _lifeController = GetComponent<LifeController>();
        _rb = GetComponent<Rigidbody>();
    }

    private void Initialize()
    {
        transform.position = _spawnPoint.position;
        _currentJumps = 0;
        _lifeController.OnDie += Die;
    }

    private void Update()
    {
        if (!_isDead)
        {
            CheckJumps();
            CheckTreshHold();
            CheckMoveAnim();
        }
        CheckRotation();
    }

    #region Actions

    public void Move(Vector3 dir)
    {
        if (!_isDead)
        {
            dir *= stats.Speed;
            dir.z = 0;
            dir.y = _rb.velocity.y;
            _rb.velocity = dir;
        }
        else
        {
            transform.position += dir * stats.Speed * Time.deltaTime;
        }
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

    public void Attack()
    {
        if (_attackCoroutine != null || _isDead)
        {
            return;
        }
        _view.AttackAnimation(true,stats.AttackCooldown);
        Collider[] contacts  = Physics.OverlapSphere(attackPoint.position, 0.3f, stats.ContactLayers);
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
        _isDead = true;
        StopAllCoroutines();
        _view.StopAllCoroutines();
        _rb.detectCollisions = false;
        _rb.isKinematic = true;
        _rb.useGravity = false;
        _view.DieAnimation(true);
        RequestManager.Instance.PlayerDie();
    }
    public void Respawn()
    {
        _view.DieAnimation(false);
        _lifeController.Reset();
        transform.position = _spawnPoint.position;
        _currentJumps = 0;
        _rb.detectCollisions = true;
        _rb.useGravity = true;
        _rb.isKinematic = false;
        _isDead = false;
    }


    #endregion
    #region Checks
    public void CheckMoveAnim()
    {
        if (_rb.velocity.x != 0)
        {
            _isMoving = true;
        }
        else
        {
            _isMoving = false;
        }
        _view.MoveAnimation(_isMoving);

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
        if (_rb.velocity.x >= 0)
        {
            transform.rotation = Quaternion.identity;
        }else if (_rb.velocity.x < 0)
        {
            transform.rotation = Quaternion.Euler(0,180,0); 
        }
    }

    public void OnHitAction(int damage, Vector3 dir)
    {
        _lifeController.TakeDamage(-damage);
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
    #endregion


    public void AssignStats(Transform spawnPoint)
    {
        _spawnPoint = spawnPoint;
        Initialize();
    }
    

}