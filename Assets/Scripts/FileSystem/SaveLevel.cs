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

    public List<Transform> props = new List<Transform>();

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
        serializedDatas.propsDataList = ConvertGOListToEditorDataList<PropsData>(props);

        var stringJS = JsonUtility.ToJson(serializedDatas);

        /*var propsJS = new System.Text.StringBuilder();

        foreach(PropsData prop in propsDataList)
        {
            propsJS.AppendLine(JsonUtility.ToJson(prop));
        }*/

        string filePath = Application.persistentDataPath + "/" + levelName + ".json";
        Debug.Log(filePath);

        System.IO.File.WriteAllText(filePath, stringJS);
    }

    private List<T> ConvertGOListToEditorDataList<T>(List<Transform> inputList) where T: EditorData
    {
        List<T> targetList = new List<T>();

        foreach (Transform go in inputList)
        {
            //T targetObject = typeof(T).GetMethod("ConvertGOToEditorData").Invoke(null, new object[] { go }) as T;
            //targetList.Add(targetObject);
            targetList.Add(EditorData.ConvertGOToEditorData<T>(go));
        }

        return targetList;
    }
    
}

[System.Serializable]
public struct SerializableDataToSave
{
    public List<PropsData> propsDataList;
}


