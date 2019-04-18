using UnityEngine;

public class AxialSpring : SpringComponent
{
    public Vector3 axis;
    public float dynamicStrength;
    public float staticStrength;
    public float maxDistance = -1f;

    protected override (Vector3, Quaternion) GetPositionRotation(Vector3 position, Quaternion rotation, float deltaTime)
    {
        Vector3 offset = this.position.Get() - position;
        float value = Vector3.Dot(rotation * axis, offset);
        value = Mathf.Lerp(value, 0f, dynamicStrength * deltaTime);
        value = Mathf.MoveTowards(value, 0f, staticStrength * deltaTime);

        if(maxDistance > 0f && Mathf.Abs(value) > maxDistance)
        {
            value = Mathf.Sign(value) * maxDistance;
        }

        return (position + rotation * axis * value, rotation);
    }
}
