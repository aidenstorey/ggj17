using UnityEngine;
using System.Collections.Generic;

public class break_tooth : MonoBehaviour
{
    List<tooth> teeth = new List<tooth>();

    public int damage_amount = 1;
    public int damage_multiplier = 1;

    float cooldown_max = 0.5f;
    float cooldown_current = 0.0f;

    bool can_break = false;

	player_movement pm;

    void Update()
    {
        if( this.cooldown_current > 0.0f )
        {
            this.cooldown_current = Mathf.Max( 0.0f, this.cooldown_current - Time.deltaTime );
        }
    }

    public void damage_closest()
    {
        if( this.cooldown_current == 0.0f && this.can_break )
        {
            tooth closest = null;
            float distance = float.MaxValue;
            
            foreach( var t in this.teeth )
            {
                var d = Mathf.Abs( ( t.transform.position - this.transform.position ).x );
                if( d < distance )
                {
                    closest = t;
                    distance = d;
                }
            }

			if ( closest != null )
			{
				closest.damage( this.damage_amount * this.damage_multiplier );
			}
			else if ( this.pm != null )
			{
				var delta = this.pm.transform.position - this.transform.position;
				delta.z = 0.0f;
				delta.y = 1.0f;
				delta.x = delta.x > 0.0f ? 0.3f : -0.3f;

				this.pm.rb.AddForce( delta.normalized * 3000.0f );
			}

            this.cooldown_current = this.cooldown_max;
		}
	}
	
    void OnCollisionEnter2D( Collision2D other )
    {
        if( other.collider.CompareTag( "bottom" ) )
        {
            {
                var t = other.gameObject.GetComponent<tooth>();
                if( t != null )
                {
                    this.teeth.Add( t );
                    return;
                }
            }
            {
                var t = other.gameObject.GetComponent<jaw>();
                if( t != null )
                {
                    this.can_break = false;
                    return;
                }
            }
			return;
        }

		if ( other.collider.CompareTag( "player" ) )
		{
			var t = other.gameObject.GetComponent<player_movement>();
			if ( t != null )
			{
				this.pm = t;
				return;
			}
			return;
		}
    }

    void OnCollisionExit2D( Collision2D other )
    {
        if( other.collider.CompareTag( "bottom" ) )
        {
            {
                var t = other.gameObject.GetComponent<tooth>();
                if( t != null )
                {
                    this.teeth.Remove( t );
                    return;
                }
            }
            {
                var t = other.gameObject.GetComponent<jaw>();
                if( t != null )
                {
                    this.can_break = true;
                    return;
                }
            }
		}

		if ( other.collider.CompareTag( "player" ) )
		{
			var t = other.gameObject.GetComponent<player_movement>();
			if ( t != null )
			{
				this.pm = null;
				return;
			}
			return;
		}
	}
}
