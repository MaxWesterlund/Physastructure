
public class Agent {
	public int X;
	public int Y;
	public float Heading;
	public float SporeStrength;

	float IEEEX;
	float IEEEY;

	public enum Action {
		Skip,
		Delete,
		Spawn
	}
	
	public Action State;
	Random rnd;

	public Agent(int xStart, int yStart, float heading, ref Random rrnd) {
		X = xStart;
		Y = yStart;
		
		IEEEX = X;
		IEEEY = Y;

		rnd = rrnd;
		Heading = (float)rnd.NextDouble() * 2 * MathF.PI;
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

			float angle = Heading + (i - 1) * Settings.SensorAngle;
			int x = (int)(IEEEX + MathF.Cos(angle) * Settings.SensorDistance);
			int y = (int)(IEEEY + MathF.Sin(angle) * Settings.SensorDistance);

			if (scene.IsOutOfBounds(x, y)) {
				sensors[i].Value = float.MinValue;
				continue;
			}
			sensors[i].Value = scene.Grid[x, y].Evaluate(height);
		}

		Array.Sort(sensors, (a, b) => b.Value.CompareTo(a.Value));
	
		if (sensors[0].Value == sensors[2].Value) {
			// random dir
			Heading += (rnd.Next(3) - 1) * Settings.SensorAngle;
		}
		else if (sensors[0].Value == sensors[1].Value) {
			// chose random of top 2
			Heading += (sensors[rnd.Next(2)].ID - 1) * Settings.SensorAngle;
		}
		else {
			Heading += (sensors[0].ID - 1) * Settings.SensorAngle;
		}

		return;
	}
	
	public void Move(ref Scene scene) {

		State = Action.Skip;

		float tmpIEEEX = IEEEX + MathF.Cos(Heading) * Settings.AgentSpeed;
		float tmpIEEEY = IEEEY + MathF.Sin(Heading) * Settings.AgentSpeed;
		int tmpX = (int)MathF.Round(tmpIEEEX);
		int tmpY = (int)MathF.Round(tmpIEEEY);


		int nCount = scene.GetNeighbourCount(X, Y, 5);
		if (nCount > 15) {
			State = Action.Delete;
			return;
		}

		if (scene.IsOutOfBounds(tmpX, tmpY) || scene.Grid[tmpX, tmpY].IsOccupied) {
			Heading = (float)rnd.NextDouble() * 2 * MathF.PI;
			return;
		}

		scene.Grid[X, Y].IsOccupied = false;
		X = tmpX;
		Y = tmpY;
		scene.Grid[X, Y].IsOccupied = true;
		IEEEX = tmpIEEEX;
		IEEEY = tmpIEEEY;

		scene.Grid[X, Y].SporeStrength += 5;

		nCount = scene.GetNeighbourCount(X, Y, 9);
		if (nCount <= 4) {
			State = Action.Spawn;
			return;
		}
		return;
	}
}
