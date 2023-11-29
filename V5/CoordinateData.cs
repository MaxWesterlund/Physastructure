public struct CoordinateData {
    public float SporeStrength;
    public bool IsNode;

    public CoordinateData() {
        SporeStrength = 0;
        IsNode = false;
    }

    public float Evaluate() {
        float value = IsNode ? 2 : SporeStrength;
        return value;
    }
}