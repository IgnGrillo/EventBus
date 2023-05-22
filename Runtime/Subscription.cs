using System;

public class Subscription<T> : ISubscription where T : Emitable
{
    private readonly Action<T> callback;
        
    public Guid SubscriptionGuid { get; }

    public Subscription(Action<T> callback)
    {
        this.callback = callback;
        SubscriptionGuid = Guid.NewGuid();
    }

    public void Publish(Emitable emitable) =>
            callback(emitable as T);
}