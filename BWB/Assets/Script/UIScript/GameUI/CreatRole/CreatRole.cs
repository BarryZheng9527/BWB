using FairyGUI;

public class CreatRole : Window
{
    private GTextInput _RoleName;
    private GButton _RandomBtn;
    private GButton _SureBtn;

    private RoleData _RoleData;
    private RoleClass _CurrentRoleInfo;
    private int _CurrentIndex;

    protected override void OnInit()
    {
        base.OnInit();
        this.contentPane = UIPackage.CreateObject("CreatRole", "CreatRole").asCom;
        this.Center();
        this.modal = false;
        _RoleName = contentPane.GetChild("_RoleName").asTextInput;
        _RoleName.maxLength = 8;
        _RandomBtn = contentPane.GetChild("_RandomBtn").asButton;
        _RandomBtn.onClick.Add(OnRandomName);
        _SureBtn = contentPane.GetChild("_SureBtn").asButton;
        _SureBtn.onClick.Add(OnSure);
        contentPane.GetController("role").onChanged.Add(OnPageChanged);
    }

    protected override void OnShown()
    {
        base.OnShown();
        GameEventHandler.Messenger.AddEventListener(EventConstant.CreatRole, OnCreatRoleResponse);
        GameEventHandler.Messenger.AddEventListener(EventConstant.ChooseRole, OnEnterGameResponse);
        _RoleData = DataManager.Instance.RoleData;
        InitRoleData();
    }

    protected override void OnHide()
    {
        base.OnHide();
        GameEventHandler.Messenger.RemoveEventListener(EventConstant.CreatRole, OnCreatRoleResponse);
        GameEventHandler.Messenger.RemoveEventListener(EventConstant.ChooseRole, OnEnterGameResponse);
    }

    /*
     * 初始化默认选中角色
     */
    private void InitRoleData()
    {
        _CurrentIndex = 0;
        double LastOffLineTime = 0;
        for (int iIndex = 0; iIndex < _RoleData.RoleList.Count; ++iIndex)
        {
            RoleClass role = _RoleData.RoleList[iIndex];
            if (role.LastOffLineTime >= LastOffLineTime)
            {
                _CurrentIndex = role.Job - 1;
                LastOffLineTime = role.LastOffLineTime;
            }
        }
        contentPane.GetController("role").SetSelectedIndex(_CurrentIndex);
        UpdateShowInfo();
    }

    /*
     * 更新选中角色信息
     */
    private void UpdateShowInfo()
    {
        _CurrentRoleInfo = GetRoleInfo(_CurrentIndex);
        if (_CurrentRoleInfo == null)
        {
            _RandomBtn.visible = true;
            _SureBtn.text = LanguageConfig.Instance.GetText("Text_100001");
            _RoleName.editable = true;
            OnRandomName();
        }
        else
        {
            _RandomBtn.visible = false;
            _SureBtn.text = LanguageConfig.Instance.GetText("Text_100002");
            _RoleName.editable = false;
            _RoleName.text = "Lv." + _CurrentRoleInfo.Level + "" + _CurrentRoleInfo.Name;
        }
    }

    /*
     * 获取对应角色位的角色信息
     */
    private RoleClass GetRoleInfo(int iTargetIndex)
    {
        for (int iIndex = 0; iIndex < _RoleData.RoleList.Count; ++iIndex)
        {
            RoleClass role = _RoleData.RoleList[iIndex];
            if (iTargetIndex == (role.Job - 1))
            {
                return role;
            }
        }
        return null;
    }

    /*
     * 随机名
     */
    private void OnRandomName()
    {
        bool isMale = _CurrentIndex < 1 ? true : false;
        _RoleName.text = NameConfig.Instance.GetRandomName(isMale);
    }

    /*
     * 更新当前页信息
     */
    private void OnPageChanged()
    {
        _CurrentIndex = contentPane.GetController("role").selectedIndex;
        UpdateShowInfo();
    }

    /*
     * 创建角色/进入游戏
     */
    private void OnSure()
    {
        if (_CurrentRoleInfo == null)
        {
            string roleName = _RoleName.text;
            if (roleName != "")
            {
                NetManager.Instance.CreatRoleRequest(roleName, _CurrentIndex + 1);
            }
        }
        else
        {
            NetManager.Instance.EnterGameRequest(_CurrentRoleInfo.Name);
        }
    }

    /*
     * 创角结果
     */
    public void OnCreatRoleResponse(EventContext context)
    {
        CreatRoleResponse response = context.data as CreatRoleResponse;
        OnPageChanged();
    }

    /*
     * 进入游戏结果
     */
    private void OnEnterGameResponse()
    {
        Hide();
        GUIManager.Instance.OpenMainUI();
    }
}