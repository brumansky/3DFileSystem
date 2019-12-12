using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Contents : MonoBehaviour
{
    public TextMeshProUGUI FileName;
    public TextMeshProUGUI FullPath;
    public TextMeshProUGUI Extension;
    public TextMeshProUGUI Size;
    public TextMeshProUGUI Access;
    public TextMeshProUGUI CreateDate;
    public TextMeshProUGUI CreateTime;
    public TextMeshProUGUI LastAccessDate;
    public TextMeshProUGUI LastAccessTime;
    public TextMeshProUGUI LastWriteDate;
    public TextMeshProUGUI LastWriteTime;
    public GameObject Panel;
    
    // Start is called before the first frame update
    void Start()
    {
    }
}
