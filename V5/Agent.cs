using System.Numerics;
using Settings;

public class Agent {
    Vector2 position;
    Vector2 direction;
    float heading;

    Random random;

    public Agent(int x, int y, float head) {
        position = new Vector2(x, y);
        heading = head;
        direction = DirectionFromAngle(heading);

        random = new Random();
    }

    public void Move(CoordinateData[,] grid) {
        Sensor[] sensors = new Sensor[3];
        for (int i = 0; i < sensors.Length; i++) {
            float angle = heading - Simulation.AgentTurnAngle + i * Simulation.AgentTurnAngle;
            Vector2 p = position + Simulation.AgentSensorDistance * DirectionFromAngle(angle);
            int x = Math.Clamp((int)Math.Round(p.X), 0, Simulation.Size - 1);
            int y = Math.Clamp((int)Math.Round(p.Y), 0, Simulation.Size - 1);

            float value = grid[x, y].Evaluate();

            Sensor sensor = new Sensor(
                angle,
                value
            );

            sensors[i] = sensor;
        }

        Sensor bestChoice = sensors[1];
        for (int i = 0; i < sensors.Length; i++) {
            Sensor sensor = sensors[i];
            if (sensor.Pheremone > bestChoice.Pheremone) {
                bestChoice = sensor;
            }
        }
        if (bestChoice != null) {
            heading = bestChoice.Angle;
            direction = DirectionFromAngle(heading);
        }
        
        Vector2 wantedPos = position + direction;
        while (!Utils.IsWithinBounds(wantedPos)) {
            Vector2 center = new Vector2(Simulation.Size / 2, Simulation.Size / 2);
            Vector2 normal = wantedPos - center;
            direction = Vector2.Reflect(direction, normal);
            wantedPos = position + direction;
        }
        
        heading = MathF.Atan2(direction.Y, direction.X);
        position = wantedPos;
    }

    public void LeaveSpore(CoordinateData[,] grid) {
        grid[Math.Clamp((int)Math.Round(position.X), 0, Simulation.Size - 1), Math.Clamp((int)Math.Round(position.Y), 0, Simulation.Size - 1)].Strength = 1;
    }

    Vector2 DirectionFromAngle(float a) {
        return new Vector2((float)Math.Cos(a), (float)Math.Sin(a));
    }

    class Sensor {
        public float Angle;
        public float Pheremone;

        public Sensor(float angle, float pheremone) {
            Angle = angle;
            Pheremone = pheremone;
        }
    }
}