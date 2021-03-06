﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum MonsterType
{
    Ogre = 1,
    Harpy = 2,
    Beast = 3,
    Skeleton = 4,
}

public enum DodgeType
{
    None = 0,
    Defend = 1,
    Dash = 2,
    Jump = 3,
    Attack = 4
}

public class SkeletonScript : MonoBehaviour
{

	public const float Velocity = 0.71f;
	private float _playerVelocity;
	
	public MonsterType Monster;

	// Use this for initialization
	void Start ()
	{
	}
	
	// Update is called once per frame
	void Update () {
		_playerVelocity = GameObject.FindGameObjectWithTag("Player").GetComponent<KnightBehavior>().Speed;
		if(GameObject.FindGameObjectWithTag("Player").GetComponent<KnightBehavior>().Running)
			transform.Translate(Time.deltaTime*Velocity*_playerVelocity*Vector3.left);
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
        if (other.CompareTag("destructor"))
            Destroy(gameObject);
		if(!other.CompareTag("Player"))
			return;
        var knight = other.GetComponent<KnightBehavior>();
        if (!CheckPlayerAction(other.gameObject) && knight.GameMode == GameMode.Game) {
            StartCoroutine(Attack(knight));
        }
	}

    public bool CheckPlayerAction(GameObject player)
    {
        var animator = player.gameObject.GetComponent<Animator>();
        var state = animator.GetCurrentAnimatorStateInfo(0);
        var cases = new Dictionary<int, DodgeType>()
        {
            { Animator.StringToHash("attacking"), DodgeType.Attack },
            { Animator.StringToHash("blocking"), DodgeType.Defend },
            { Animator.StringToHash("dashing"), DodgeType.Dash },
            { Animator.StringToHash("jumping"), DodgeType.Jump },
        };

        var dodge = DodgeType.None;
        cases.TryGetValue(state.shortNameHash, out dodge);
        Debug.Log(state.fullPathHash);
        return (int)dodge == (int)Monster;
    }

	private IEnumerator Attack(KnightBehavior knight)
	{
		var animator = GetComponent<Animator>();
        knight.PrepareToDie();
		animator.SetTrigger("kill");
		while (!animator.GetCurrentAnimatorStateInfo(0).IsName("attack"))
		{
			yield return null;
		}		
		while (animator.GetCurrentAnimatorStateInfo(0).IsName("attack"))
        {
            yield return null;
        }
		yield return knight.GetDamage(1);
	}
}
