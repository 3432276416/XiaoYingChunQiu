using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System.Data;
using System.IO;
using ExcelDataReader;
using System.Text;
using UnityEngine.AddressableAssets;    

public class VNMgr : MonoBehaviour
{

    private bool isTyping=false;
    public TextMeshProUGUI speakerName;
    public TextMeshProUGUI speakerContent;

    public TypeWriter typeWriter; //打印文字的脚本
    private string filePath;
    public List<Content> contents;
    private int contentIndex=0;
    public ObjectEventSO CloseDialogEvent; //关闭对话场景

    private void Start()
    {

        //TeachLaserLevelStory();
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

    void LoadStoryFromFile(string path)  //从路径加载xls文件作为对话
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

    void ShowNextText()  //下一行文字
    {
        if(contentIndex >= contents.Count-1)
        {
            CloseDialogEvent.RaiseEvent(this, this);
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

    void ShowCurText()  //当前文字
    {
        speakerName.text= contents[contentIndex].name;
        speakerContent.text= contents[contentIndex].content;
    }

    void ShowUpperText()  //上一行文字
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

    #region 事件函数
    public void PlayIntroductionStory()
    {
        LoadStoryFromFile(Story.introduction_path);
        ShowCurText();
    }

    public void TeachLaserLevelStory()
    {
        LoadStoryFromFile(Story.laser_describe_path);
        ShowCurText();
    }
    public void SuccessLaserLevelStory()
    {
        LoadStoryFromFile(Story.laser_sucess_path);
        ShowCurText();
    }
    #endregion

}