using UnityEngine;
using System.Collections;

public class GameSettings : MonoBehaviour {

	public static string selectedDwarf;

	void Start () {
		DontDestroyOnLoad(this);
	}
}
