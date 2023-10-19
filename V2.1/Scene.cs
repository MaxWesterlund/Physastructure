public class Scene {
	public Data[,] Grid;

	public Scene() {
		Grid = new Data[Settings.Size, Settings.Size];

		for (int y = 0; y < Settings.Size; y++) {
			for (int x = 0; x < Settings.Size; x++) {
				Grid[x, y] = new Data();
			}
		}
	}

	public void Decay() {
		for (int x = 0; x < Settings.Size; x++) {
			for (int y = 0; y < Settings.Size; y++) {
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
		if (x < 0 || x > Settings.Size -1 || y < 0 || y > Settings.Size -1) {
			return true;
		}
		return false;
	}
}
