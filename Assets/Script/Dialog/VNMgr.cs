using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System.IO;
using ExcelDataReader;
using System.Text;
using System;
using UnityEngine.SceneManagement;

public class VNMgr : MonoBehaviour
{

    private bool isTyping=false;
    public TextMeshProUGUI speakerName;
    public TextMeshProUGUI speakerContent;
    public GameObject DialogUI;
    public TypeWriter typeWriter; //��ӡ���ֵĽű�
    private string filePath;
    public List<Content> contents;
    private int contentIndex=0;
  

    private void Awake()
    {
        EventManager.Instance.AddListener(EventName.TeachLaserLevel, TeachLaserLevelStory);
        EventManager.Instance.AddListener(EventName.TeachObstacle, TeachObstacle);
        EventManager.Instance.AddListener(EventName.LaserLevelSuccess, SuccessLaserLevelStory);
        EventManager.Instance.AddListener(EventName.LoadIntroduction, PlayIntroductionStory);
        SetDialogVisible(false);
    }


    private void OnEnable()
    {
        if(!DialogUI)
        {
            DialogUI = GameObject.Find("DialogUI");
        }

        if(this.gameObject.scene==SceneManager.GetSceneByName(SceneName.LaserLevel2))
        {
            EventManager.Instance.RaiseEvent(EventName.TeachObstacle,this.gameObject);
        }
        if (this.gameObject.scene == SceneManager.GetSceneByName(SceneName.LaserLevel1))
        {
            EventManager.Instance.RaiseEvent(EventName.TeachLaserLevel, this.gameObject);
        }
        
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

    void LoadStoryFromFile(string path)  //��·������xls�ļ���Ϊ�Ի�
    {
        FileStream stream=File.Open(path, FileMode.Open, FileAccess.Read);
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
        stream.Dispose();
    }

    void ShowNextText()  //��һ������
    {
        if(contentIndex >= contents.Count-1 && !typeWriter.isTyping) //��������
        {
            SetDialogVisible(false);  
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

    void ShowCurText()  //��ǰ����
    {
        speakerName.text= contents[contentIndex].name;
        speakerContent.text= contents[contentIndex].content;
    }

    void ShowUpperText()  //��һ������
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

    public void SetDialogVisible(bool isShow)
    {
       DialogUI.gameObject.SetActive(isShow);
    }

 

    #region �¼�����
    public void PlayIntroductionStory(object name, EventArgs args)
    {
        LoadStoryFromFile(Story.introduction_path);
        SetDialogVisible(true);
        ShowCurText();
    }

    public void TeachLaserLevelStory(object name, EventArgs args)
    {
        LoadStoryFromFile(Story.laser_describe_path);
        SetDialogVisible(true);
        ShowCurText();
    }
    public void TeachObstacle(object name, EventArgs args)
    {
        LoadStoryFromFile(Story.teach_obstacle_path);
        SetDialogVisible(true);
        ShowCurText();
    }
    public void SuccessLaserLevelStory(object name, EventArgs args)
    {
        SetDialogVisible(true);
        LoadStoryFromFile(Story.laser_sucess_path);
        ShowCurText();
    }
    #endregion

}