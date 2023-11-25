public class Scene {
	public Data[,] Grid;

	float[,] kernel;

	public Scene() {
		Grid = new Data[Settings.Width, Settings.Height];
		
		for (int y = 0; y < Settings.Height; y++) {
			for (int x = 0; x < Settings.Width; x++) {
				Grid[x, y] = new Data();
			}
		}
		
		GenerateDiffuseKernel();
	}

	void GenerateDiffuseKernel() {
		float sigma = MathF.Max((float)Settings.KernelRadius / 2, 2);
		int kernelWidth = 2 * Settings.KernelRadius + 1;
		kernel = new float[2 * kernelWidth, 2 * kernelWidth];
		float sum = 0f;

		for (int x = -Settings.KernelRadius; x <= Settings.KernelRadius; x++) {
			for (int y = -Settings.KernelRadius; y <= Settings.KernelRadius; y++) {
				float exponentNumerator = (float)-(x * x + y * y);
				float exponentDenominator = 2f * sigma * sigma;

				float eExpression = MathF.Pow(MathF.E, exponentNumerator / exponentDenominator);
				float kernelValue = (eExpression / (2f * MathF.PI * sigma * sigma));

				kernel[x + Settings.KernelRadius, y + Settings.KernelRadius] = kernelValue;
				sum += kernelValue;
			}
		}

		for (int x = 0; x < kernelWidth; x++) {
			for (int y = 0; y < kernelWidth; y++) {
				kernel[x, y] /= sum;
			}
		}
	}

	public void DiffuseLattice() {
		for (int x = 0; x < Settings.Width; x++) {
			for (int y = 0; y < Settings.Height; y++) {
				float value = 0f;

				for (int kx = -Settings.KernelRadius; kx <= Settings.KernelRadius; kx++) {
					for (int ky = -Settings.KernelRadius; ky <= Settings.KernelRadius; ky++) {
						
						float kernelValue = kernel[kx + Settings.KernelRadius, ky + Settings.KernelRadius];
						int offsetX = Clamp(x + kx, 0, Settings.Width -1);
						int offsetY = Clamp(y + ky, 0, Settings.Height -1);
						value += Grid[offsetX, offsetY].Solflux * kernelValue;

					}
				}

				Grid[x, y].NewSolflux = value;

			}
		}
	}

	private int Clamp(int value, int low, int high) {
		return Math.Max(low, Math.Min(value, high));
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
