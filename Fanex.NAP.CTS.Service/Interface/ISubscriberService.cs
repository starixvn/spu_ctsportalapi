namespace Fanex.NAP.CTS.Service.Interface
{
    using Fanex.NAP.CTS.Model.API.FPS.Subscriber;
    using Fanex.NAP.CTS.Model.Subscriber;

    public interface ISubscriberService
    {
        SubscriberInfo GetSubscriberBySubscriberName(string subscriberName);

        SubscriberFpsInfo GetSubscriberBySubscriberName_DCSExtra(string subscriberName);

    }
}
