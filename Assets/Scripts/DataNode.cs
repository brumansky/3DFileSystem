using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataNode : MonoBehaviour
{
    public string Name;
    public string FullPath;
    public string Extension;
    public String Access;
    public FileAttributes fileAttributes;
    public long Size = 0;
    public long TotalFreeSpace = 0;
    public int Files = 0;
    public int Folders = 0;
    public bool IsFolder = false;
    public bool IsDrive = false;

    public DateTime CreationTime;
    public DateTime LastAccessTime;
    public DateTime LastWriteTime;
}

