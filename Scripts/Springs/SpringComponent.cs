using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SpringComponent : MonoBehaviour
{
    public SpringComponent parent { get; private set; }
    public readonly List<SpringComponent> children = new List<SpringComponent>();

    private Wrapper<Vector3> _position = new Wrapper<Vector3>();
    private Wrapper<Quaternion> _rotation = new Wrapper<Quaternion>();

    public IReadOnlyWrapper<Vector3> position
    {
        get
        {
            return _position;
        }
    }

    public IReadOnlyWrapper<Quaternion> rotation
    {
        get
        {
            return _rotation;
        }
    }

    public static T Create<T>(string name = "", SpringComponent parent = null) where T : SpringComponent
    {
        return new GameObject().AddComponent<T>()._Init<T>(name, parent);
    }

    private T _Init<T>(string name = "", SpringComponent parent = null) where T : SpringComponent
    {
        this.name = name;
        if(parent != null)
        {
            this.parent = parent;
            parent.children.Add(this);
        }

        transform.parent = parent?.transform;

        return (T)this;
    }

#if UNITY_EDITOR
    public static void AutoUpdate(GameObject gameObject)
    {
        AutoUpdateScan(null, gameObject);
    }

    private static void AutoUpdateScan(SpringComponent parent, GameObject gameObject)
    {
        var comp = gameObject.GetComponent<SpringComponent>();
        if(comp != null)
        {
            comp.parent = parent;

            if(parent != null)
            {
                parent.children.Add(comp);
            }

            comp.children.Clear();
        }
        else
        {
            comp = parent;
        }

        foreach(Transform child in gameObject.transform)
        {
            AutoUpdateScan(comp, child.gameObject);
        }
    }
#endif

    protected abstract (Vector3, Quaternion) GetPositionRotation(Vector3 position, Quaternion rotation, float deltaTime);

    public void Propegate(Vector3 position, Quaternion rotation, float deltaTime = -1f)
    {
        if(deltaTime < 0f)
        {
            deltaTime = Time.deltaTime;
        }
        (position, rotation) = GetPositionRotation(position, rotation, deltaTime);
        _position.Value = position;
        _rotation.Value = rotation;
        foreach(var child in children)
        {
            child.Propegate(position, rotation, deltaTime);
        }
    }

    public IReadOnlyWrapper<Vector3> GetPositionRef()
    {
        return position;
    }
    public IReadOnlyWrapper<Quaternion> GetRotationRef()
    {
        return rotation;
    }
}
