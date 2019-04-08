using UnityEngine;
using System.Collections;

public enum tooth_type { normal, special };

public class tooth_manager : MonoBehaviour
{
    public Sprite[] normal;
    public Sprite[] special;
    public Sprite[] special_top;

    public Sprite[] damage_sprites;

    static tooth_manager instance;

    void Start()
    {
        if( tooth_manager.instance != null )
        {
            DestroyImmediate( this.gameObject );
            return;
        }

        tooth_manager.instance = this;
    }

    public static Sprite random_tooth( tooth_type tt, jaw_type jt )
    {
        Sprite[] teeth = tt == tooth_type.normal ? 
            tooth_manager.instance.normal : 
            jt == jaw_type.top ? 
                tooth_manager.instance.special_top :
                tooth_manager.instance.special;
                       
        if( teeth.Length > 0 )
        {
            return teeth[Random.Range( 0, teeth.Length )];
        }

        return null;
    }

    public static Sprite damage( int amount )
    {
        if( 0 <= amount && amount < tooth_manager.instance.damage_sprites.Length )
        {
            return tooth_manager.instance.damage_sprites[amount];
        }

        return null;
    }
}
