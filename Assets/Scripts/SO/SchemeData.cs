using UnityEngine;

public class SchemeData : ScriptableObject
{
    public int copperAmountForUnlock;
    public int wiresAmountForUnlock;
    public int scrapMetalAmountForUnlock;

    public virtual string SchemeName { get; }
    public virtual string ItemNameInScheme { get; }
}
