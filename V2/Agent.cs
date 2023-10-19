
public class Agent {
	public int X;
	public int Y;
	public float Heading;
	public float PheremoneStrength;

	float realX;
	float realY;

	public enum Action {
		Skip,
		Delete,
		Spawn
	}
	
	public Action State;
	Random rnd;

	public Agent(int xStart, int yStart, float heading) {
		X = xStart;
		Y = yStart;
		
		realX = X;
		realY = Y;

		rnd = new Random(xStart * yStart);
		Heading = (float)rnd.NextDouble() * 2 * MathF.PI;

	}

	public void Sense(Scene scene) {
		float leftAngle = Heading - Settings.AgentSensorAngle;
		int leftX = (int)(realX + MathF.Cos(leftAngle) * Settings.AgentSensorDistance);
		int leftY = (int)(realY + MathF.Sin(leftAngle) * Settings.AgentSensorDistance);
		
		float left = float.MinValue;
		if (!scene.IsOutOfBounds(leftX, leftY)) {
			left = scene.Grid[leftX, leftY].Evaluate(0);
		}
		
		float rightAngle = Heading + Settings.AgentSensorAngle;
		int rightX = (int)(realX + MathF.Cos(rightAngle) * Settings.AgentSensorDistance);
		int rightY = (int)(realY + MathF.Sin(rightAngle) * Settings.AgentSensorDistance);
		
		float right = float.MinValue;
		if (!scene.IsOutOfBounds(rightX, rightY)) {
			right = scene.Grid[rightX, rightY].Evaluate(0);
		}

		int centerX = (int)(realX + MathF.Cos(Heading) * Settings.AgentSensorDistance);
		int centerY = (int)(realY + MathF.Sin(Heading) * Settings.AgentSensorDistance);
		
		float center = float.MinValue;
		if (!scene.IsOutOfBounds(centerX, centerY)) {
			scene.Grid[centerX, centerY].Evaluate(0);
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
	
	public void Move(ref Scene scene) {
		State = Action.Skip;

		float tmpfX = realX + MathF.Cos(Heading) * Settings.AgentSpeed;
		float tmpfY = realY + MathF.Sin(Heading) * Settings.AgentSpeed;
		int tmpX = (int)tmpfX;
		int tmpY = (int)tmpfY;


		int nCount = scene.GetNeighbourCount(X, Y, 5);
		if (nCount > 15) {
			State = Action.Delete;
			return;
		}

		if (scene.IsOutOfBounds(tmpX, tmpY) || scene.Grid[tmpX, tmpY].IsOccupied) {
			Heading = (float)(rnd.NextDouble() * 2 * MathF.PI);
			return;
		}

		scene.Grid[X, Y].IsOccupied = false;
		X = tmpX;
		Y = tmpY;
		scene.Grid[X, Y].IsOccupied = true;
		realX = tmpfX;
		realY = tmpfY;

		scene.Grid[X, Y].PheremoneStrength += 5;

		nCount = scene.GetNeighbourCount(X, Y, 9);
		if (nCount <= 4) {
			State = Action.Spawn;
			return;
		}
		return;
	}
}
