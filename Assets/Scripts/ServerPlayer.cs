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
    [SerializeField] float playerSpeed;
    [SerializeField] Transform spawnPoint;
    [SerializeField] Vector3 JumpPower = new Vector3 ();
    bool isGrounded;
	// Start is called before the first frame update

	void Start()
    {
        inputActions = new();
        inputActions.Enable();
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
        if (_input.y > 0) Jump(); 
        if(_input.y < 0) return;
		transform.position += _input * Time.deltaTime * playerSpeed;
    }

    void Jump()
    {
        transform.position += JumpPower * Time.deltaTime * playerSpeed;
    }

    [ServerRpc]
    void MoveServerRpc(Vector2 _input) 
    {
        Debug.Log("This is called: " +_input);
        Move(_input);
    }
}
