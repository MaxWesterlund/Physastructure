using Raylib_cs;

namespace Settings {
    class Window {
        public const int Size = 800;
        public static int Resolution = 1;

        public const int NodeRadius = 1;

        public static Color BackgroundColor = new Color(0, 0, 0, 255);
        public static Color NodeColor = new Color(255, 255, 255, 255);
        public static Color BorderColor = new Color(20, 20, 20, 255);
        public static Color TextColor = new Color(255, 255, 255, 255);
    }

    class Simulation {
        public const int Size = 200;

        public const int NodeSpreadRadius = 5;

        public const int AgentAmount = 2000;
        public const float AgentTurnAngle = 0.5f;
        public const float AgentSensorDistance = 5;

        public const float DecayRate = 0.999f;
    }
}