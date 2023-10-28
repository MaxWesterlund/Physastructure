public class Data {
	public int Height;
	public float SporeStrength;
	public float SolfluxStrength;

	public bool IsOccupied;

	public Data() {
		Height = 0;
		SporeStrength = 0;
		SolfluxStrength = 0;

		IsOccupied = false;

		return;
	}

	public void Decay() {
		SporeStrength *= Settings.DecayRate;
		if (SporeStrength < 0.001) {
			SporeStrength = 0f;
		}
		
		SolfluxStrength *= Settings.DecayRate;
		if (SolfluxStrength < 0.001) {
			SolfluxStrength = 0f;
		}
		return;
	}

	public float Evaluate(int height) {
		int heightDiff = Math.Abs(height - Height);
		float value =  SporeStrength * Settings.SporeWeight + SolfluxStrength * Settings.SolfluxWeight - heightDiff * Settings.HeightWeight;
		return value;
	}
}


