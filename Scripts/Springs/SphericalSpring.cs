using UnityEngine;

public class SphericalSpring : SpringComponent
{
    public float dynamicStrength;
    public float staticStrength;
    public float maxDistance = -1f;

    protected override (Vector3, Quaternion) GetPositionRotation(Vector3 position, Quaternion rotation, float deltaTime)
    {
        Vector3 temp = Vector3.Lerp(this.position.Get(), position, dynamicStrength * deltaTime);
        temp = Vector3.MoveTowards(temp, position, staticStrength * deltaTime);

        if(maxDistance > 0f && Vector3.Distance(position, temp) > maxDistance)
        {
            temp = Vector3.Normalize(temp - position) * maxDistance;
        }

        return (temp, rotation);
    }
}
