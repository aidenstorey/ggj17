using UnityEngine;
using System.Collections.Generic;

public enum direction { none, up, down, left, right };

public class player_movement : MonoBehaviour
{
    Dictionary<direction, KeyCode> kd = new Dictionary<direction, KeyCode>()
    {
        { direction.up, KeyCode.UpArrow },
        { direction.down, KeyCode.DownArrow },
        { direction.left, KeyCode.LeftArrow },
        { direction.right, KeyCode.RightArrow },
    };

    current_queue<direction> horizontal = new current_queue<direction>();
    Dictionary<direction, float> horizonal_velocity = new Dictionary<direction, float>()
    {
        { direction.none, 0.0f },
        { direction.left, -1.0f },
        { direction.right, 1.0f },

    };

    public float speed = 10.0f;
    public float jump = 1000.0f;

    public bool is_jumping = true;

	Animator a;
    public Rigidbody2D rb;
    break_tooth bt;

	direction previous_state = direction.none;

	public bool has_moved = false;
	public bool has_control = false;

	float punch_time = 0.0f;
	float punch_time_total = 0.5f;

    void Start()
    {
		this.a = this.GetComponent<Animator>();
        this.rb = this.GetComponent<Rigidbody2D>();
        this.bt = this.GetComponent<break_tooth>();
    }

	void Update()
	{
		this.horizontal_input();

		if ( this.has_control && this.punch_time == 0.0f )
		{
			if ( !this.is_jumping && this.rb.velocity.y >= 0.0f && Input.GetKeyDown( this.kd[ direction.up ] ) )
			{
				this.rb.AddRelativeForce( new Vector2( 0.0f, this.jump ) );
				this.is_jumping = true;

				this.has_moved = true;
			}

			var vel = this.rb.velocity;
			vel.x = this.horizonal_velocity[ this.horizontal.current ] * this.speed;

			this.rb.velocity = vel;

			var s = this.transform.localScale;

			switch ( this.horizontal.current )
			{
				case direction.left:
					this.has_moved = true;
					this.a.Play( "run" );
					if ( s.x < 0 )
					{
						s.x *= -1;
					}
					break;

				case direction.right:
					this.has_moved = true;
					this.a.Play( "run" );
					if ( s.x > 0 )
					{
						s.x *= -1;
					}
					break;

				case direction.none:
					if ( this.previous_state != direction.none )
					{
						this.a.Play( "idle" );
					}
					break;
			}

			this.previous_state = this.horizontal.current;

			this.transform.localScale = s;

			if ( Input.GetKeyDown( this.kd[ direction.down ] ) )
			{
				this.a.Play( "punch" );
				this.bt.damage_closest();
				this.punch_time = this.punch_time_total;
			}
		}

		if ( this.punch_time > 0.0f )
		{
			var vel = this.rb.velocity;
			vel.x = 0.0f;

			this.rb.velocity = vel;
			this.punch_time = Mathf.Max( 0.0f, this.punch_time -= Time.deltaTime );
		}
	}

    void OnCollisionEnter2D( Collision2D other )
    {
        if( other.collider.CompareTag( "bottom" ) )
        {
            this.is_jumping = false;

			{
				var t = other.gameObject.GetComponent<jaw>();
				if ( t != null )
				{
					this.a.Play( "crouch" );
					return;
				}
			}
		}
	}

	void OnCollisionStay2D( Collision2D other )
	{
		if ( other.collider.CompareTag( "bottom" ) )
		{
			{
				var t = other.gameObject.GetComponent<jaw>();
				if ( t != null )
				{
					this.a.Play( "crouch" );
					return;
				}
			}
		}
	}

	void OnCollisionExit2D( Collision2D other )
	{
		if ( other.collider.CompareTag( "bottom" ) )
		{
			{
				var t = other.gameObject.GetComponent<jaw>();
				if ( t != null )
				{
					this.a.Play( "idle" );
					return;
				}
			}
		}
	}

	void horizontal_input()
    {
        foreach( var d in helper.iterate_enum_excluding( direction.none, direction.up, direction.down ) )
        {
            if( Input.GetKeyDown( this.kd[d] ) )
            {
                this.horizontal.add( d );
            }
            else if( Input.GetKeyUp( this.kd[d] ) )
            {
                this.horizontal.remove( d );
            }
        }
    }

    public void set_input_scheme( KeyCode u, KeyCode d, KeyCode l, KeyCode r )
    {
        this.kd[direction.up] = u;
        this.kd[direction.down] = d;
        this.kd[direction.left] = l;
        this.kd[direction.right] = r;
    }
}
