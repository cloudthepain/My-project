using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerStats : NetworkBehaviour
{
    public NetworkVariable<float> playerHealth;
	public NetworkVariable<float> playerMana;
	public NetworkVariable<float> playerAttack;
	public NetworkVariable<float> playerSpeed;
	public NetworkVariable<float> jumpPower;
	public NetworkVariable<bool> isGrounded;

	public override void OnNetworkSpawn()
	{
		base.OnNetworkSpawn();
		playerHealth.Value = 0;
		playerMana.Value = 0;
		playerAttack.Value = 0;
		playerSpeed.Value = 20;
		jumpPower.Value = 9;
		isGrounded.Value = true;
	}
}
