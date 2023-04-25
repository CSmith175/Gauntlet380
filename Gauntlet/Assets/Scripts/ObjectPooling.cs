using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooling : MonoBehaviour
{
    public Dictionary<string, List<GameObject>> objectPools = new Dictionary<string,List<GameObject>>();

    private int defaultPoolSize = 50;

    public void MakeNewObjectPool(GameObject go)
    {
        GameObject temp;
        List<GameObject> list = new List<GameObject>();
        for (int i = 0; i < defaultPoolSize; i++)
        {
            temp = Instantiate(go);
            temp.SetActive(false);
            list.Add(temp);
        }

        objectPools.Add(go.name, list);
    }

    public GameObject PullObjectFromPool(GameObject go)
    {
        if (objectPools.ContainsKey(go.name))
        {
            List<GameObject> tempList = new List<GameObject>();
            objectPools.TryGetValue(go.name, out tempList);
            if(tempList == null)
            {
                MakeNewObjectPool(go);
                PullObjectFromPool(go);
                return null;
            }

            foreach (GameObject gameObject in tempList)
            {
                if (gameObject.activeInHierarchy != true)
                    return gameObject;
            }

            Debug.LogError("ERROR " + go.name + " pool all assets used");
            return null;
        }
        else
        {
            Debug.LogError("ERROR " + go.name + " pool all assets used");
            return null;
        }
    }
}
