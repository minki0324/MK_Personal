using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class IntroScene : MonoBehaviour
{
    private void Awake()
    {
        // ���� ������ ������ ��Ŀ���� ���� �ʾƵ� ������ ����� �� �ֵ��� �ϴ� ����
        Application.runInBackground = true;

        // ������ ���� ���� �Ͽ����� Stage�� �ε����� 0���� �ʱ�ȭ
        PlayerPrefs.SetInt("StageIndex", 0);

        // ��ó���� : ���� �����ϱ� ���� ���� �����ϴ� ��
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
