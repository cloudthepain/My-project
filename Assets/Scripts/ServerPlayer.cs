using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.Netcode.Components;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ServerPlayer : NetworkBehaviour
{
    MyPlayerActions inputActions;
    [SerializeField] Transform spawnPoint;
    PlayerStats playerStats;
    bool isGrounded;
    bool canJump;
    Rigidbody2D rb;
	// Start is called before the first frame update

	void Start()
    {
        inputActions = new();
        inputActions.Enable();
        inputActions.Player.Jump.performed += OnJumpPerformed;
        playerStats = GetComponent<PlayerStats>();
        rb = GetComponent<Rigidbody2D>();
        canJump = true;
	}

	public override void OnNetworkSpawn()
	{
        transform.position = spawnPoint.position;
	}

	// Update is called once per frame
	void Update()
    {
        if(!IsOwner) return;
		Vector2 input = inputActions.Player.Movement.ReadValue<Vector2>();

        if (IsServer && IsLocalPlayer)
        {
			Move(input);
		}
        else if(IsClient && IsLocalPlayer)
        {
			MoveServerRpc(input);
		}
    }

    void Move(Vector3 _input)
    {
        _input.Normalize();
        Debug.Log(_input);
        rb.AddForce(new Vector2(_input.x,0));
    }



    [ServerRpc]
    void MoveServerRpc(Vector2 _input) 
    {
        Move(_input);
    }
	private void OnJumpPerformed(InputAction.CallbackContext context)
	{
		if (IsServer && IsLocalPlayer)
		{
			if (!canJump) { return; }
			rb.AddForce(new Vector2(0, 10), ForceMode2D.Impulse);
		}
		else if (IsClient && IsLocalPlayer)
		{
            OnJumpPerformedServerRpc();
		}
	}
    [ServerRpc]
    void OnJumpPerformedServerRpc()
    {
		if (!canJump) { return; }
		rb.AddForce(new Vector2(0, 100), ForceMode2D.Impulse);
	}
}
