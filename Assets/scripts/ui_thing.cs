using UnityEngine;

public class ui_thing : MonoBehaviour
{
	SpriteRenderer[] srs;
	win_mode[] wm;

	bool is_wm;

	int w;

	public int winner
	{
		set { this.w = value;}
	}

	void Start()
	{
		this.srs = this.GetComponentsInChildren<SpriteRenderer>();
		this.wm = this.GetComponentsInChildren<win_mode>();

		this.is_wm = this.wm.Length > 0;
	}

	public void enable()
	{
		foreach ( var sr in this.srs )
		{
			sr.enabled = true;
		}

		if ( this.is_wm )
		{
			for ( int i = 0; i < this.wm.Length; i++ )
			{
				this.wm[ i ].enable = (int) i == w;
				print( this.wm[ i ].sr.sprite.name );
			}
		}
	}

	public void disable()
	{
		foreach ( var sr in this.srs )
		{
			sr.enabled = false;
		}
	}
}
