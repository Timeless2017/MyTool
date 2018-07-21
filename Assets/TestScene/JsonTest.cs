using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class JsonTest : MonoBehaviour {

    private Button btn_Save, btn_Load;
    private InputField path, text;

	void Start () {
        //string jsonStr = ResourcesManager.Instance.LoadJson("JsonTest");
        //Debug.Log(jsonStr);

        //ResourcesManager.Instance.SaveJson("01/02/MyJson", "你好");

        path = gameObject.GetChildComponet<InputField>("Input_Path");
        text = gameObject.GetChildComponet<InputField>("Input_Text");
        btn_Save = gameObject.GetChildComponet<Button>("Btn_Save");
        btn_Save.onClick.AddListener(() => ResourcesManager.Instance.SaveJson(path.text, text.text));
        btn_Load = gameObject.GetChildComponet<Button>("Btn_Load");
        btn_Load.onClick.AddListener(() =>
        {
            Debug.Log("btn_Load");
            string jsonText = ResourcesManager.Instance.LoadJson(path.text);
            if (jsonText == "")
                text.text = "找不到该路径";
            else
                text.text = jsonText;
        });

	}
	

}
