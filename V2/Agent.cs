
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

	public Agent(int xStart, int yStart) {
		X = xStart;
		Y = yStart;
		
		realX = X;
		realY = Y;

		rnd = new Random(xStart * yStart);
		Heading = (float)rnd.NextDouble() * 2 * MathF.PI;
	}

	public void Sense(Scene scene) {
		// float[] sensors = new float[3];
		int sensor = 0;
		float maxVal = 0;
		for (int i = 0; i < 3; i++) {
			float a = Heading + (i - 1) * Settings.AgentSensorAngle;
			int x = (int)(realX + MathF.Cos(a) * Settings.AgentSensorDistance);
			int y = (int)(realY + MathF.Sin(a) * Settings.AgentSensorDistance);
			// if (!scene.IsOutOfBounds(x, y)) {
			// 	sensors[i] = scene.Grid[X, y].Evaluate(0);
			// }
			// else {
			// 	sensors[i] = float.MinValue;
			// }
			float val = scene.IsOutOfBounds(x, y) ?  0 : scene.Grid[X, y].Evaluate(0);
			if (i == 0) {
				maxVal = val;
				continue;
			}
			if (val > maxVal || (val == maxVal && (float)rnd.NextDouble() > .5f)) {
				maxVal = val;
				sensor = i;
			}
		}

		switch (sensor) {
			case 0:
				Heading -= Settings.AgentSensorAngle;
				break;
			case 2:
				Heading -= Settings.AgentSensorAngle;
				break;
		}
		
		// float max = MathF.Max(MathF.Max(sensors[0], sensors[2]), sensors[1]);

		// if (sensors[2] == sensors[0] && sensors[0] == sensors[1]) {
		// 	Heading += Settings.AgentSensorAngle * rnd.Next(3) -1;
		// }
		// else if (sensors[0] == sensors[2]) {
		// 	Heading += rnd.Next(2) == 1 ? Settings.AgentSensorAngle : -Settings.AgentSensorAngle;
		// }
		// else if (max == sensors[0]) {
		// 	Heading -= Settings.AgentSensorAngle;
		// }
		// else if (max == sensors[2]) {
		// 	Heading += Settings.AgentSensorAngle;

		// }

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

		scene.Grid[X, Y].PheremoneStrength = 1;

		nCount = scene.GetNeighbourCount(X, Y, 9);
		if (nCount <= 4) {
			State = Action.Spawn;
			return;
		}
		return;
	}
}
