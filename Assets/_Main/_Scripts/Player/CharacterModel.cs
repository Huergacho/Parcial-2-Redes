using System;
using UnityEngine;
public class CharacterModel : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float jumpForce;
    [SerializeField] private int maxJumps = 1;
    [SerializeField] private float maxForce;
    private int currentJumps;
    private Rigidbody rb;

    private void Awake()
    {
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
        if (currentJumps == maxJumps){return;}
            print("Salto");
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
}