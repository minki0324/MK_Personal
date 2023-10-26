using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class IntroScene : MonoBehaviour
{
    private void Awake()
    {
        // 현재 게임이 윈도우 포커스가 되지 않아도 게임이 실행될 수 있도록 하는 변수
        Application.runInBackground = true;

        // 게임을 새로 시작 하였을때 Stage의 인덱스를 0으로 초기화
        PlayerPrefs.SetInt("StageIndex", 0);

        // 전처리기 : 무언가 수행하기 전에 먼저 실행하는 것
#if UNITY_EDITOR_WIN
        DirectoryInfo directory = new DirectoryInfo(Application.streamingAssetsPath);
        StageControl.MaxStageCount = directory.GetFiles().Length / 2;
#elif PLATFORM_STANDALONE_WIN
        StageControl.MaxStageCount = directory.GetFiles().Length;
#endif
    }

    private void Update()
    {
        if(Input.anyKeyDown)
        {
            SceneLoader.LoadScene("MainGame");
        }
    }
}
