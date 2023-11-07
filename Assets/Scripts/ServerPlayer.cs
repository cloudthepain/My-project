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
    public Rigidbody2D rb;
	// Start is called before the first frame update

	void Start()
    {
        inputActions = new();
        inputActions.Enable();
        inputActions.Player.Jump.performed += OnJumpPerformed;
        playerStats = GetComponent<PlayerStats>();
        rb = GetComponent<Rigidbody2D>();
	}

	public override void OnNetworkSpawn()
	{
        transform.position = spawnPoint.position;
	}

	// Update is called once per frame
	void Update()
    {
		Debug.Log(NetworkObject.IsOwner);
		if (!NetworkObject.IsOwner) { return; }

		Vector2 input = inputActions.Player.Movement.ReadValue<Vector2>();
		Debug.Log(input);
        if (IsServer && IsLocalPlayer)
        {
			Move(input);
		}
        else if(IsClient && IsLocalPlayer)
        {
			MoveServerRpc(input);
		}
    }

    void Move(Vector2 _input)
    {
        _input.Normalize();
        rb.AddForce(new Vector2(_input.x,0));
    }

	[ServerRpc]
	void MoveServerRpc(Vector2 _input)
	{
		Move(_input);
	}


		void OnCollisionEnter2D(Collision2D collision)
	{
		if(!IsLocalPlayer) return;
		if (collision.gameObject.tag == "Ground")
		{
			playerStats.isGrounded.Value = true;
			Debug.Log("Landed: " + playerStats.isGrounded.Value);
		}
	}
	private void OnJumpPerformed(InputAction.CallbackContext context)
	{
		if (IsServer && IsLocalPlayer)
		{
			if (!playerStats.isGrounded.Value) { return; }
			rb.AddForce(new Vector2(0, playerStats.jumpPower.Value), ForceMode2D.Impulse);
			playerStats.isGrounded.Value = false;
			Debug.Log("Jumping");
		}
		else if (IsClient && IsLocalPlayer)
		{
            OnJumpPerformedServerRpc();
		}
	}
    [ServerRpc]
    void OnJumpPerformedServerRpc()
    {
		if (!playerStats.isGrounded.Value) { return; }
		rb.AddForce(new Vector2(0, playerStats.jumpPower.Value), ForceMode2D.Impulse);
        playerStats.isGrounded.Value = false;
        Debug.Log("Jumping");
	}
}
