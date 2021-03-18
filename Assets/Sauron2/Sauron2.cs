using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;


public class Sauron2 : MonoBehaviour
{
    // GO has Example1 script assigned to it so is Cube1
    public GameObject GO;

    void Start()
    {
        Debug.Log("Example2");
        //TODO: Set up scene
        // generate map
        // set number 
    }

    // allow Cube1 to activated just once
    private bool activateGO = true;
/*
    IEnumerator Upload()
    {
        List<IMultipartFormSection> formData = new List<IMultipartFormSection>();
        formData.Add(new MultipartFormDataSection("field1=foo&field2=bar"));
        formData.Add(new MultipartFormFileSection("my file data", "myfile.txt"));

        UnityWebRequest www = UnityWebRequest.Post("http://www.my-server.com/myform", formData);
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(www.error);
        }
        else
        {
            Debug.Log("Form upload complete!");
        }
    }
  */  
    void Update()
    {
        if (activateGO == true)
        {
            if (Input.GetKeyDown("space"))
            {
                Debug.Log("space key was pressed");
                GO.SetActive(true);
                activateGO = false;
            }
        }
    }
}
