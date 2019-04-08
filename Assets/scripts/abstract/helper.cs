using UnityEngine;
using System.Linq;

public class helper
{
	public static T random_enum<T>()
	{
		var v = System.Enum.GetValues( typeof( T ) );
		return ( T ) v.GetValue( Random.Range( 0, v.Length ) );
	}

	public static T random_enum_excluding<T>( params T[] excluding )
	{
		var v = System.Enum.GetValues( typeof( T ) ).Cast<T>().ToList().Except( excluding ).ToList();
		return ( T ) v[ Random.Range( 0, v.Count() ) ];
	}

	public static System.Collections.Generic.IEnumerable<T> iterate_enum<T>()
	{
		foreach ( var v in System.Enum.GetValues( typeof( T ) ) )
		{
			yield return ( T ) v;
		}
	}

	public static System.Collections.Generic.IEnumerable<T> iterate_enum_excluding<T>( params T[] excluding )
	{
		foreach ( var v in System.Enum.GetValues( typeof( T ) ).Cast<T>().ToList().Except( excluding ) )
		{
			yield return ( T ) v;
		}
	}
}
