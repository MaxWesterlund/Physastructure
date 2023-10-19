using Raylib_cs;
using System.Numerics;
using System.Security.Cryptography;

public class Agent {
    float heading;
    Vector2 pos = new(Settings.Size / 2, Settings.Size / 2);
    Vector2 dir;

    public Agent(float a) {
        heading = a;
        dir = directionFromAngle(a);
    }

    public void Move() {
        Sensor[] sensors = new Sensor[3];
        for (int i = 0; i < sensors.Length; i++) {
            float a = heading - Settings.AgentTurnRad + i * Settings.AgentTurnRad;
            Vector2 p = Settings.AgentSensorDist * directionFromAngle(a);
            int x = Math.Clamp((int)p.X, 0, Settings.Size - 1);
            int y = Math.Clamp((int)p.Y, 0, Settings.Size - 1);
            Sensor s = new Sensor(
                a,
                Map.PheremoneMap[x, y],
                Map.DiscoveredMap[x, y]
            );

            sensors[i] = s;
        }

        Sensor bestChoice = sensors[0];
        for (int i = 1; i < sensors.Length; i++) {
            Sensor s = sensors[i];
            if (!s.Discovered) {
                bestChoice = s;
            }
        }
        dir = directionFromAngle(bestChoice.Angle);

        if (pos.X < 0 || pos.X >= Settings.Size - 1) {
            dir.X *= -1;
        }
        if (pos.Y < 0 || pos.Y >= Settings.Size - 1) {
            dir.Y *= -1;
        }
        
        Vector2 wantedPos = pos + Settings.AgentSpeed * dir;
        // Map.DiscoveredMap[(int)wantedPos.X, (int)wantedPos.Y] = true;

        // Map.OccupiedMap[(int)pos.X, (int)pos.Y] = false;
        // Map.OccupiedMap[(int)wantedPos.X, (int)wantedPos.Y] = true;
        
        pos = wantedPos;
    }

    public void LeaveSpore() {
        int x = (int)pos.X;
        int y = (int)pos.Y;
        if (x < 0 || x >= Settings.Size || y < 0 || y >= Settings.Size) return;
        
        Map.PheremoneMap[x, y] = 1;
    }

    Vector2 directionFromAngle(float a) {
        return new Vector2((float)Math.Cos(a), (float)Math.Sin(a));
    }

    struct Sensor {
        public float Angle;
        public float Pheremone;
        public bool Discovered;

        public Sensor(float angle, float pheremone, bool discovered) {
            Angle = angle;
            Pheremone = pheremone;
            Discovered = discovered;
        }
    }
}