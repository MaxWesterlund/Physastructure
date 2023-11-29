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
            if (sensor.Spore > bestChoice.Spore) {
                bestChoice = sensor;
            }
        }
        if (bestChoice != null) {
            heading = bestChoice.Angle;
            direction = DirectionFromAngle(heading);
        }
        
        Vector2 wantedPos = position + direction;
        int tries = 0;
        while (!Utils.IsWithinBounds(wantedPos)) {
            Vector2 center = new Vector2(Simulation.Size / 2, Simulation.Size / 2);
            if (tries == 3) {
                wantedPos = center;
                break;
            }
            Vector2 normal = center - wantedPos;
            direction = Vector2.Reflect(direction, normal);
            wantedPos = position + direction;
            tries++;
        }
        
        heading = MathF.Atan2(direction.Y, direction.X);
        position = wantedPos;
    }

    public void LeaveSpore(CoordinateData[,] grid) {
        grid[(int)Math.Clamp(Math.Round(position.X), 0, Simulation.Size - 1), (int)Math.Clamp(Math.Round(position.Y), 0, Simulation.Size - 1)].SporeStrength = 1;
    }

    Vector2 DirectionFromAngle(float a) {
        return new Vector2((float)Math.Cos(a), (float)Math.Sin(a));
    }

    class Sensor {
        public float Angle;
        public float Spore;

        public Sensor(float angle, float spore) {
            Angle = angle;
            Spore = spore;
        }
    }
}