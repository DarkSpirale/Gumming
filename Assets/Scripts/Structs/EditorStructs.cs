using UnityEngine;

[System.Serializable]
public class EditorData
{
    public Vector3 position;

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
    public int id;
    public Quaternion rotation;
    public Vector3 scale;

    public PropsData(Vector3 position, Quaternion rotation, Vector3 scale, int id) : base(position)
    {
        this.id = id;
        this.rotation = rotation;
        this.scale = scale;
    }

    public static new PropsData ConvertGOToEditorData(Transform go)
    {
        //TODO: getID
        int id = 1;

        return new PropsData(go.position, go.rotation, go.localScale, id);
    }
}

[System.Serializable]
public class GummingsData : EditorData
{
    public int facingDirection = 1;

    public GummingsData(Vector3 position, int facingDirection) : base(position)
    {
        this.facingDirection = facingDirection;
    }

    public static new GummingsData ConvertGOToEditorData(Transform go)
    {
        GummingEditor gumming = go.GetComponent<GummingEditor>();

        return new GummingsData(go.position, gumming.FacingDirection);
    }
}
