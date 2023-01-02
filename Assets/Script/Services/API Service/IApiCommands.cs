namespace Assets.Script.Services.API_Service.Login
{
    public interface IApiCommands
    {
        void Send<T>(HttpRequestAction<T> actions);

    }
}
