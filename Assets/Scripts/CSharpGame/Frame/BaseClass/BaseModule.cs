
/******************************************************************************
 * 
 *  Title:  捕鱼项目
 *
 *  Version:  1.0版
 *
 *  Description:
 *
 *  Author:  WangXingXing
 *       
 *  Date:  2018
 * 
 ******************************************************************************/

public abstract class BaseModule
{
    public enum EnumRegisterMode
    {
        NotRegister,
        AutoRegister,
        AlreadyRegister,
    }

    private EnumObjectState state = EnumObjectState.Initial;
    public event StateChangeEvent StateChanged;
    public EnumObjectState State
    {
        get
        {
            return state;
        }
        set
        {
            if (state == value) return;
            EnumObjectState oldState = state;
            state = value;
            StateChanged?.Invoke(this, state, oldState);
            OnStateChanged(state, oldState);
        }
    }
    protected virtual void OnStateChanged(EnumObjectState newState, EnumObjectState oldState) { }

    private EnumRegisterMode registerMode = EnumRegisterMode.NotRegister;

    public bool AutoRegister
    {
        get
        {
            return registerMode == EnumRegisterMode.NotRegister ? false : true;
        }
        set
        {
            if (registerMode == EnumRegisterMode.NotRegister || registerMode == EnumRegisterMode.AutoRegister)
                registerMode = value ? EnumRegisterMode.AutoRegister : EnumRegisterMode.NotRegister;
        }
    }

    public bool HasRegistered
    {
        get
        {
            return registerMode == EnumRegisterMode.AlreadyRegister;
        }
    }

    public void Load()
    {
        if (State != EnumObjectState.Initial) return;

        State = EnumObjectState.Loading;
        if (registerMode == EnumRegisterMode.AutoRegister)
        {
            ModuleManager.Instance.Register(this);
            registerMode = EnumRegisterMode.AlreadyRegister;
        }

        OnLoad();
        State = EnumObjectState.Ready;
    }

    protected virtual void OnLoad() { }

    public void Release()
    {
        if (State != EnumObjectState.Disabled)
        {
            State = EnumObjectState.Disabled;
            if (registerMode == EnumRegisterMode.AlreadyRegister)
            {
                ModuleManager.Instance.UnRegister(this);
                registerMode = EnumRegisterMode.AutoRegister;
            }
            OnRelease();
        }
    }

    protected virtual void OnRelease() { }
}
