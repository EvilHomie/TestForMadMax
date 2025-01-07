using TMPro;
using UnityEngine;
using UnityEngine.UI;
using YG;
using YG.Utils.LB;

public class TestLB : MonoBehaviour
{
    string testLBName = "TESTSCORES";

    [SerializeField] Button _ADDScore;
    [SerializeField] TMP_InputField _inputField;


    private void OnEnable()
    {
        _ADDScore.onClick.AddListener(ADDSCORE);
        YandexGame.onGetLeaderboard += OnGetLeaderboard;
    }
    private void OnDisable()
    {
        YandexGame.onGetLeaderboard -= OnGetLeaderboard;
    }
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.G))
        {
            GetLeaderboard();
        }
    }



    void ADDSCORE()
    {
        if (int.TryParse(_inputField.text, out int result))
        {
            Debug.Log(result);
            YandexGame.NewLeaderboardScores(testLBName, result);

        }
    }


    public void GetLeaderboard()
    {
        YandexGame.GetLeaderboard(testLBName, 10, 3, 3, "small");
    }
    
    private void OnGetLeaderboard(LBData lb)
    {
        if (lb.technoName == testLBName)
        {
            foreach (var item in lb.players)
            {
                Debug.Log($"{item.name}  {item.rank}   {item.score}");
            }



        }
    }
}
