using MQTTnet.Server;
using System;
using System.Linq;

namespace NMqttServer.Mqtt
{
    public interface IMqttInterceptor
    {
        MqttApplicationMessageInterceptorContext GetMessageInterceptorValue(MqttApplicationMessageInterceptorContext context);
        MqttSubscriptionInterceptorContext GetSubscriptionInterceptorValue(MqttSubscriptionInterceptorContext context);
    }
}
