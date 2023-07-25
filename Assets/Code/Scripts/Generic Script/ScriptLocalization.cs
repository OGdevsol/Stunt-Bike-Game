
public static class ScriptLocalization
{
      public static  string  LevelTitle => Get("Level : ");
      public static  string  AddCoin => Get("CoinRewarded");
      public static  string  UpgradeBike => Get("UpgradeBikeRewarded");
      public static  string  UnlockAllLevel => Get("UnlockAllLevelRewarded");
      public static  string  UnlockFullGame => Get("UnlockFullGameRewarded");

      public static  string  AllGame => Get("AllGame");
      public static  string  AllBikes => Get("AllBikes");
      public static  string  AllLevels => Get("AllLevels");
      
      public static  string  AllGameRemainingVideo => Get("AllGameRemainingVideo");
     
      public static  string  AllLevelsRemainingVideo => Get("AllLevelsRemainingVideo");
      
      public static  string  BikeRewarded => Get("BikeRewarded");
      
      public static  string  RemoveAdRewarded => Get("RemoveAdRewarded");

      public static  string  UpgradeMulitplier => Get("UpgradeMulitplier");

      public static  string  BikeHandling => Get("BikeHandling");
      public static  string  BikeGrip => Get("BikeGrip");
      public static  string  BikeSpeed => Get("BikeSpeed");

    public static string  Get(string Term)
    {
        return Term;
    }
}
