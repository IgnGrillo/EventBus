# Crispy Event Bus

*Simple implementation of an event bus system in Unity.* 

**Overview**

To start using this event bus, you will need to create a class that inherits from the **Emitable** class.

Using the **Subscribe** method, you will be receiving all the emissions of the child class of the Emitable you have created and subscribed, being able to read any parameter that is available of that class.

Remember that the emission only happens when you call the **Publish** method. 

Any methods previously subscribed, will be called in call order

Finally, If you want, you can remove any subscriptions with the **Remove Subscription** method.\

Just remember to store the ID provided with the **Subscribe** method to provide the **Remove Subscription** call successfully.

**Implementation Example**

You can create a new Emitable class

    private class OnPlayerInitialized : Emitable  
    {  
      public int Life { get; }  
      public int Strength { get; }  
      
      public OnPlayerInitialized(int life, int strength)  
     {  
	     Life = life;  
	     Strength = strength;  
     }
    }

We can now proceed to create an action to receive all the OnPlayerInitialized emissions

    var subscriptionGUID = EventBus.Subscribe<OnPlayerInitialized>(initialized => RefreshPlayersLifeUI(initialized.Life))

This way we are receiving the values provided when the function Published is called

    EventBus.Publish(new OnPlayerInitialized(life: 100, strength: 10));
	
Whenever we are done we can just call the RemoveSubscription method

	EventBus.RemoveSubscription<OnPlayerInitialized>(subscriptionGUID));
