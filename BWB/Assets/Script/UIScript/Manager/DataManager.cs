using System.Collections.Generic;

public class DataManager
{
    static private DataManager instance = null;

    public static DataManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new DataManager();
            }
            return instance;
        }
    }

    private RoleData _RoleData = new RoleData(); //角色列表信息
    private RoleClass _CurrentRole = new RoleClass(); //当前选择角色
    private ItemData _ItemData = new ItemData(); //道具信息
    private SkillData _SkillData = new SkillData(); //拥有技能信息
    private Dictionary<int, double> _DictBaseAttr = new Dictionary<int, double>(); //基础属性
    private Dictionary<int, string> _DictBaseAttrShow = new Dictionary<int, string>(); //展示面板属性
    private Dictionary<int, double> _DictTotalAttr = new Dictionary<int, double>(); //战斗属性
    private bool _AutoMonster = false;

    public RoleData RoleData
    {
        get
        {
            return _RoleData;
        }
        set
        {
            _RoleData = value;
        }
    }

    public RoleClass CurrentRole
    {
        get
        {
            return _CurrentRole;
        }
        set
        {
            _CurrentRole = value;
        }
    }

    public ItemData ItemData
    {
        get
        {
            return _ItemData;
        }
        set
        {
            _ItemData = value;
        }
    }

    public SkillData SkillData
    {
        get
        {
            return _SkillData;
        }
        set
        {
            _SkillData = value;
        }
    }

    public Dictionary<int, double> DictBaseAttr
    {
        get
        {
            return _DictBaseAttr;
        }
        set
        {
            _DictBaseAttr = value;
        }
    }

    public Dictionary<int, string> DictBaseAttrShow
    {
        get
        {
            return _DictBaseAttrShow;
        }
        set
        {
            _DictBaseAttrShow = value;
        }
    }

    public Dictionary<int, double> DictTotalAttr
    {
        get
        {
            return _DictTotalAttr;
        }
        set
        {
            _DictTotalAttr = value;
        }
    }

    public bool AutoMonster
    {
        get
        {
            return _AutoMonster;
        }
        set
        {
            _AutoMonster = value;
        }
    }
}