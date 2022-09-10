using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LightGive.UIUtil;

public class UIPowerup : UINodeAnimation
{
	[SerializeField] Transform _buttonContentParent = null;
	[SerializeField] GameObject _buttonPrefab = null;

	protected override void OnInit()
	{
		base.OnInit();
	}

	public void CreateButton(int powerupType)
	{
		var button = Instantiate(_buttonPrefab, _buttonContentParent);
	}
}