using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaveEntrance : MonoBehaviour, IInteractable
{
	private TeleportToPoint _teleporter;
	public event EventHandler OnInteract;

	private void Start() => _teleporter = GetComponent<TeleportToPoint>();
	public void Interact() => _teleporter.Teleport();
}
