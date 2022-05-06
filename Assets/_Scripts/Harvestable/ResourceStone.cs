﻿using UnityEngine;

public class ResourceStone : Resource
{
	public override ItemType ItemDrop => ItemType.STONE;

	public override void Interact()
	{
		Player.InventoryWrapper.ModifyQuantity(ItemDrop, Random.Range(3, 7)); // 3-6
	}
}