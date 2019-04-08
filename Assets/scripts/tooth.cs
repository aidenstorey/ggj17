using UnityEngine;
using System.Collections;

public class tooth : MonoBehaviour
{
    public jaw_type type;

    bool in_position = true;

    float time_max = 1.0f;
    const float y_initial = 1.0f;
    const float y_final = 0.0f;

    float delta = 0.0f;

    float time_taken = 0.0f;
    
    const int damage_max = 5;
    int damage_taken = damage_max;

    float cooldown_max = 5.0f;
    float cooldown_current = 0.0f;

    SpriteRenderer sr;
    SpriteRenderer sr_damage;

    void Start()
    {
        sr = this.GetComponent<SpriteRenderer>();

        var go = new GameObject( "tooth_damage" );
        go.transform.parent = this.transform;
        go.transform.localPosition = Vector3.zero;
        go.transform.localScale = new Vector3( 1.0f, -1.0f, 1.0f );

        this.sr_damage = go.AddComponent<SpriteRenderer>();
        this.sr_damage.sortingLayerName = "bottom_tooth_damage";

        this.random_tooth();
        this.calculate_detla();
    }
    	
	void Update ()
    {
        if( !this.in_position )
        {
            this.time_taken = Mathf.Min( time_max, this.time_taken + Time.deltaTime );

            var p = this.transform.localPosition;
            p.y = y_initial + delta * this.time_taken;

            this.transform.localPosition = p;

            if( this.time_taken == time_max )
            {
                this.in_position = true;
            }
        }

        if( this.cooldown_current > 0.0f )
        {
            this.cooldown_current = Mathf.Max( 0.0f, this.cooldown_current - Time.deltaTime );

            if( this.cooldown_current == 0.0f )
            {
                this.in_position = false;
            }
        }
	}

    void calculate_detla()
    {
        this.delta = ( y_final - y_initial ) / this.time_max;
    }

    void update_time_max( float tm )
    {
        this.time_max = tm;
        this.calculate_detla();
    }

    public void random_tooth()
    {
		var tt = tooth_type.normal;

		if ( Random.Range( 0, 1000 ) == 0 )
		{
			tt = tooth_type.special;
		}

        this.sr.sprite = tooth_manager.random_tooth( tt, this.type );
    }

    public void damage(int amount)
    {
        this.damage_taken = Mathf.Max( 0, this.damage_taken - amount );

        if( this.damage_taken == 0 )
        {
            this.reset();
        }
        else
        {
            this.sr_damage.sprite = tooth_manager.damage( this.damage_taken );
        }
    }

	void reset(bool reset_growth=true)
	{
		this.damage_taken = damage_max;
		this.sr_damage.sprite = null;

		var p = this.transform.localPosition;
		p.y = y_initial;

		this.transform.localPosition = p;

		this.time_taken = 0.0f;
		this.in_position = true;

		if ( reset_growth )
		{
			this.set_grow_tooth( cooldown_max );
		}
	}

	void set_grow_tooth( float cooldown )
	{
		this.cooldown_current = cooldown;
		this.random_tooth();
	}

	public class grow_teeth : task_one_off
	{
		tooth[] teeth;
		float cooldown;

		public grow_teeth( tooth[] teeth, float cooldown = 0.0f ) : base()
		{
			this.teeth = teeth;
			this.cooldown = cooldown;
		}

		public override void update()
		{
			foreach ( tooth t in this.teeth )
			{
				if ( cooldown == 0.0f )
				{
					t.in_position = false;
				}
				else
				{
					t.set_grow_tooth( this.cooldown );
				}
				
			}
		}
	}

	public class reset_teeth : task_one_off
	{
		tooth[] teeth;

		public reset_teeth( tooth[] teeth ) : base()
		{
			this.teeth = teeth;
		}

		public override void update()
		{
			foreach ( tooth t in this.teeth )
			{
				t.reset( false );
			}
		}
	}
}
