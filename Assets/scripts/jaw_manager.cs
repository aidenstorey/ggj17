using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class jaw_manager : MonoBehaviour
{
	float width_initial = 600.0f;

	float hi = 300.0f;
	float hh = 200.0f;
	float height = 300.0f;

	float osi = 0.0f;

	public float dur = 0.0f;

	Dictionary<jaw_type, jaw> jaws = new Dictionary<jaw_type, jaw>();

	side_bits[] side_bits;

	void Start()
	{
		this.osi = Camera.main.orthographicSize;

		foreach ( var j in this.GetComponentsInChildren<jaw>() )
		{
			this.jaws[ j.type ] = j;
		}

		this.side_bits = this.GetComponentsInChildren<side_bits>();

		this.update_window_size();
	}

	void Update()
	{
		if ( game_manager.PI( 1.475f ) <= this.dur && this.dur <= game_manager.PI( 1.525f ) )
		{
			game_manager.instance.check_game_done();
		}

		this.update_window_size();
	}
	
	void update_window_size()
	{
		var delta = Mathf.Sin( this.dur );

		this.height = this.hi + delta * this.hh;
		Screen.SetResolution( ( int ) this.width_initial, ( int ) this.height, false );


		var ratio = ( this.height / this.hi );

		var size = this.osi * ratio;

		Camera.main.orthographicSize = size;
		Camera.main.transform.position = new Vector3( 0.0f, size - this.osi, -10.0f );

		this.jaws[ jaw_type.top ].transform.position = new Vector3( 0.0f, 2.0f * size - this.osi );
		this.jaws[ jaw_type.top ].killing_active = delta <= -0.8f;

		foreach ( var sb in this.side_bits )
		{
			var scale = sb.transform.localScale;
			scale.y = 1.0f * ratio;

			sb.transform.localScale = scale;

			var pos = sb.transform.position;
			pos.y = size - this.osi;

			sb.transform.position = pos;
		}
	}

	public jaw_rotation create_rotation( float speed, float initial = 0.0f, float final = 1.0f, int repeat = 0 )
	{
		return new jaw_rotation( this, speed, initial, final, repeat );
	}

	public class jaw_rotation : task
	{
		jaw_manager jm;
		float speed;
		float initial;
		float final;
		int repeat;

		public jaw_rotation( jaw_manager jm, float speed, float initial = 0.0f, float final = 1.0f, int repeat = 0 )
		{
			this.jm = jm;
			this.speed = speed;
			this.initial = initial;
			this.final = final;
			this.repeat = repeat;
		}

		public override void on_start()
		{
			this.jm.dur = this.initial;
		}

		public override void update()
		{
			this.jm.dur = Mathf.Min( this.final, this.jm.dur + Time.deltaTime / this.speed );

			if ( this.jm.dur == this.final )
			{
				if ( this.repeat > 0 )
				{
					game_manager.task_manager.push_front( task_type.jaw, game_manager.jaw_manager.create_rotation( this.speed, this.initial, this.final, this.repeat - 1 ) );
				}

				this.mark_complete();
			}
		}
	}
}
