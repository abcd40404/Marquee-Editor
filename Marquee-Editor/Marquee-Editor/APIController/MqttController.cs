using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;
using System.Text;
using Marquee_Editor.Models;


namespace Marquee_Editor.APIController
{
    public class MqttController : ApiController
    {
        delegate void SetTextCallback(string text);//用來更新UIText 的Callback

        MqttClient client;//MqttClient
        string clientId;//連線時所用的ClientID
        string MosquittoIP = "localhost";

        public IHttpActionResult GetSubscribe(string id)
        {
            client = new MqttClient(MosquittoIP);//MQTTServer在本機
            client.MqttMsgPublishReceived += client_MqttMsgPublishReceived;//當接收到訊息時處理函式
            clientId = Guid.NewGuid().ToString();//取得唯一碼
            client.Connect(clientId);//建立連線

            if (id != "")
            {
                //自訂完整主題名稱
                string Topic = id;

                //設定主題及傳送品質 0 ( 0, 1, 2 )
                client.Subscribe(new string[] { Topic }, new byte[] { 0 });   // we need arrays as parameters because we can subscribe to different topics with one call
                                                                              //清空接收文字
            }
            else
            {

            }
            //System.Diagnostics.Debug.WriteLine("OK");
            return Ok("ok");
        }

        void client_MqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs e)
        {
            //收到的訊息內容以UTF8編碼
            string ReceivedMessage = Encoding.UTF8.GetString(e.Message);

            // we need this construction because the receiving code in the library and the UI with textbox run on different threads
            //將訊息寫進接收訊息框內，但因為MQTT接收的執行緒與UI執行緒不同，我們需要呼叫自訂的SetText函式做些處理
            System.Diagnostics.Debug.WriteLine("Message Received");
            System.Diagnostics.Debug.WriteLine(ReceivedMessage);
        }

        public IHttpActionResult PostPublish(Mqtt mqtt)
        {
            client = new MqttClient(MosquittoIP);//MQTTServer在本機
            client.MqttMsgPublishReceived += client_MqttMsgPublishReceived;//當接收到訊息時處理函式
            clientId = Guid.NewGuid().ToString();//取得唯一碼
            client.Connect(clientId);//建立連線
            //若有輸入發佈主題
            if (mqtt.Topic != "")
            {
                //設定完整的發佈路徑
                string Topic = mqtt.Topic;
                string Text = mqtt.Topic + "|" + mqtt.Type + '|' + mqtt.Data + "|" + mqtt.Date;

                //發佈主題、內容及設定傳送品質 QoS 0 ( 0, 1, 2 )
                client.Publish(Topic, Encoding.UTF8.GetBytes(Text), MqttMsgBase.QOS_LEVEL_AT_MOST_ONCE, true);
            }
            else
            {

            }
            System.Diagnostics.Debug.WriteLine(mqtt.Topic);
            System.Diagnostics.Debug.WriteLine(mqtt.Data);
            return Ok("ok");
        }
    }
}
