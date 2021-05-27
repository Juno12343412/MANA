[System.Serializable]
public class Spring
{
    public float Dampening = 0.1f;
    public float Tension = 0.05f;
    public float BaseHeight; //get; private set;?
    public float Velocity; // get; private set;?
    public float CurrentHeight; // get; private set;?

    public void ApplyAdditiveForce(float force)
    {
        Velocity += force;
    }

    public void ApplyForceStartingAtPosition(float force, float position)
    {
        CurrentHeight = position;
        Velocity = force;
    }

    public float Simulate()
    {
        float heightOffset = BaseHeight - CurrentHeight;
        Velocity += Tension * heightOffset - Velocity * Dampening;
        CurrentHeight += Velocity;

        return CurrentHeight;
    }
}