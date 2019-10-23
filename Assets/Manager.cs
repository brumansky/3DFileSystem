using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour
{
    List<GameObject> files;
    public GameObject file;

    // Start is called before the first frame update
    void Start()
    {
        float radius = 2f;
        for(int i = 0; i < 10; i++)
        {
            float angle = i * Mathf.PI * 2f / 8;
            Vector3 newPos = new Vector3(0, Mathf.Cos(angle) * radius, Mathf.Sin(angle) * radius);
            Vector3 direction = newPos + new Vector3(0, 90, 0);
            GameObject go = Instantiate(file, newPos, Quaternion.FromToRotation(Vector3.up, newPos));
        }

        //instantiateInCircle(file, new Vector3(5, 0, 0), 10);
    }

    public void instantiateInCircle(GameObject obj, Vector3 location, int howMany)
    {
        float radius = howMany > 4 ? howMany / 2 : 2;
        for (int i = 0; i < 10; i++)
        {
            float angle = i * Mathf.PI * 2f / 8;
            Vector3 newPos = new Vector3(0, Mathf.Cos(angle) * radius, Mathf.Sin(angle) * radius);
            GameObject go = Instantiate(obj, newPos, Quaternion.identity);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
