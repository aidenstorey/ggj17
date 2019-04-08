using UnityEngine;
using System.Collections;

public enum jaw_type { top, bottom }

public class jaw : MonoBehaviour
{
    public jaw_type type;
	public bool killing_active = true;
}
