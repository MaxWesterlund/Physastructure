
public class Agent {
	public float X;
	public float Y;
	public float Heading;
	public float SporeStrength;

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
		
		rnd = rrnd;
		Heading = (float)rnd.NextDouble() * 2 * MathF.PI;
	}

	private struct Sensor {
		public float Value;
		public int ID;
	}

	public void Sense(Scene scene) {
		Sensor[] sensors = new Sensor[3];
		int height = scene.Grid[(int)X, (int)Y].Height;

		for (int i = 0; i < 3; i++) {
			sensors[i].ID = i;

			float angle = Heading + (i - 1) * Settings.SensorAngle;
			int x = (int)MathF.Round(X + MathF.Cos(angle) * Settings.SensorDistance);
			int y = (int)MathF.Round(Y + MathF.Sin(angle) * Settings.SensorDistance);
			
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

		float tmpX = X + MathF.Round(MathF.Cos(Heading) * Settings.AgentSpeed);
		float tmpY = Y + MathF.Round(MathF.Sin(Heading) * Settings.AgentSpeed);

		int nCount = scene.GetNeighbourCount((int)X, (int)Y, 5);
		if (nCount > 15) {
			State = Action.Delete;
			return;
		}

		if (scene.IsOutOfBounds((int)tmpX, (int)tmpY) || scene.Grid[(int)tmpX, (int)tmpY].IsOccupied) {
			Heading = (float)rnd.NextDouble() * 2 * MathF.PI;
			return;
		}

		scene.Grid[(int)X, (int)Y].IsOccupied = false;
		X = tmpX;
		Y = tmpY;
		scene.Grid[(int)X, (int)Y].IsOccupied = true;

		scene.Grid[(int)X, (int)Y].Solflux += Settings.AgentSolflux;

		nCount = scene.GetNeighbourCount((int)X, (int)Y, 9);
		if (nCount <= 4) {
			State = Action.Spawn;
			return;
		}
		return;
	}
}
