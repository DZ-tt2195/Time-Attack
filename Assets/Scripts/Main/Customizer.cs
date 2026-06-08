using UnityEngine;

public class Customizer : MonoBehaviour
{
    public static Customizer inst;
    [SerializeField] WeaponDisplay weaponInfo;
    void Awake()
    {
        inst = this;
    }
    public void BeginGame()
    {
        
    }
}
