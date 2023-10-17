using Raylib_cs;
using System.Collections.Generic;
using System.Numerics;
using System;

public enum AgentAction {
	Skip,
	Delete,
	Spawn
}

public class Agent {
	public int X;
	public int Y;
	public float Heading;
	public float PheremoneStrength;

	float realX;
	float realY;

	public AgentAction State;
	Random rnd;

	public Agent(int xStart, int yStart, float heading) {
		X = xStart;
		Y = yStart;
		
		realX = X;
		realY = Y;

		rnd = new Random(xStart * yStart);
		Heading = (float)rnd.NextDouble() * 2 * MathF.PI;

	}

	public void Sense(Map map) {
		float leftAngle = Heading - Settings.AgentSensorAngle;
		int leftX = (int)(realX + MathF.Cos(leftAngle) * Settings.AgentSensorDistance);
		int leftY = (int)(realY + MathF.Sin(leftAngle) * Settings.AgentSensorDistance);
		
		float left = float.MinValue;
		if (!map.IsOutOfBounds(leftX, leftY)) {
			left = map.Grid[leftX, leftY].Evaluate(0);
		}
		
		float rightAngle = Heading + Settings.AgentSensorAngle;
		int rightX = (int)(realX + MathF.Cos(rightAngle) * Settings.AgentSensorDistance);
		int rightY = (int)(realY + MathF.Sin(rightAngle) * Settings.AgentSensorDistance);
		
		float right = float.MinValue;
		if (!map.IsOutOfBounds(rightX, rightY)) {
			right = map.Grid[rightX, rightY].Evaluate(0);
		}

		int centerX = (int)(realX + MathF.Cos(Heading) * Settings.AgentSensorDistance);
		int centerY = (int)(realY + MathF.Sin(Heading) * Settings.AgentSensorDistance);
		
		float center = float.MinValue;
		if (!map.IsOutOfBounds(centerX, centerY)) {
			map.Grid[centerX, centerY].Evaluate(0);
		}
		
		float max = MathF.Max(MathF.Max(left, right), center);
	
		float h1 = Heading;

		if (right == left && left == center) {
			Heading += Settings.AgentSensorAngle * rnd.Next(3) -1;
		}
		else if (left == right) {
			Heading += rnd.Next(2) == 1 ? Settings.AgentSensorAngle : -Settings.AgentSensorAngle;
		}
		else if (max == left) {
			Heading -= Settings.AgentSensorAngle;
		}
		else if (max == right) {
			Heading += Settings.AgentSensorAngle;

		}

		return;
	}
	
	public void Move(ref Map map) {
		State = AgentAction.Skip;

		float tmpfX = realX + MathF.Cos(Heading) * Settings.AgentSpeed;
		float tmpfY = realY + MathF.Sin(Heading) * Settings.AgentSpeed;
		int tmpX = (int)tmpfX;
		int tmpY = (int)tmpfY;


		int nCount = map.GetNeighbourCount(X, Y, 5);
		if (nCount > 15) {
			State = AgentAction.Delete;
			return;
		}

		if (map.IsOutOfBounds(tmpX, tmpY) || map.Grid[tmpX, tmpY].IsOccupied) {
			Heading = (float)(rnd.NextDouble() * 2 * MathF.PI);
			return;
		}

		map.Grid[X, Y].IsOccupied = false;
		X = tmpX;
		Y = tmpY;
		map.Grid[X, Y].IsOccupied = true;
		realX = tmpfX;
		realY = tmpfY;

		map.Grid[X, Y].PheremoneStrength += 5;

		nCount = map.GetNeighbourCount(X, Y, 9);
		if (nCount <= 4) {
			State = AgentAction.Spawn;
			return;
		}
		return;
	}
}
