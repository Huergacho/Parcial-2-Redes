using UnityEngine;

[CreateAssetMenu(fileName = "Base Stats", menuName = "Actor", order = 0)]
public class ActorStats : ScriptableObject
{
    [Header("Attack")]
    [SerializeField] private LayerMask contactLayers;
    public LayerMask ContactLayers => contactLayers;
    
    [SerializeField] private float attackForce;
    public float AttackForce => attackForce;
    
    [SerializeField] private int damageAmount = 1;
    public int DamageAmount => damageAmount;
    
    [SerializeField] private float attackCooldown;
    public float AttackCooldown => attackCooldown;
    
    [Header("Movement")]
    [SerializeField] private float speed;
    public float Speed => speed;
    
    [SerializeField] private float jumpForce;
    public float JumpForce => jumpForce;
    
    [SerializeField] private int maxJumps = 1;
    public int MaxJumps => maxJumps;
    
    [SerializeField] private float maxForce;
    public float MaxForce => maxForce;
    
    [SerializeField]private LayerMask killZoneLayer;
    public LayerMask KillZoneLayer => killZoneLayer;
}