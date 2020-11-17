using MQTTnet.Protocol;
using MQTTnet.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NMqttServer.Mqtt
{
    public class MqttInterceptor : IMqttInterceptor
    {
        public MqttApplicationMessageInterceptorContext GetMessageInterceptorValue(MqttApplicationMessageInterceptorContext context)
        {
            if (!context.ApplicationMessage.Topic.StartsWith("test"))
            {
                context.AcceptPublish = false;
            }
            if(context.ApplicationMessage.Payload == null)
            {
                context.AcceptPublish = false;
            }
            return context;
        }

        public MqttSubscriptionInterceptorContext GetSubscriptionInterceptorValue(MqttSubscriptionInterceptorContext context)
        {
            if (context.TopicFilter.Topic.StartsWith("test"))
            {
                context.AcceptSubscription = true;
            }
            else
            {
                context.AcceptSubscription = false;
            }
            return context;
        }

        public MqttConnectionValidatorContext Authentication(MqttConnectionValidatorContext c)
        {
            if (c.ClientId.Length < 10)
            {
                c.ReasonCode = MqttConnectReasonCode.ClientIdentifierNotValid;
                return c;
            }

            if (c.Username != "mySecretUser")
            {
                c.ReasonCode = MqttConnectReasonCode.BadUserNameOrPassword;
                return c;
            }

            if (c.Password != "mySecretPassword")
            {
                c.ReasonCode = MqttConnectReasonCode.BadUserNameOrPassword;
                return c;
            }

            c.ReasonCode = MqttConnectReasonCode.Success;
            return c;
        }
    }
}
