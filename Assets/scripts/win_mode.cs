using UnityEngine;

public class win_mode : MonoBehaviour
{
	public SpriteRenderer sr;

	void Start()
	{
		this.sr = this.GetComponent<SpriteRenderer>();
	}

	public bool enable
	{
		set { this.sr.enabled = value; }
	}
}
