using System;
using System.Collections.Generic;
using System.Linq;

public static class EventBus
{
    private static readonly Dictionary<Type, List<ISubscription>> Subscriptions = new();

    public static Guid Subscribe<T>(Action<T> callback) where T : Emitable
    {
        if (!Subscriptions.ContainsKey(typeof(T)))
            Subscriptions.Add(typeof(T), new List<ISubscription>());

        var subscription = new Subscription<T>(callback);
        Subscriptions[typeof(T)].Add(subscription);
        return subscription.SubscriptionGuid;
    }

    public static void RemoveSubscription<T>(Guid subscriptionGuid) where T : Emitable
    {
        Subscriptions.TryGetValue(typeof(T), out var subscriptions);
        if (subscriptions != null)
            Subscriptions[typeof(T)] = subscriptions.Where(it => it.SubscriptionGuid != subscriptionGuid).ToList();
    }

    public static void Publish<T>(T messageToEmit) where T : Emitable
    {
        Subscriptions.TryGetValue(typeof(T), out var subscriptions);
        subscriptions?.ToList().ForEach(it => it.Publish(messageToEmit));
    }

    public static void Clear() => 
            Subscriptions.Clear();
}