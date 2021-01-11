
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

using JBPROTO;

public class ClientFishingMainResponser : ClientFishingMainResponserBase
{
    public override void onRecv_CLFMEnterServerNtf(CLFMEnterServerNtf proto)
    {
        MessageCenter.Instance.SendMessage(MsgType.NET_ENTERSERVER_NTF, this, proto);
    }
}