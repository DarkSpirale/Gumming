using UnityEngine;

[System.Serializable]
public class EditorData
{
    public Vector3 position;

    public EditorData() { }

    public static T ConvertGOToEditorData<T>(Transform go) where T: EditorData, new()
    {
        T newEditorData = new T();
        newEditorData.Initialize(go);
        return newEditorData;
    }

    protected virtual void Initialize(Transform go)
    {
        position = go.position;
    }
}

[System.Serializable]
public class PropsData : EditorData
{
    public int id;
    public Quaternion rotation;
    public Vector3 scale;

    public PropsData() { }

    protected override void Initialize(Transform go)
    {
        base.Initialize(go);

        id = 1;
        rotation = go.rotation;
        scale = go.localScale;
    }
}

[System.Serializable]
public class GummingsData : EditorData
{
    public int facingDirection = 1;

    public GummingsData() { }

    protected override void Initialize(Transform go)
    {
        base.Initialize(go);

        GummingEditor gumming = go.GetComponent<GummingEditor>();

        facingDirection = gumming.FacingDirection;
    }
}
