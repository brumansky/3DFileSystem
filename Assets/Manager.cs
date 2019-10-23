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
        /*
        float radius = 2f;
        for(int i = 0; i < 10; i++)
        {
            float angle = i * Mathf.PI * 2f / 8;
            Vector3 newPos = new Vector3(0, Mathf.Cos(angle) * radius, Mathf.Sin(angle) * radius);
            Quaternion Direction = Quaternion.FromToRotation(Vector3.up, newPos);
            Vector3 DirectEuler = Direction.eulerAngles;
            DirectEuler.x += 90;
            Direction.eulerAngles = DirectEuler;
            GameObject go = Instantiate(file, newPos, Direction);
        }
        */
        instantiateInCircle(file, new Vector3(0, 0, 0), 10);
    }

    public void instantiateInCircle(GameObject obj, Vector3 location, int howMany)
    {
        float radius = howMany > 4 ? howMany / 4 : 2;
        for (int i = 0; i < 10; i++)
        {
            float angle = i * Mathf.PI * 2f / 8;
            Vector3 newPos = new Vector3(0, Mathf.Cos(angle) * radius, Mathf.Sin(angle) * radius);
            Quaternion Direction = Quaternion.FromToRotation(Vector3.up, newPos);
            Vector3 DirectEuler = Direction.eulerAngles;
            DirectEuler.x += 90;
            Direction.eulerAngles = DirectEuler;
            GameObject go = Instantiate(obj, newPos, Direction);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
