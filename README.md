# C# Multiton Pure MVC For Unity
The famous AS3 MVC comes to LIFE with this reposory!!

I usualy have a static class like this:


    using JHSEngine.Interfaces;
    public static class MainComponent
    {
      static public IFacade Game;
    }


Then I have a class that represend the main mediator:

    public class Game : Mediator
    {
        [Header("Mediators")]
        [SerializeField()]
        public GameObject[] AllMediators; // Add here the game objects that has scripts that extends Mediators

        public void Load()
        {
            MainComponent.Game = Facade.GetInstance("Game");
            MainComponent.Game.RegisterMediator(this);       
        }

        public override void OnRegister()
        {
            RegisterCommands();

            for (int i = 0; i < AllMediators.Length; i++)
            {
                IMediator med = AllMediators[i].GetComponent<IMediator>();
                if (med != null)
                    facade.RegisterMediator(med);
            }
            facade.SendNotification(UINotifications.FADE_IN_LOADING, "Loading...");

        }

        public override void OnRemove()
        {

        }

        public override string[] ListNotificationInterests()
        {
            return new string[] { "test" };
        }

        public override void HandleNotification(INotification notification)
        {
            switch (notification.Name)
            {
                case "test":

                    break;
            }
        }

        public override string MediatorName { get => "Game"; set { } }

        public void RegisterCommands()
        {
            facade.RegisterCommand(PlayerNotification.HERO_EQUIP_ITEM, new EquipWeaponCommand());
        }
    }
      


    public class EquipWeaponCommand : SimpleCommand
    {
        public override void Execute(INotification notification)
        {
        
        }
    }
