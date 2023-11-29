using Raylib_cs;

namespace Settings {
    class Window {
        public const int Size = 800;
        public const int Resolution = 1;

        public static Color BackgroundColor = new Color(0, 0, 0, 255);
        public static Color SlimeColor = new Color(245, 224, 43, 255);
        public static Color PointColor = new Color(255, 255, 255, 255);
        public static Color TextColor = new Color(255, 255, 255, 255);
    }

    class Simulation {
        public const int Size = 200;

        public const int PointRadius = 3;

        public const int AgentSpawnRadius = 30;

        public const int AgentAmount = 5000;
        public const float AgentTurnAngle = 0.65f;
        public const float AgentSensorDistance = 5;

        public const float DecayRate = 0.999f;
    }
}