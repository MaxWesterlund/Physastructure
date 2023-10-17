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


