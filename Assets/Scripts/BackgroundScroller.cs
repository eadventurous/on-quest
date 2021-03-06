﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundScroller : MonoBehaviour {
	
	[HideInInspector]
	public float ScrollSpeed;

	public float MaxScrollSpeed;
	private Vector2 _savedOffset;
	private Renderer _renderer;
	private float _playerSpeed;

    public float PlayerSpeed { get => _playerSpeed; set => _playerSpeed = value; }

    void Start ()
	{
		_renderer = GetComponent<Renderer>();
		_savedOffset = _renderer.sharedMaterial.GetTextureOffset ("_MainTex");
		ScrollSpeed = MaxScrollSpeed;
	}
	//0.01 * player_speed and 0.71 * player_speed
	void Update () {
		PlayerSpeed = GameObject.FindGameObjectWithTag("Player").GetComponent<KnightBehavior>().Speed;
		var x = Time.deltaTime * ScrollSpeed * PlayerSpeed;
		var offset = _renderer.sharedMaterial.GetTextureOffset ("_MainTex") + new Vector2 (x, 0);
		_renderer.sharedMaterial.SetTextureOffset ("_MainTex", offset);
	}

	void OnDisable () {
		//_renderer.sharedMaterial.SetTextureOffset ("_MainTex", _savedOffset);
	}
}
