using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooling : MonoBehaviour
{
    public static Dictionary<string, List<GameObject>> objectPools = new Dictionary<string,List<GameObject>>();

    private static int defaultPoolSize = 50;
    private static GameObject poolParent, specificPool;

    private void Awake()
    {
        poolParent = new GameObject();
        poolParent.name = "Pool Empty";
    }

    public static void MakeNewObjectPool(GameObject go)
    {
        specificPool = Instantiate(poolParent);
        specificPool.transform.position = Vector3.zero;
        specificPool.name = go.name + " Pool";

        GameObject temp;
        List<GameObject> list = new List<GameObject>();
        for (int i = 0; i < defaultPoolSize; i++)
        {
            temp = Instantiate(go);
            temp.transform.parent = specificPool.transform;
            temp.SetActive(false);
            list.Add(temp);
        }

        objectPools.Add(go.name, list);
    }

    public static void MakeNewObjectPool(GameObject go, int poolSize)
    {
        specificPool = Instantiate(poolParent);
        specificPool.transform.position = Vector3.zero;
        specificPool.name = go.name + " Pool";

        GameObject temp;
        List<GameObject> list = new List<GameObject>();
        for (int i = 0; i < poolSize; i++)
        {
            temp = Instantiate(go);
            temp.transform.parent = specificPool.transform;
            temp.SetActive(false);
            list.Add(temp);
        }
        objectPools.Add(go.name, list);
    }

    //Returns an object if it has a current pool
    public static GameObject PullObjectFromPool(GameObject go)
    {
        Debug.Log("Pulling " + go.name);
        if (objectPools.ContainsKey(go.name))
        {
            List<GameObject> tempList = new List<GameObject>();
            objectPools.TryGetValue(go.name, out tempList);

            //If the object does not have a list, make a new object list and pull it
            if(tempList == null)
            {
                MakeNewObjectPool(go);
                //PullObjectFromPool(go);
                return null;
            }
            else
            {
                foreach (GameObject gameObject in tempList)
                {
                    if (gameObject.activeInHierarchy == false)
                    {
                        gameObject.SetActive(true);
                        return gameObject;
                    }
                }
            }

            Debug.LogError("ERROR " + go.name + " pool all assets used");
            return null;
        }
        else
        {
            MakeNewObjectPool(go);
            Debug.LogError("ERROR " + go.name + " pool all assets used");
            return null;
        }
    }
}
