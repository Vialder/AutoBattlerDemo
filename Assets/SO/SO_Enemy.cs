using UnityEngine;

[CreateAssetMenu(fileName = "SO_Enemy", menuName = "SO/SO_Enemy")]
public class SO_Enemy : ScriptableObject
{
    public string enemyName;
    public Sprite enemySprite;
    public int enemyHP;
    public int enemyAttackDmg;
    public int enemySpeed;
}