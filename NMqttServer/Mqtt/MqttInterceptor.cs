using LiteDB;
using MQTTnet.Protocol;
using MQTTnet.Server;
using NMqttServer.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NMqttServer.Mqtt
{
    public class MqttInterceptor : IMqttInterceptor
    {
        private ILiteDatabase _db;

        public MqttInterceptor(ILiteDatabase db)
        {
            _db = db;
        }

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
            var q = _db.GetCollection<Device>("devices").FindOne(x => x.ClientId == c.ClientId);

            if(q == null)
            {
                c.ReasonCode = MqttConnectReasonCode.ClientIdentifierNotValid;
                return c;
            }

            if(!q.VerifyCredentials(c.Username, c.Password))
            {
                c.ReasonCode = MqttConnectReasonCode.BadUserNameOrPassword;
                return c;
            }

            c.ReasonCode = MqttConnectReasonCode.Success;
            return c;
        }
    }
}
