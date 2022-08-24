using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveLevel : MonoBehaviour
{
    [SerializeField] private bool saveLevel;
    [SerializeField] private string levelName = "Level01";

    [SerializeField] private Transform propsRef;
    [SerializeField] private Transform gummingsRef;
    [SerializeField] private Transform exitsRef;

    private List<Transform> props = new List<Transform>();
    private List<Transform> gummings = new List<Transform>();
    private List<Transform> exits = new List<Transform>();

    private SerializableDataToSave serializedDatas = new SerializableDataToSave();

    private void Start()
    {
        Debug.Log("Type of Props Data : " + typeof(PropsData));
    }

    private void Update()
    {
        if (saveLevel)
        {
            saveLevel = false;
            Save();
        }

    }

    public void Save()
    {
        GetChildrenFromGO();
        SaveToJson();
    }

    private void GetChildrenFromGO()
    {
        props = GetChildren(propsRef);
        gummings = GetChildren(gummingsRef);
        exits = GetChildren(exitsRef);
    }

    private List<Transform> GetChildren(Transform goRef)
    {
        List<Transform> children = new List<Transform>();

        foreach (Transform child in goRef)
        {
            children.Add(child);
        }
        return children;
    }

    private void SaveToJson()
    {
        SerializeDatas();        

        var stringJS = JsonUtility.ToJson(serializedDatas);
        string filePath = Application.persistentDataPath + "/" + levelName + ".json";
        //Debug.Log(filePath);

        System.IO.File.WriteAllText(filePath, stringJS);
    }

    private void SerializeDatas()
    {
        serializedDatas.propsDataList = ConvertGOListToEditorDataList<PropsData>(props);
        serializedDatas.gummingsDataList = ConvertGOListToEditorDataList<GummingsData>(gummings);
        serializedDatas.exitsDataList = ConvertGOListToEditorDataList<EditorData>(exits);
    }

    private List<T> ConvertGOListToEditorDataList<T>(List<Transform> inputList) where T: EditorData, new()
    {
        List<T> targetList = new List<T>();

        foreach (Transform go in inputList)
        {
            T targetObject = EditorData.ConvertGOToEditorData<T>(go);
            targetList.Add(targetObject);
        }

        return targetList;
    }
}

[System.Serializable]
public struct SerializableDataToSave
{
    public List<PropsData> propsDataList;
    public List<GummingsData> gummingsDataList;
    public List<EditorData> exitsDataList;
}


