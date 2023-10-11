public class Data {
	public int Height;
	public float SporeStrength;
	public float PheremoneStrength;

	public bool IsOccupied;
	
	public Data() {
		Height = 0;
		PheremoneStrength = 0;

		IsOccupied = false;

		return;
	}

	public void Decay() {
		PheremoneStrength *= Settings.PheremoneDecayRate;
		if (PheremoneStrength < 0.001) {
			PheremoneStrength = 0f;
		}
		return;
	}

	public float Evaluate(int height) {
		int heightDiff = Math.Abs(height - Height);
		float value =  PheremoneStrength * Settings.PheremoneWeight - heightDiff * Settings.HeightWeight;
		return value;
	}
}

public class Map {
	public Data[,] Grid;

	public Map() {
		Grid = new Data[Settings.Width, Settings.Height];

		for (int y = 0; y < Settings.Height; y++) {
			for (int x = 0; x < Settings.Width; x++) {
				Grid[x, y] = new Data();
			}
		}
	}

	public void Decay() {
		for (int x = 0; x < Settings.Width; x++) {
			for (int y = 0; y < Settings.Height; y++) {
				Grid[x, y].Decay();
			}
		}
	}

	public int GetNeighbourCount(int xPos, int yPos, int kernel) {
		int half = kernel / 2;
		int count = 0;

		for (int x = xPos - half; x <= xPos + half; x++) {
			for (int y = yPos- half; y <= yPos + half; y++) {
				
				if (IsOutOfBounds(x, y)) {
					continue;
				}

				if (Grid[x, y].IsOccupied) {
					count++;
				}
			}
		}

		return count;
	}

	public bool IsOutOfBounds(int x, int y) {
		if (x < 0 || x > Settings.Width -1 || y < 0 || y > Settings.Height -1) {
			return true;
		}
		return false;
	}
}

