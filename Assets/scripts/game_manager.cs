using UnityEngine;
using System.Collections.Generic;


public class game_manager : MonoBehaviour
{
	static game_manager gmi;
	cf_task_manager cftm = new cf_task_manager();

	jaw_manager jm;

	List<touching_butt> dead_players = new List<touching_butt>();

	ui_thing[] ui_things;

	int current_ui = 0;

	public bool game_done = false;
	bool end_game_processed = false;

	public static game_manager instance
	{
		get { return game_manager.gmi; }
	}

	void Start()
	{
		if ( gmi != null )
		{
			DestroyImmediate( this.gameObject );
			return;
		}

		gmi = this;

		this.jm = this.GetComponent<jaw_manager>();

		this.ui_things = this.GetComponentsInChildren<ui_thing>();

		foreach ( var ui in this.ui_things )
		{
			ui.disable();
		}

		var t = this.jm.create_rotation( 1.0f, 0.0f, PI( 0.5f ) );

		this.cftm.push_back( task_type.jaw,
			t
		);

		this.cftm.push_back( task_type.menu,
			new show_ui_thing( this.ui_things[ this.current_ui ], 1.0f, 1.5f ),
			new wait_for_task( t ),
			new tooth.grow_teeth( this.GetComponentsInChildren<tooth>() ),
			new reset_game()
        );
	}

	public static float PI( float scalar )
	{
		return Mathf.PI * scalar;
	}

	public void check_game_done()
	{
		this.game_done = this.dead_players.Count > 0;
	}

	void Update()
	{
		if ( this.dead_players.Count > 0 && !this.end_game_processed)
		{
			this.end_game();
		}

		this.cftm.update();
	}

	public void end_game()
	{
		this.cftm.clear_all();

		int winner = 2;

		if ( this.dead_players.Count < 2 )
		{
			var tb = this.GetComponentsInChildren<touching_butt>();
			for ( int i = 0; i < tb.Length; i++ )
			{
				if ( tb[ i ] == this.dead_players[ 0 ] )
				{
					winner = i;
					break;
				}
			}
		}

		print( winner );
		this.ui_things[this.current_ui].winner = winner;

		this.cftm.push_back( task_type.player, new take_control() );
		
		var t = this.jm.create_rotation( 0.25f, this.jm.dur, PI( 2.0f ) );
		this.cftm.push_back( task_type.jaw, 
			t,
			this.jm.create_rotation( 0.25f, PI( 0.0f ), PI( 0.5f ) ) 
		);

		this.cftm.push_back( task_type.menu, 
			new wait_for_task( t ),
			new show_ui_thing( this.ui_things[ this.current_ui ], 0.2f, 1.5f ),
			new reset_game()
		);

		this.end_game_processed = true;		
    }

	public static cf_task_manager task_manager
	{
		get { return game_manager.gmi.cftm; }
	}

	public static jaw_manager jaw_manager
	{
		get { return game_manager.gmi.jm; }
	}

	public static void add_dead_player( touching_butt tb )
	{
		gmi.dead_players.Add( tb );
	}

	public class take_control : task_one_off
	{
		public override void update()
		{
			foreach ( var p in game_manager.gmi.GetComponentsInChildren<player_movement>() )
			{
				p.has_control = false;
			}
		}
	}

	public class give_control : task_one_off
	{
		public override void update()
		{
			foreach ( var p in game_manager.gmi.GetComponentsInChildren<player_movement>() )
			{
				p.has_control = true;
			}
		}
	}

	class players_moved : task
	{
		player_movement[] players;

		public override void on_start()
		{
			this.players = game_manager.gmi.GetComponentsInChildren<player_movement>();
			foreach ( var p in this.players )
			{
				p.has_moved = false;
			}
		}

		public override void update()
		{
			bool has_moved = true;
			foreach ( var p in this.players )
			{
				has_moved &= p.has_moved;
			}

			if ( has_moved )
			{
				this.mark_complete();
			}
		}
	}

	class show_ui_thing : task
	{
		ui_thing ut;
		float time_taken = 0.0f;
		float duration;
		float scale;
		float delta;

		public show_ui_thing( ui_thing ut, float duration = 1.0f, float scale = 1.0f )
		{
			this.ut = ut;
			this.duration = duration;
			this.scale = scale;
			this.delta = this.scale / duration;
		}

		public override void on_start()
		{
			this.ut.enable();
			this.ut.transform.localScale = Vector3.zero;
		}

		public override void update()
		{
			this.time_taken = Mathf.Min( this.duration, this.time_taken + Time.deltaTime );

			var d = this.delta * this.time_taken;
			ut.transform.localScale = new Vector3( d, d, d );

			if ( this.time_taken == this.duration )
			{
				this.mark_complete();
			}
		}
	}

	class hide_ui_thing : task
	{
		ui_thing ut;
		float time_taken = 0.0f;
		float duration;
		float scale;
		float delta;

		public hide_ui_thing( ui_thing ut, float duration = 1.0f, float scale = 1.5f )
		{
			this.ut = ut;
			this.duration = duration;
			this.scale = scale;
			this.delta =  this.scale / duration;
		}

		public override void on_start()
		{
			this.ut.transform.localScale = new Vector3( this.scale, this.scale, this.scale );
		}

		public override void update()
		{
			this.time_taken = Mathf.Min( this.duration, this.time_taken + Time.deltaTime );

			var d = this.scale - this.delta * this.time_taken;
			ut.transform.localScale = new Vector3( d, d, d );

			if ( this.time_taken == this.duration )
			{
				this.ut.disable();
				this.mark_complete();
			}
		}
	}


	class reset_game : task_one_off
	{
		public override void update()
		{
			int shown_ui = game_manager.gmi.current_ui;

			game_manager.gmi.current_ui = 1;
			game_manager.gmi.end_game_processed = false;
			game_manager.gmi.game_done = false;
			game_manager.gmi.dead_players.Clear();
			game_manager.task_manager.clear_all();

			var t = new players_moved();


			game_manager.task_manager.push_back( task_type.player, 
				t 
			);

			game_manager.task_manager.push_back( task_type.jaw,
				new give_control(),
				new wait_for_task( t ),
				new hide_ui_thing( game_manager.gmi.ui_things[shown_ui], 0.5f ),
				game_manager.jaw_manager.create_rotation( 2.0f, game_manager.jaw_manager.dur, game_manager.PI( 2.5f ), 2 ),
				game_manager.jaw_manager.create_rotation( 1.0f, game_manager.PI( 0.5f ), game_manager.PI( 2.5f ), 2 ),
				game_manager.jaw_manager.create_rotation( 0.5f, game_manager.PI( 0.5f ), game_manager.PI( 2.5f ), 2 ),
				game_manager.jaw_manager.create_rotation( 0.25f, game_manager.PI( 0.5f ), game_manager.PI( 2.5f ), int.MaxValue )
			);
		}
	}
}
