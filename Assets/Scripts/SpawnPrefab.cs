using UnityEngine;

public class SpawnPrefab : MonoBehaviour
{
    [SerializeField]
    private GameObject prefab = null;

    void Update()
    {
        CreateGameObject();
    }

    public void CreateGameObject()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            Vector3 mPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mPos.z = 0;
            Instantiate(prefab, mPos, new Quaternion(0, 0, 0, 0));
        }
    }
}