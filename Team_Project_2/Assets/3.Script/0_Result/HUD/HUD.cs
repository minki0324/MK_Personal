using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    /*  
        ���� �ؾ� �ϴ� HUD ���
        1. �ð� ���� (����ð�, ���� ���� �ð� ���)
        2. ���� �� ���� (���� ���� ����, �ִ� ���� ���� ���)
        3. ü�� ���� (�÷��̾��� ���� ü��, �ִ� ü�� ����)
        4. ��� (���� ������ �ִ� ��� ���)
        5. ���â (���� ����� �� �ִ� ���� ���� (���׷��̵� ���� �� ����� �� ���� ������ ȸ������ ��µǵ��� ����)
    */

    public enum InfoType
    {
        Time, Soldier, Health, Gold, Employ, TeamPoint, Occupation
    }

    public InfoType type;
    [SerializeField] private Text[] textarray;

    [SerializeField] private Ply_Controller ply;

    [SerializeField] private Slider HP_Slider;

    [SerializeField] private Image SliderImg;
    public Gradient HP_gradient;

    private void LateUpdate()
    {
        switch(type)
        {
            case InfoType.Time:
                textarray[0].text = string.Format("{0}:{1:00}", ((int)GameManager.instance.currentTime / 60), ((int)GameManager.instance.currentTime) % 60); // ���� �ð�
                textarray[1].text = string.Format("{0}:{1:00}", ((int)GameManager.instance.EndTime / 60), ((int)GameManager.instance.EndTime) % 60); // ���� ���� �ð�
                break;
            case InfoType.Soldier:
                textarray[0].text = ply.Max_MinionCount.ToString();         // �ִ� �����
                textarray[1].text = ply.Current_MinionCount.ToString();     // ���� �����
                break;
            case InfoType.Health:
                HP_Slider.value = GameManager.instance.Current_HP / GameManager.instance.Max_Hp;
                textarray[0].color = HP_gradient.Evaluate(GameManager.instance.Current_HP / GameManager.instance.Max_Hp);
                SliderImg.color = HP_gradient.Evaluate(GameManager.instance.Current_HP / GameManager.instance.Max_Hp); // ����ü�� ��� ü�¹� ������
                textarray[0].text = $"{(int)GameManager.instance.Current_HP}<color=FFFFFF>/{(int)GameManager.instance.Max_Hp}</color>";   // ����ü�� / �� ü��
                textarray[1].text = string.Format("+{0:0.00}", GameManager.instance.Regeneration);
                break;
            case InfoType.Gold:
                textarray[0].text = $"���: {(int)GameManager.instance.Gold}";
                break;
            case InfoType.Employ:
                textarray[0].text = $"��80 /20\n��� Ű: <color=#FF3E3E>1</color>\n���: <color=#B7AF3D>15</color>"; // �˻�
                textarray[1].text = $"��90 /20\n��� Ű: <color=#FF3E3E>2</color>\n���: <color=#B7AF3D>20</color>"; // ���
                textarray[2].text = $"��100 /25\n��� Ű: <color=#FF3E3E>3</color>\n���: <color=#B7AF3D>25</color>"; // �ü�
                break;
            case InfoType.TeamPoint:
                // ���� ��� ���� ������ �����غ�����
                textarray[0].text = $"{(int)GameManager.instance.currentTime * 1}";
                textarray[1].text = $"{(int)GameManager.instance.currentTime * 2}";
                textarray[2].text = $"{(int)GameManager.instance.currentTime * 3}";
                textarray[3].text = $"{(int)GameManager.instance.currentTime * 4}";
                break;
            case InfoType.Occupation:
                break;

        }
    }
}
