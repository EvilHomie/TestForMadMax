using UnityEngine;

public class PosInEnemyGameZone : MonoBehaviour
{
    [SerializeField] int _lineIndex;
    [SerializeField] int _posInLineIndex;
    [SerializeField] bool _isReserved;

    public int LineIndex => _lineIndex;
    public int PosInLineIndex => _posInLineIndex;
    public bool IsReserved
    {
        get
        {
            return _isReserved;
        }
        set { _isReserved = value; }
    }
}
