using UnityEngine;

public class Customizer : MonoBehaviour
{
    public static Customizer inst;
    [SerializeField] WeaponDisplay weaponInfo;
    void Awake()
    {
        inst = this;
        weaponInfo.AssignWeapon(ThingsToCarry.inst.RandomWeapon());        
        //rulesInfo.AssignRule(ThingsToCarry.inst.RandomRule());
    }
    public void BeginGame()
    {
        
    }
}
