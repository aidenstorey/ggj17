using UnityEngine;

public class touching_butt : MonoBehaviour
{
	public bool is_touching_butt = false;

	void OnCollisionEnter2D( Collision2D other )
	{
		var t = other.gameObject.GetComponent<jaw>();
		if ( t != null )
		{
			if ( t.CompareTag( "bottom" ) )
			{
				this.is_touching_butt = true;
				return;
			}

			if ( t.CompareTag( "top" ) )
			{
				if ( !this.is_touching_butt && t.killing_active )
				{
					game_manager.add_dead_player( this );
				}
				return;
			}
		}
	}

	void OnCollisionExit2D( Collision2D other )
	{
		var t = other.gameObject.GetComponent<jaw>();
		if ( t != null )
		{
			if ( other.collider.CompareTag( "bottom" ) )
			{
				this.is_touching_butt = false;
				return;
			}
		}
	}
}
