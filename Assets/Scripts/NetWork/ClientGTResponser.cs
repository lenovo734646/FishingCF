
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

public class ClientGTResponser : ClientGTResponserBase
{
    public override void onRecv_CLGTDisconnectNtf(CLGTDisconnectNtf proto)
    {
        MessageCenter.Instance.SendMessage(MsgType.NET_DISCONNECT_NTF, this, proto);
    }
}