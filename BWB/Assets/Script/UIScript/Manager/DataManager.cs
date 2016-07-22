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

    private RoleData _RoleData = new RoleData();
    private RoleClass _CurrentRole = new RoleClass();
    private ItemData _ItemData = new ItemData();
    private SkillData _SkillData = new SkillData();
    private Dictionary<int, double> _DictBaseAttr = new Dictionary<int, double>();
    private Dictionary<int, string> _DictBaseAttrShow = new Dictionary<int, string>();
    private Dictionary<int, double> _DictTotalAttr = new Dictionary<int, double>();

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
}