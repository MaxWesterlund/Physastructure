public struct CoordinateData {
    public float SporeStrength;
    public float NodeStrength;
    public bool IsObstacle;
    public bool IsNode;

    public CoordinateData() {
        SporeStrength = 0;
        NodeStrength = 0;
    }

    public float Evaluate() {
        if (IsObstacle) {
            return 0;
        }
        float value = SporeStrength + NodeStrength * 3;
        return value;
    }
}