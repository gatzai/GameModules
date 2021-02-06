using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
原点，方向（向量，点），间隔，力度
*/
public class Ejector : MonoBehaviour
{
    public GameObject bullet;
    public bool PointT_DirxF = false;
    public Transform Target;
    public float Force = 1.0f;

    protected Transform StartEjPos;
    private int InitAmount = 5;
    private GameObject alivePool;
    private GameObject deadPool;
    
    public Transform GetStartEjPos()
    {
        return StartEjPos;
    }
    public void Eject(GameObject go)
    {
        go.transform.position = StartEjPos.position;
        go.transform.rotation = Quaternion.identity;
        Rigidbody rb = go.GetComponent<Rigidbody>();
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        Vector3 dir = Vector3.zero;
        if(PointT_DirxF)
            dir = Vector3.Normalize(Target.position - transform.position);
        else
            dir = Target.right;
        go.transform.forward = dir;
        rb.AddForce(dir * Force, ForceMode.Impulse);
    }

    public void DoEject()
    {
        Eject(GetUsableObj());
    }

    public void Recycle(GameObject go)
    {
        if(go == null)
            return;
        go.transform.SetParent(deadPool.transform);
        go.transform.localPosition = Vector3.zero;
        go.transform.rotation = Quaternion.identity;
        Rigidbody[] rigidbodies = go.GetComponentsInChildren<Rigidbody>();
        foreach(var rb in rigidbodies)
        {
            rb.velocity = Vector3.zero;
        }
        go.SetActive(false);
    }

    GameObject GetUsableObj()
    {
        if(deadPool.transform.childCount <= 0)
        {
            ExpandDeadPool();
            return GetUsableObj();
        }else
        {
            GameObject go = deadPool.transform.GetChild(0).gameObject;
            go.SetActive(true);
            go.transform.SetParent(alivePool.transform);
            go.transform.localPosition = Vector3.zero;
            return go;
        }
    }

    void ExpandDeadPool()
    {
        if(deadPool == null)
        {
            Debug.LogError("DeadPool is NULL");
            return;
        }
        for(int i=0; i < InitAmount; ++i)
        {
            GameObject go = Instantiate(bullet);
            go.transform.SetParent(deadPool.transform);
            go.transform.localPosition = Vector3.zero;
            go.SetActive(false);
        }
    }

    protected void Init()
    {
        alivePool = new GameObject("AlivePool");
        deadPool = new GameObject("DeadPool");
        StartEjPos = new GameObject("Start Eject Point").transform;
        alivePool.transform.SetParent(this.transform);
        deadPool.transform.SetParent(this.transform);
        StartEjPos.SetParent(this.transform);
        alivePool.transform.localPosition = Vector3.zero;
        deadPool.transform.localPosition = Vector3.zero;
        StartEjPos.localPosition = Vector3.zero;
        ExpandDeadPool();
    }


}
