using Tomlyn;
using Tomlyn.Model;

static class Settings {
	public static int Width;
	public static int Height;
	public static int Scaling;
	public static int FrameSkip;
	public static bool DrawAgents;
	public static string? SplashText;

	public static float DecayRate;

	public static int AgentCount;
	public static float AgentSpeed;
	
	public static float SensorAngle;
	public static float SensorDistance;
	public static float SensorSize;
	public static float AgentSolflux;

	public static int NodeCount;
	public static float NodeSolflux;

	public static int KernelRadius;
	public static float SolfluxWeight;
	public static float HeightWeight;

	public static void LoadSettings(string path) {
		string tomlString = File.ReadAllText(path);
		TomlTable tomlTable = Toml.ToModel(tomlString);

		// ---------------- window params ------------------ \\
		
		TomlTable screenTable = (TomlTable)tomlTable["window"];
		
		Width = (int)(long)screenTable["Width"];
		Height = (int)(long)screenTable["Height"];

		Scaling = (int)(long)screenTable["Scaling"];
		FrameSkip = (int)(long)screenTable["FrameSkip"];

		DrawAgents = (bool)screenTable["DrawAgents"];
		SplashText = (string)screenTable["SplashText"];

		// ------------------ sim params ------------------ \\

		TomlTable paramsTable = (TomlTable)tomlTable["parameters"];
		
		DecayRate = (float)(double)paramsTable["DecayRate"];
		
		AgentCount = (int)(long)paramsTable["AgentCount"];
		AgentSpeed = (float)(double)paramsTable["AgentSpeed"];
		AgentSolflux = (float)(double)paramsTable["AgentSolflux"];

		NodeCount = (int)(long)paramsTable["NodeCount"];
		NodeSolflux = (float)(double)paramsTable["NodeSolflux"];

		SensorAngle = (float)(double)paramsTable["SensorAngle"];
		SensorDistance = (float)(double)paramsTable["SensorDistance"];
		SensorSize = (float)(double)paramsTable["SensorSize"];
		
		KernelRadius = (int)(long)paramsTable["KernelRadius"];
		SolfluxWeight = (float)(double)paramsTable["SolfluxWeight"];
		HeightWeight = (float)(double)paramsTable["HeightWeight"];
	
		return;
	}
}
