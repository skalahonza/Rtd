using System;
using System.Collections.Generic;
using Assets.Scripts.Car;
using UnityEngine;

class AIPlayer : Player {
	CarControl control;
	CarInfo frontWheelpair;
	CarInfo backWheelPair;
	CarSpirit spirit;
	private static System.Random rand = new System.Random ();

	public override void GameStart () {
		spirit = GetComponent<CarSpirit> ();
		control = GetComponent<CarControl> ();
		agent.autoBraking = false;
		agent.radius = 8.0f;
		agent.speed = 20.0f;
		agent.acceleration = 0.5f;
		if (control.wheelPairs[1].steering) {
			frontWheelpair = control.wheelPairs[1];
			backWheelPair = control.wheelPairs[0];
		} else {
			frontWheelpair = control.wheelPairs[0];
			backWheelPair = control.wheelPairs[1];
		}
	}

    override protected void OnRaceStart(){
        agent.updatePosition = true;
    }

	void FixedUpdate () {
		if (!startRace || finished)
			return;
		agent.velocity = GetComponent<Rigidbody> ().velocity;
		control.VisualizeWheel (control.wheelPairs[1]);
		control.VisualizeWheel (control.wheelPairs[0]);
		if (spirit._powerUp != null)
			if (rand.Next () % 5 == 0)
				spirit.UsePowerUp ();
		if (!agent.isOnNavMesh) {
			Respawn (false);
		}
	}
}
