using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimulateHall : MonoBehaviour
{
    private void Start()
    {
        DontDestroyOnLoad(gameObject);

        //TableManager.InitData();

        NetController.Instance.GetDownloadUrl((md5) => {
            TableLoadHelper.LoadFromNet(SysDefines.OssUrl, md5, () => {
                ModuleManager.Instance.RegisterAllModules();
                SysDefines.Platform = 2;
                ModuleManager.Instance.Get<LoginModule>().SendNetConnect(1);
            });
        });
    }
}
