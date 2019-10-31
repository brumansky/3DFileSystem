using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour
{
    List<GameObject> files = new List<GameObject>();
    public GameObject file;
    public GameObject folder;


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
        GameObject toReplace = files[0];
        
        files[0] = Instantiate(folder, toReplace.transform.position, toReplace.transform.rotation);
        //files[0].transform.Rotate(0, 0, 90);
        Destroy(toReplace);
        

    }

    public void instantiateInCircle(GameObject obj, Vector3 location, int howMany)
    {
        float radius = howMany > 4 ? howMany / 4 : 2;
        for (int i = 0; i < howMany; i++)
        {
            float angle = i * Mathf.PI * 2f / howMany;
            Vector3 newPos = new Vector3(0, Mathf.Cos(angle) * radius, Mathf.Sin(angle) * radius);
            Quaternion Direction = Quaternion.FromToRotation(Vector3.up, newPos);
            Vector3 DirectEuler = Direction.eulerAngles;
            DirectEuler.x += 90;
            Direction.eulerAngles = DirectEuler;
            GameObject go = Instantiate(obj, newPos, Direction);
            files.Add(go);
        }
    }

    GameObject SelectedFile;
    RaycastHit hitInfo = new RaycastHit();
    // Update is called once per frame
    void Update()
    {
        if(Input.GetAxis("Mouse ScrollWheel") > 0f)
        {
            foreach(GameObject fi in files)
            {
                fi.transform.RotateAround(Vector2.zero, Vector3.right, 200 * Time.deltaTime);
            }
        } else if(Input.GetAxis("Mouse ScrollWheel") < 0f)
        {
            foreach(GameObject fi in files)
            {
                fi.transform.RotateAround(Vector2.zero, Vector3.right, -200 * Time.deltaTime);
            }
        } else if (Input.GetMouseButtonDown(0))
        {
            // Create a raycase from the screen-space into World Space, store the data in hitInfo Object
            bool hit = Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo);
            if (hit)
            {
                // if there is a hit, we want to get the DataNode component to extract the information
                print(hitInfo.transform.name);
            }
        }

    }
}
