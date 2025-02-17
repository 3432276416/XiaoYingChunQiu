using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System.Data;
using System.IO;
using ExcelDataReader;
using System.Text;

public class VNMgr : MonoBehaviour
{


    private bool isTyping=false;
    public TextMeshProUGUI speakerName;
    public TextMeshProUGUI speakerContent;

    public TypeWriter typeWriter;
    private string filePath;
    public List<Content> contents;
    private int contentIndex=0;

    private void Awake()
    {
        LoadStoryFromFile(Story.introduction_path);
        ShowCurText();
    }
    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        { 
            ShowNextText();
        }
        if(Input.GetMouseButtonDown(1))
        {
            ShowUpperText();
        }
    }

    void LoadStoryFromFile(string path)
    {
        FileStream stream=File.Open(Story.introduction_path, FileMode.Open, FileAccess.Read);
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);


        IExcelDataReader reader= ExcelReaderFactory.CreateReader(stream);
         contents = new List<Content>();
        do
        {
            while (reader.Read())
            {
                Content content=new Content(reader.GetString(0),reader.GetString(1));
                Debug.Log(content.content);
                contents.Add(content);
            }

        }while (reader.NextResult());
        contentIndex = 0;
    }

    void ShowNextText()
    {
        if(contentIndex >= contents.Count-1)
        {
            return;
        }
        if(typeWriter.isTyping)
        {
            typeWriter.CompleteLine();
        }
        else
        {
            contentIndex++;
            speakerName.text = contents[contentIndex].name;
            speakerContent.text = contents[contentIndex].content;
            typeWriter.textDisplay = speakerContent;
            typeWriter.StartTyping(speakerContent.text);
        }

    }

    void ShowCurText()
    {
        speakerName.text= contents[contentIndex].name;
        speakerContent.text= contents[contentIndex].content;
    }

    void ShowUpperText()
    {
        if (contentIndex <=0)
        {
            return;
        }

        if (typeWriter.isTyping)
        {
            typeWriter.CompleteLine();
        }
        else
        {
            contentIndex--;
            speakerName.text = contents[contentIndex].name;
            speakerContent.text = contents[contentIndex].content;
            typeWriter.textDisplay = speakerContent;
            typeWriter.StartTyping(speakerContent.text);
        }
     
    }
    
}