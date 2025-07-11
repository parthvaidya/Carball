

public static class ServiceLocator
{
    private static GameManager _gameManager;

    public static GameManager GameManager => _gameManager;

    public static void Register(GameManager gameManager)
    {
        _gameManager = gameManager;
    }

    public static void Clear()
    {
        _gameManager = null;
    }
}