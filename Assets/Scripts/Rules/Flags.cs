using System.Collections.Generic;
using UnityEngine;

public class Flags : Rule
{
    [SerializeField] List<float> randomStarts = new();
    [SerializeField] GameObject flag;
    List<GameObject> allFlags = new();
    [SerializeField] int energyGain;
    int currentFlags;
    public override string MyText => AutoTranslate.Flags_Text(GetTime.ToString(), randomStarts.Count.ToString(), energyGain.ToString());
    protected override void Awake()
    {
        base.Awake();
        flag.tag = "PlayerTrigger";
        flag.SetActive(false);

        foreach (float next in randomStarts)
        {
            GameObject nextFlag = Instantiate(flag);
            allFlags.Add(nextFlag);
            nextFlag.SetActive(false);
        }

        ActivateRule();
        EventManager.inst.Subscribe<PlayerTouch>(TouchedFlag);
        void TouchedFlag(PlayerTouch info)
        {
            if (allFlags.Contains(info.obj))
            {
                info.obj.SetActive(false);
                currentFlags--;
                if (currentFlags == 0)
                    Player.instance.ChangeEnergy(energyGain);
            }
        }
    }
    protected override void ActivateRule()
    {
        currentFlags = randomStarts.Count;
        for (int i = 0; i<randomStarts.Count; i++)
        {
            allFlags[i].transform.position = new Vector2(randomStarts[i], WaveManager.RandomY(1f));
            allFlags[i].gameObject.SetActive(true);
        }
    }
}