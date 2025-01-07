using UnityEngine;
using UnityEngine.UI;

public class LevelProgressElement : MonoBehaviour
{
    [SerializeField] Image _levelStatusIcon;
    [SerializeField] Image _levelEventIcon;
    [SerializeField] Image _selectedlevelIcon;

    public Image SelectedlevelIcon => _selectedlevelIcon;

    public void ConfigElement(Sprite levelStatusImage, Sprite levelEventImage = null)
    {
        _levelStatusIcon.sprite = levelStatusImage;

        if (levelEventImage != null)
        {
            _levelEventIcon.transform.parent.gameObject.SetActive(true);
            _levelEventIcon.sprite = levelEventImage;
        }
        else
        {
            _levelEventIcon.transform.parent.gameObject.SetActive(false);
        }

        _selectedlevelIcon.gameObject.SetActive(false);
    }
}
