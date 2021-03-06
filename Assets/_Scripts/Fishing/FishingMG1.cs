using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishingMG1 : FishingMinigame
{
	[SerializeField] private FishingGameFish _fish;
	[SerializeField] private ProgressBar _playerBar;
	[SerializeField] private ProgressBar _fishBar;

	protected override void _init()
	{
		_fish.Init(_fishItem);
		_started = true;
	}

	protected override void _onUpdate()
	{
		_fish.DoMove(_getDirection());
		_fish.ClampPosition(4.5f);
		_tickProgress();
	}

	protected async override System.Threading.Tasks.Task _onGameOver()
	{
		_playerBar.SetProgress(0f);
		_fishBar.SetProgress(0f);
	}

	private Vector2 _getDirection()
	{
		// perlin
		var perlinStep = _fishItem.PerlinStep;
		var direction = Vector2.right;
		var influence = 2f * Mathf.PerlinNoise(Time.time * perlinStep, 0f) - 1f;

		//// will to escape
		var fishLocalPos = _fish.transform.localPosition;
		influence += .1f * (fishLocalPos.x < 0f ? -1f : 1f);
		_fish.GetComponent<SpriteRenderer>().flipX = influence <= 0f;

		// player influence
		influence += 1.5f * _action.ReadValue<float>();

		return direction * influence;
	}

	private void _tickProgress()
	{
		var playerDelta = Time.fixedDeltaTime / _fishItem.PlayerFillTime;
		var fishDelta = Time.fixedDeltaTime / _fishItem.FishFillTime;

		if (_fish.GetIsInZone())
			_playerBar.AddProgress(playerDelta);
		else
			_fishBar.AddProgress(fishDelta);

		var (playerComplete, fishComplete) = (_playerBar.Progress == 1f, _fishBar.Progress == 1f);
		
		if (playerComplete || fishComplete) OnGameOver(playerComplete);
	}
}
