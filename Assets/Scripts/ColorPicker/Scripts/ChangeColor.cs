using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ChangeColor : MonoBehaviour {
    [Tooltip("What Color Picker Code Will Affect The Color.")]
    public GameObject ColorPicker;
    [Tooltip("material index to change")]
    public int MaterialIndex = 0;
	private ColorPickerUnityUI ColorPickerToUse;
	public ObjectPalette ObjectPaletteToUse;
	private bool isCamera;
	private bool isLight;
	Color color;


    private void Awake()
    {
		color = new Color(PlayerPrefs.GetFloat("Color"), PlayerPrefs.GetFloat("Color1"), PlayerPrefs.GetFloat("Color2"), 255f);
	}
    void Start () 
	{
		
		gameObject.GetComponent<Renderer>().materials[MaterialIndex].color = color;
		if (!ObjectPaletteToUse)
        {
            ColorPickerToUse = GameObject.Find("BodyColorPicker").GetComponent<ColorPickerUnityUI>();
        }

		if (gameObject.GetComponent<Camera> ()) 
		{
			isCamera = true;
		} 
		else 
		{
			isCamera = false;		
		}

		if (gameObject.GetComponent<Light> ()) 
		{
			isLight = true;

		} 
		else 
		{
			isLight = false;		
		}
	}

	void Update () 
	{
		if (ColorPickerToUse) {
			if (!isCamera && !isLight && gameObject.GetComponent<Renderer> ().material) {
                if (ColorPickerToUse.value.a < 1)
                    return;
				gameObject.GetComponent<Renderer>().material.color = ColorPickerToUse.value;
				PlayerPrefs.SetFloat("Color", ColorPickerToUse.value.r);
				PlayerPrefs.SetFloat("Color1", ColorPickerToUse.value.g);
				PlayerPrefs.SetFloat("Color2", ColorPickerToUse.value.b);

			}

			if (isCamera) {
                if (ColorPickerToUse.value.a < 1)
                    return;
                gameObject.GetComponent<Camera> ().backgroundColor = ColorPickerToUse.value;
			}

			if (isLight) {
                if (ColorPickerToUse.value.a < 1)
                    return;
                gameObject.GetComponent<Light> ().color = ColorPickerToUse.value;
			}
		} 
		else if (ObjectPaletteToUse) 
		{
			if (!isCamera && !isLight && gameObject.GetComponent<Renderer> ().material) 
			{
				gameObject.GetComponent<Renderer> ().materials[MaterialIndex].color = ObjectPaletteToUse.value;
				PlayerPrefs.SetFloat("Color", ColorPickerToUse.value.r);
				PlayerPrefs.SetFloat("Color1", ColorPickerToUse.value.g);
				PlayerPrefs.SetFloat("Color2", ColorPickerToUse.value.b);
			}

			if (isCamera) 
			{
				gameObject.GetComponent<Camera> ().backgroundColor = ObjectPaletteToUse.value;
			}
			
			if (isLight) 
			{
				gameObject.GetComponent<Light> ().color = ObjectPaletteToUse.value;
			}
		}
	}
}
