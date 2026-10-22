using UnityEngine;

public class SMEffectCaster : MonoBehaviour
{
	public GameObject moveThis;

	public RaycastHit hit;

	public GameObject[] createThis;

	public float cooldown;

	public float changeCooldown;

	public int selected;

	public GUIText writeThis;

	private float _rndNr;

	private GameObject _effect;

	private void Start()
	{
		selected = createThis.Length - 1;
		writeThis.text = selected + " " + createThis[selected].name;
	}

	private void Update()
	{
		if (cooldown > 0f)
		{
			cooldown -= Time.deltaTime;
		}
		if (changeCooldown > 0f)
		{
			changeCooldown -= Time.deltaTime;
		}
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		if (Physics.Raycast(ray, out hit))
		{
			moveThis.transform.position = hit.point;
			if (Input.GetMouseButton(0) && cooldown <= 0f)
			{
				_effect = Object.Instantiate(createThis[selected], moveThis.transform.position, moveThis.transform.rotation);
				if (_effect.tag == "explosion" || _effect.tag == "missile" || _effect.tag == "breath")
				{
					_effect.transform.position = _effect.transform.position + new Vector3(0f, 1.5f, 0f);
				}
				if (_effect.tag == "shield")
				{
					_effect.transform.position = _effect.transform.position + new Vector3(0f, 0.5f, 0f);
				}
				cooldown = 0.15f;
			}
		}
		if (Input.GetKeyDown(KeyCode.UpArrow) && changeCooldown <= 0f)
		{
			selected++;
			if (selected > createThis.Length - 1)
			{
				selected = 0;
			}
			writeThis.text = selected + " " + createThis[selected].name;
			changeCooldown = 0.1f;
		}
		if (Input.GetKeyDown(KeyCode.DownArrow) && changeCooldown <= 0f)
		{
			selected--;
			if (selected < 0)
			{
				selected = createThis.Length - 1;
			}
			writeThis.text = selected + " " + createThis[selected].name;
			changeCooldown = 0.1f;
		}
	}
}
