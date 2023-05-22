using System;

public interface ISubscription
{
    Guid SubscriptionGuid { get; }
    void Publish(Emitable emitable);
}