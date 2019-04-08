using UnityEngine;
using System.Collections;

public class assign_players : MonoBehaviour
{   
	void Start ()
    {
        var players = this.GetComponentsInChildren<player_movement>();

        if( players.Length == 2)
        {
            players[1].set_input_scheme( KeyCode.W, KeyCode.S, KeyCode.A, KeyCode.D );
        }
	}
}
