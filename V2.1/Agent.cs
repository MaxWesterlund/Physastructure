using System.Numerics;

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
	readonly Random rng;

	public Agent(int xStart, int yStart) {
		X = xStart;
		Y = yStart;
		
		realX = X;
		realY = Y;

		rng = new Random();
		Heading = (float)rng.NextDouble() * 2 * MathF.PI;
	}

	private struct Sensor {
		public float Value;
		public int ID;
	}

	public void Sense(Scene scene) {
		Sensor[] sensors = new Sensor[3];
		int height = scene.Grid[X, Y].Height;

		for (int i = 0; i < 3; i++) {
			sensors[i].ID = i;

			float angle = Heading + (i - 1) * Settings.AgentSensorAngle;
			int x = (int)MathF.Round(realX + MathF.Cos(angle) * Settings.AgentSensorDistance);
			int y = (int)MathF.Round(realY + MathF.Sin(angle) * Settings.AgentSensorDistance);

			if (scene.IsOutOfBounds(x, y)) {
				sensors[i].Value = float.MinValue;
				continue;
			}
			sensors[i].Value = scene.Grid[x, y].Evaluate(height);
		}

		Array.Sort(sensors, (a, b) => b.Value.CompareTo(a.Value));
	
		if (sensors[0].Value == float.MinValue) {
			// random dir
			Heading += (rng.Next(3) - 1) * Settings.AgentSensorAngle;
		}
		else if (sensors[0].Value == sensors[1].Value) {
			// chose random of top 2
			Heading += (sensors[rng.Next(1)].ID - 1) * Settings.AgentSensorAngle;
		}
		else {
			Heading += (sensors[0].ID - 1) * Settings.AgentSensorAngle;
		}

		// int sensor = 0;
		// float maxVal = 0;
		// for (int i = 0; i < 3; i++) {
		// 	float a = Heading + (i - 1) * Settings.AgentSensorAngle;
		// 	int x = (int)(realX + MathF.Cos(a) * Settings.AgentSensorDistance);
		// 	int y = (int)(realY + MathF.Sin(a) * Settings.AgentSensorDistance);
		// 	float val = scene.IsOutOfBounds(x, y) ?  0 : scene.Grid[X, y].Evaluate(0);
		// 	if (i == 0) {
		// 		maxVal = val;
		// 		sensor = 2;
		// 		continue;
		// 	}
		// 	if (val > maxVal) {
		// 		maxVal = val;
		// 		sensor = i + 2;
		// 	}
		// 	else if (val == maxVal) {
		// 		sensor += i + 2;
		// 	}
		// }

		// switch (sensor) {
		// 	case 2: // Left
		// 		Heading -= Settings.AgentSensorAngle;
		// 		break;
		// 	case 4: // Right
		// 		Heading += Settings.AgentSensorAngle;
		// 		break;
		// 	case 5: // Left & Middle
		// 		Heading -= rng.Next(2) == 1 ? 0 : Settings.AgentSensorAngle;
		// 		break;
		// 	case 6: // Left & Right
		// 		Heading += rng.Next(2) == 1 ? Settings.AgentSensorAngle : -Settings.AgentSensorAngle;
		// 		break;
		// 	case 7: // Middle & Right
		// 		Heading += rng.Next(2) == 1 ? 0 : Settings.AgentSensorAngle;
		// 		break;
		// 	case 8: // Left, Middle & Right
		// 		Heading += Settings.AgentSensorAngle * (rng.Next(3) -1);
		// 		break;
		// }
	}
	
	public void Move(ref Scene scene) {
		State = Action.Skip;

		float tmpfX = realX + MathF.Cos(Heading) * Settings.AgentSpeed;
		float tmpfY = realY + MathF.Sin(Heading) * Settings.AgentSpeed;
		int tmpX = (int)MathF.Round(tmpfX);
		int tmpY = (int)MathF.Round(tmpfY);

		int n = scene.GetNeighbourCount(X, Y, 5);
		if (n > 15) {
			State = Action.Delete;
			return;
		}

		if (scene.IsOutOfBounds(tmpX, tmpY) || scene.Grid[tmpX, tmpY].IsOccupied) {
			Heading = (float)(rng.NextDouble() * 2 * MathF.PI);
			return;
		}

		scene.Grid[X, Y].IsOccupied = false;
		X = tmpX;
		Y = tmpY;
		scene.Grid[X, Y].IsOccupied = true;
		realX = tmpfX;
		realY = tmpfY;

		scene.Grid[X, Y].PheremoneStrength = 1;

		n = scene.GetNeighbourCount(X, Y, 9);
		if (n <= 4) {
			State = Action.Spawn;
			return;
		}
	}
}
