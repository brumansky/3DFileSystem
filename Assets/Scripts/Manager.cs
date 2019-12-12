using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Manager : MonoBehaviour
{
    List<GameObject> files = new List<GameObject>();
    public List<GameObject> path = new List<GameObject>();
    
    public GameObject file;
    public GameObject folder;
    public GameObject drive;
    public int minPerPage = 6;
    public float cameraOffsetInside = 2.5f;
    public float cameraOffsetOutside = -2.5f;

    public Colors colors;
    public GameObject txtPathNode;
    public GameObject txtHoveredOverNode;
    public Contents properties;
    private DataNode currentSelectedNode;
    
    float radius;
    public float extraSpacing = 1f;
    RaycastHit hitInfo = new RaycastHit();
    
    private GameObject window;
    private GameObject explorer;

    private bool inside = false;
    private bool scroll = true;

    // Start is called before the first frame update
    void Start()
    {
        txtHoveredOverNode.GetComponent<TextMeshProUGUI>().text = "Hovered Node";
        properties.FileName.text = "Select A File";
        
        window = new GameObject("Explorer");
        DirectoryDown("My Computer");
        
        OpenExplorer();
    }

    // Open directory/drive to view child contents
    private void DirectoryDown(String filePath)
    {
        txtPathNode.GetComponent<TextMeshProUGUI>().text = filePath;
        scroll = true;
        GameObject cache = new GameObject(filePath);

        cache.transform.parent = window.transform;
        if (path.Count() > 0)
        {
            path[path.Count() - 1].SetActive(false);
        }
        path.Add(cache);

        explorer = path[path.Count() - 1];
        ResetExplorer();
    }
    
    // Move back to parent directory
    private void DirectoryUp()
    {
        scroll = true;
        if (path.Count() > 1)
        {
            Destroy(explorer);
            ResetExplorer();
            path.RemoveAt(path.Count() - 1);

            if (path.Count() > 0)
            {
                path[path.Count() - 1].SetActive(true);
            }

            explorer = path[path.Count() - 1];

            foreach (Transform child in explorer.transform)
            {
                files.Add(child.gameObject);
            }

            if (files.Count() > 1)
            {
                SetCamera();
            }
            else
            {
                scroll = false;
                SetCamera();
                RotateToFile(files[0]);
            }
            
            txtPathNode.GetComponent<TextMeshProUGUI>().text = explorer.name;
        }
    }
    
    private void OpenExplorer()
    {
        int length = DriveInfo.GetDrives().Length;
        InstantiateInCircle(drive, Vector3.zero, length);
        SetCamera();

        int i = 0;
        foreach (var drive in DriveInfo.GetDrives())
        {
            var child = files[i];
            child.name = drive.Name;
            child.transform.GetComponentInChildren<TextMeshProUGUI>().text = drive.Name;
            colors.ColorObject(child);
       
            DataNode dataNode = child.gameObject.AddComponent<DataNode>();
            dataNode.Name = drive.Name;
            dataNode.FullPath = drive.RootDirectory.FullName;

            if(drive.IsReady)
            {
                dataNode.Size = drive.TotalSize;
                dataNode.TotalFreeSpace = drive.TotalFreeSpace;
                dataNode.IsDrive = true;
            }
            else
            {
                Debug.LogWarning($"{drive.Name}" + "  Not Ready");
                dataNode.Access = "Drive Not Accessible";
            }

            i++;
        }

    }
    
    private void ExpandExplorer(String filePath)
    {
        try
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(filePath);
            IEnumerable<FileInfo> fileList = directoryInfo.EnumerateFiles();
            IEnumerable<DirectoryInfo> folderList = directoryInfo.EnumerateDirectories("*");
            int length = fileList.Count() + folderList.Count();
            
            if (length > 0)
            {
                DirectoryDown(filePath);
                InstantiateInCircle(file, Vector3.zero, length);
                SetCamera();

                try
                {
                    int i = 0;
                    foreach (FileInfo file in fileList)
                    {
                        try
                        {
                            var child = files[i];
                            child.name = file.Name;
                            TextMeshProUGUI[] texts = child.transform.GetComponentsInChildren<TextMeshProUGUI>();

                            texts[0].text = file.Name;
                            texts[1].text = file.Extension;
                            

                            DataNode dataNode = child.gameObject.AddComponent<DataNode>();
                            dataNode.Name = file.Name;
                            dataNode.FullPath = file.FullName;
                            dataNode.Size = file.Length;
                            dataNode.fileAttributes = file.Attributes;
                            dataNode.Extension = file.Extension;
                            dataNode.CreationTime = file.CreationTime;
                            dataNode.LastAccessTime = file.LastAccessTime;
                            dataNode.LastWriteTime = file.LastWriteTime;
                            colors.ColorObject(child);

                            if (file.IsReadOnly)
                            { 
                                dataNode.Access = "ReadOnly";
                            }
                            
                            i++;
                        }
                        catch (UnauthorizedAccessException unAuthTop)
                        {
                            Debug.LogWarning($"{unAuthTop.Message}");
                            currentSelectedNode.Access = "Access Denied";
                        }
                    }
                    foreach (DirectoryInfo directory in folderList)
                    {
                        try
                        {
                            var child = files[i];
                            files[i] = Instantiate(this.folder, child.transform.position, child.transform.rotation, explorer.transform);
                            Destroy(child);
                            child = files[i];
                            
                            child.name = directory.Name;
                            child.transform.GetComponentInChildren<TextMeshProUGUI>().text = directory.Name;
                            colors.ColorChildObjects(child);

                            DataNode dataNode = child.gameObject.AddComponent<DataNode>();
                            dataNode.Name = directory.Name;
                            dataNode.FullPath = directory.FullName;
                            dataNode.IsFolder = true;
                            dataNode.CreationTime = directory.CreationTime;
                            dataNode.LastAccessTime = directory.LastAccessTime;
                            dataNode.LastWriteTime = directory.LastWriteTime;
                        }
                        catch (UnauthorizedAccessException unAuthDir)
                        {
                            Debug.LogWarning($"{unAuthDir.Message}");
                            currentSelectedNode.Access = "Access Denied";
                        }

                        i++;
                    }
                    
                    if (files.Count() == 1)
                    {
                        inside = true;
                        scroll = false;
                        RotateToFile(files[0]);
                    }
                }
                catch (DirectoryNotFoundException dirNotFound)
                {
                    Debug.LogWarning($"{dirNotFound.Message}");
                    currentSelectedNode.Access = "Directory Not Found";
                }
                catch (UnauthorizedAccessException unAuthDir)
                {
                    Debug.LogWarning($"unAuthDir: {unAuthDir.Message}");
                    currentSelectedNode.Access = "Access Denied";
                }
                catch (PathTooLongException longPath)
                {
                    Debug.LogWarning($"{longPath.Message}");
                    currentSelectedNode.Access = "ReadOnly";
                }
            }   
        }         
        catch (DirectoryNotFoundException dirNotFound)
        {
            Debug.LogWarning($"{dirNotFound.Message}");
            currentSelectedNode.Access = "Directory Not Found";
        }
        catch (UnauthorizedAccessException unAuthDir)
        {
            Debug.LogWarning($"unAuthDir: {unAuthDir.Message}");
            currentSelectedNode.Access = "Access Denied";
        }
        catch (PathTooLongException longPath)
        {
            Debug.LogWarning($"{longPath.Message}");
            currentSelectedNode.Access = "ReadOnly";
        }
    }

    public void ReturnToComputer()
    {
        while (path.Count() > 1)
        {
            DirectoryUp();
        }
    }
    
    public void CreateDirectory(TextMeshProUGUI folderName)
    {
        if (!folderName.text.Any())
        {
            folderName.text = "New Folder";
        }
        if (path.Count() > 1)
        {
            String currentPath = path[path.Count() - 1].name;
            String newDirectoryPath = currentPath + "\\" + folderName.text;
            try 
            {
                // Determine whether the directory exists.
                if (Directory.Exists(newDirectoryPath)) 
                {
                    Debug.LogWarning("That path exists already.");
                    return;
                }

                // Try to create the directory.
                Directory.CreateDirectory(newDirectoryPath);
                //Debug.LogWarning("The directory was created successfully at {0}.", Directory.GetCreationTime(newDirectoryPath).ToShortDateString());
                
                
                DirectoryUp();
                ExpandExplorer(currentPath);
            } 
            catch (Exception e) 
            {
                Console.WriteLine("The process failed: {0}", e.ToString());
            }
        } 
    }
    
    public void RenameDirectory(TextMeshProUGUI folderName)
    {
        DataNode targetNode = currentSelectedNode;
        
        if (!folderName.text.Any())
        {
            folderName.text = targetNode.Name;
        }
        if (path.Count() > 1)
        {
            String currentPath = path[path.Count() - 1].name;
            String oldDirectroyPath = targetNode.FullPath;
            String newDirectoryPath = currentPath + "\\" + folderName.text;
            try 
            {
                // Determine whether the directory exists.
                if (Directory.Exists(newDirectoryPath)) 
                {
                    Debug.LogWarning("That path exists already.");
                    return;
                }

                if (!targetNode.IsFolder)
                {
                    newDirectoryPath += targetNode.Extension;
                }

                // Try to create the directory.
                Directory.Move(oldDirectroyPath, newDirectoryPath);
                targetNode.Name = folderName.text;
                targetNode.FullPath = newDirectoryPath;
                
                DirectoryUp();
                ExpandExplorer(currentPath);
                
                GetProperties();
            } 
            catch (Exception e) 
            {
                Console.WriteLine("The process failed: {0}", e.ToString());
            }
        } 
    }

    public void DeleteDirectory()
    {
        if (currentSelectedNode != null)
        {
            DataNode targetNode = currentSelectedNode;
            String currentPath = targetNode.FullPath;
            try 
            {
                // Determine whether the directory exists.
                if (!Directory.Exists(currentPath)) 
                {
                    Console.WriteLine("That path does not exist.");
                    return;
                }

                if (targetNode.IsFolder)
                {
                    // Delete the directory..
                    Directory.Delete(currentPath);
                }
                else
                {
                    // Delete the file..
                    System.IO.File.Delete(currentPath);
                }
                Debug.LogWarning(currentPath + " Deleted");

                currentPath = path[path.Count() - 1].name;
                DirectoryUp();
                ExpandExplorer(currentPath);
            } 
            catch (Exception e) 
            {
                Console.WriteLine("The process failed: {0}", e.ToString());
            }
        } 
    }

    private void ResetExplorer()
    {
        files.RemoveRange(0, files.Count);
        files = new List<GameObject>();
    }
    
    private void Example()
    {
        InstantiateInCircle(file, new Vector3(0, 0, 0), 50);
        SetCamera();
        
        /*
        instantiateInCircle(file, new Vector3(0, 0, 0), 20);
        instantiateInCircle(file, new Vector3(0, 0, 0), 30);
        instantiateInCircle(file, new Vector3(0, 0, 0), 40);
        */
        GameObject toReplace = files[0];

        files[0] = Instantiate(folder, toReplace.transform.position, toReplace.transform.rotation);
        //files[0].transform.Rotate(0, 0, 90);
        Destroy(toReplace);
    }

    private void SetCamera()
    {
        int howMany = explorer.transform.childCount;
        radius = howMany > 4 ? howMany / 4 : 2;
        float cameraOffset = -(2 * radius) + cameraOffsetOutside;

        Camera.main.transform.rotation = Quaternion.Euler(0, 90, 0);
        Camera.main.transform.position = Vector3.zero;
        Camera.main.transform.Translate(0, 0, cameraOffset);
        
        txtHoveredOverNode.SetActive(true);
    }

    private void InstantiateInCircle(GameObject obj, Vector3 location, int howMany)
    {
        radius = howMany > 4 ? howMany / 4 : 2;
        
        if (howMany >= 4)
        {
            radius += extraSpacing;
        }
        else if (howMany < 3)
        {
            radius -= extraSpacing;
        }
        
        for (int i = 0; i < howMany; i++)
        {
            float angle = i * Mathf.PI * 2f / howMany;
            Vector3 newPos = new Vector3(0 + location.x, Mathf.Cos(angle) * radius, Mathf.Sin(angle) * radius);
            Quaternion Direction = Quaternion.FromToRotation(Vector3.up, newPos);
            Vector3 DirectEuler = Direction.eulerAngles;
            DirectEuler.x += 90;
            Direction.eulerAngles = DirectEuler;
            GameObject go = Instantiate(obj, newPos, Direction, explorer.transform);
            files.Add(go);
        }
    }
    
    private void RotateToFile(GameObject fil)
    {
        float zPos = 0;
        txtHoveredOverNode.SetActive(false);
        if (radius > cameraOffsetInside)
        {
            zPos = radius - cameraOffsetInside;
        }
        Camera.main.transform.position = new Vector3(0,0, zPos);
        Camera.main.transform.eulerAngles = new Vector3(0,0,0);
        
        float rot = Mathf.Atan2(fil.transform.position.y, fil.transform.position.z) * 180/Mathf.PI;
        print(rot);
        foreach (GameObject fi in files)
            fi.transform.RotateAround(Vector3.zero, Vector3.right, rot);

        inside = true;
    }
    
    public void BackButtClick()
    {
        if (inside && explorer.transform.childCount > 1)
        {   
            inside = false;
            SetCamera();
        }
        else
        {
            DirectoryUp();
            properties.Panel.GetComponent<Animator>().SetTrigger("Normal");
        }
    }
    
    private String FileSize(long fileSize)
    {
        string[] sizes = { "B", "KB", "MB", "GB", "TB" };
        double len = fileSize;
        int order = 0;
        while (len >= 1024 && order < sizes.Length - 1) {
            order++;
            len = len/1024;
        }

        // Adjust the format string to your preferences. For example "{0:0.#}{1}" would
        // show a single decimal place, and no space.
        return String.Format("{0:0.##} {1}", len, sizes[order]);
    }

    private void GetFolderContents()
    {
        DirectoryInfo directoryInfo = new DirectoryInfo(currentSelectedNode.FullPath);
        currentSelectedNode.Files = directoryInfo.EnumerateFiles().Count();
        currentSelectedNode.Folders = directoryInfo.EnumerateDirectories("*").Count();
    }
    
    private void GetProperties()
    {
        if (currentSelectedNode != null)
        {
            properties.FileName.text = currentSelectedNode.Name;
            properties.FullPath.text = currentSelectedNode.FullPath;
            
            properties.CreateDate.text = currentSelectedNode.CreationTime.ToShortDateString();
            properties.CreateTime.text = currentSelectedNode.CreationTime.ToLongTimeString();
            properties.LastAccessDate.text = currentSelectedNode.LastAccessTime.ToShortDateString();
            properties.LastAccessTime.text = currentSelectedNode.LastAccessTime.ToLongTimeString();
            properties.LastWriteDate.text = currentSelectedNode.LastWriteTime.ToShortDateString();
            properties.LastWriteTime.text = currentSelectedNode.LastWriteTime.ToLongTimeString();
            
            properties.Access.text = currentSelectedNode.Access;

            if (currentSelectedNode.IsDrive)
            {
                properties.Extension.text = "Drive";
                properties.Size.text = FileSize(currentSelectedNode.TotalFreeSpace) 
                                       + " / " + FileSize(currentSelectedNode.Size);
            }
            else if (currentSelectedNode.IsFolder)
            {
                GetFolderContents();
                properties.Extension.text = "Folder";

                if (properties.Access != null)
                {
                    properties.Size.text = "Empty";

                    if (currentSelectedNode.Files > 0)
                    {
                        properties.Size.text = currentSelectedNode.Files + " Files";
                    }
                    else if (currentSelectedNode.Folders > 0)
                    {
                        properties.Size.text = "";
                    }
                    
                    if (currentSelectedNode.Files > 0 && currentSelectedNode.Folders > 0)
                    {
                        properties.Size.text += ", ";
                    }
                    
                    if (currentSelectedNode.Folders > 0)
                    {
                        properties.Size.text += currentSelectedNode.Folders + " Folders";
                    }
                }
                else
                {
                    properties.Size.text = "Empty";
                }
            }
            else
            {
                properties.Extension.text = currentSelectedNode.Extension;
                properties.Size.text = FileSize(currentSelectedNode.Size);
            }
        }
    }
    
    // Update is called once per frame
    void Update()
    {
        if(Input.GetAxis("Mouse ScrollWheel") > 0f && scroll)
        {
            foreach(GameObject fi in files)
            {
                fi.transform.RotateAround(Vector2.zero, Vector3.right, 200 * Time.deltaTime);
            }
        } 
        else if(Input.GetAxis("Mouse ScrollWheel") < 0f && scroll)
        {
            foreach(GameObject fi in files)
            {
                fi.transform.RotateAround(Vector2.zero, Vector3.right, -200 * Time.deltaTime);
            }
        } 
        else if (Input.GetMouseButtonDown(0))
        {
            // Create a raycast from the screen-space into World Space, store the data in hitInfo Object
            bool hit = Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo);
            if (hit)
            {
                print(hitInfo.transform.name);

                // if there is a hit, we want to get the DataNode component to extract the information
                currentSelectedNode = hitInfo.transform.GetComponent<DataNode>();

                if (!inside && files.Count() > minPerPage)
                {
                    RotateToFile(hitInfo.transform.gameObject);
                }
                else  if (hitInfo.transform.GetComponent<DataNode>() != null)
                {
                    // if there is a hit, we want to get the DataNode component to extract the information

                    if (currentSelectedNode.IsFolder || currentSelectedNode.IsDrive)
                    {
                        ExpandExplorer(currentSelectedNode.FullPath);
                        inside = false;
                    }
                    else
                    {
                        RotateToFile(hitInfo.transform.gameObject);
                    }
                }
                GetProperties();
                properties.Panel.GetComponent<Animator>().SetTrigger("Highlighted");
            }
        } 
        
        #region HANDLE MOUSE INTERACTION
        // Create a raycase from the screen-space into World Space, store the data in hitInfo Object
        bool Hoverhit = Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo);
        if (Hoverhit)
        {
            if (hitInfo.transform.GetComponentInChildren<TextMeshProUGUI>() != null)
            {
                // if there is a hit, we want to get the DataNode component to extract the information
                String name = hitInfo.transform.GetComponentInChildren<TextMeshProUGUI>().text;
                txtHoveredOverNode.GetComponent<TextMeshProUGUI>().text = $"{name}";
            }
        }
        else
        {
            txtHoveredOverNode.GetComponent<TextMeshProUGUI>().text = $"";
        }
        #endregion
    }
}
