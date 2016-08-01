using FairyGUI;
using System.Collections.Generic;

public class BattleManager
{
    static private BattleManager instance = null;

    public static BattleManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new BattleManager();
            }
            return instance;
        }
    }

    private Dictionary<int, double> _DictTotalAttr;
    private List<SkillClass> _MySkillList;
    private SkillClass _CurSkill;
    private int _CurStatus;

    public void BattleManagerStart()
    {
        //Timers.inst.Add(0.1f, 0, UpdateTaskTab);
    }
}