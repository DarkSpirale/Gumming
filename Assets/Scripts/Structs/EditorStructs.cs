using UnityEngine;

[System.Serializable]
public class EditorData
{
    protected Vector3 position;

    public EditorData(Vector3 position)
    {
        this.position = position;
    }

    public static EditorData ConvertGOToEditorData(Transform go)
    {
        return new EditorData(go.position);
    }
}

[System.Serializable]
public class PropsData : EditorData
{
    protected int id;
    protected Quaternion rotation;
    protected Vector3 scale;

    public PropsData(Vector3 position, Quaternion rotation, Vector3 scale, int id) : base(position)
    {
        this.id = id;
        this.rotation = rotation;
        this.scale = scale;
    }

    public static new PropsData ConvertGOToEditorData(Transform go)
    {
        return new PropsData(go.position, go.rotation, go.localScale, 1);
    }
}
