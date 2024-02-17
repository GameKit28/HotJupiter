using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleColorChange : MonoBehaviour
{
	private Toggle toggle;

	private void Start()
	{
		toggle = GetComponent<Toggle>();
		toggle.onValueChanged.AddListener(OnToggleValueChanged);
	}

	private void OnToggleValueChanged(bool isOn)
	{
		ColorBlock cb = toggle.colors;
		if (isOn)
		{
			cb.normalColor = new Color(0, 0.8f, 0.8f);
			cb.selectedColor = new Color(0, 0.8f, 0.8f);
			cb.highlightedColor = Color.cyan;
		}
		else
		{
			cb.normalColor = Color.white;
			cb.selectedColor = Color.white;
			cb.highlightedColor = Color.cyan;
		}
		toggle.colors = cb;
	}
}
