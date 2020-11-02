using UnityEngine;
using System.Collections;

public class Instrucoes : MonoBehaviour {
	void OnGUI(){
		GUI.color = Color.red;
		GUI.Label (new Rect (Screen.width / 25.0f, Screen.height / 25.0f, Screen.width / 1.5f, Screen.height / 1.5f), "Edit > ProjectSettings > Graphics > AlwaysIncludedShaders > Add in list: 'GlassStainedBumpDistort' ");
		if (GUI.Button (new Rect (Screen.width - Screen.width / 6.0f, Screen.height - Screen.height / 12.0f, Screen.width / 6, Screen.height / 12), "TUTORIAL")) {
			Application.OpenURL ("https://www.youtube.com/marcosschultzunity");
		}
	}
}
