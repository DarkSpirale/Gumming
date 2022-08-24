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

    private List<PropsData> propsDataList = new List<PropsData>();

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
        //propsDataList = ConvertGOListToPropsDataList(props);
        propsDataList = ConvertGOListToEditorDataList<PropsData>(props);

        var propsJS = new System.Text.StringBuilder();

        foreach(PropsData prop in propsDataList)
        {
            propsJS.AppendLine(JsonUtility.ToJson(prop));
        }

        var propsJS2 = JsonUtility.ToJson(propsDataList[0]);

        string filePath = Application.persistentDataPath + "/" + levelName + ".json";
        Debug.Log(filePath);

        System.IO.File.WriteAllText(filePath, propsJS2);
    }

    private List<PropsData> ConvertGOListToPropsDataList(List<Transform> listGO)
    {
        List<PropsData> targetList = new List<PropsData>();

        foreach(Transform go in listGO)
        {
            targetList.Add(PropsData.ConvertGOToEditorData(go));
        }

        return targetList;
    }

    
    private List<T> ConvertGOListToEditorDataList<T>(List<Transform> inputList) where T: EditorData
    {
        List<T> targetList = new List<T>();

        foreach (Transform go in inputList)
        {
            T targetObject = typeof(T).GetMethod("ConvertGOToEditorData").Invoke(null, new object[] { go }) as T;
            targetList.Add(targetObject);
        }

        return targetList;
    }
    
}



