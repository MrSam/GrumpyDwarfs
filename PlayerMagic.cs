using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerMagic : MonoBehaviour {

	public GameObject magicBar;
	public float playerMagic = 100; // from 100 till 0;
	public float maxPlayerMagic = 100;
	public Color color_100;
	public Color color_50;
	public Color color_30;
	
	private RectTransform healthRectTransform;
	private float prevMagic;

	private float lastMagicDecrease;
	
	void Start() {
		healthRectTransform = magicBar.GetComponent<RectTransform> ();
		prevMagic = playerMagic;
	}

	public void Update() {
		if(playerMagic < maxPlayerMagic && lastMagicDecrease < Time.time - 5f)
			IncreasePlayerMagic(Time.deltaTime * 5f);

		// go faster if you are really low
		if(playerMagic < maxPlayerMagic / 2 && lastMagicDecrease < Time.time - 5f)
			IncreasePlayerMagic(Time.deltaTime * 5f);
	}

	public float GetPlayerMagic() {
		return playerMagic;
	}

	public void SetPlayerMagic(float val) {
		this.playerMagic = val;
	}

	public void DecreasePlayerMagic(float val) {
		lastMagicDecrease = Time.time;

		this.playerMagic -= val;
	}

	public void IncreasePlayerMagic(float val) {
		if(this.playerMagic + val <= maxPlayerMagic)
			this.playerMagic += val;

		if (this.playerMagic + val > maxPlayerMagic)
			this.playerMagic = maxPlayerMagic;
	}

	void OnGUI() {
		// avoid unnecessary calculations
		if (prevMagic == playerMagic)
			return;

		healthRectTransform.sizeDelta = getHealth();

		if (playerMagic <= 100 && playerMagic >= 50) {
			magicBar.GetComponent<RawImage> ().color = color_100;
		}
		
		if (playerMagic < 50 && playerMagic > 30) {
			magicBar.GetComponent<RawImage> ().color = color_50;
		}
		
		if (playerMagic < 30) {
			magicBar.GetComponent<RawImage> ().color = color_30;
		}

		prevMagic = playerMagic;
	}
	
	Vector2 getHealth() {
		Vector2 healthVector = new Vector2 (playerMagic, healthRectTransform.rect.height);
		
		return healthVector;
	}
}
