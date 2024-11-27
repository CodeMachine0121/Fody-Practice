
using System.Reflection;

namespace FodyConsole;

public static class FodyConsole
{
    public static void Main(string[] args)
    {
        var service = new UserService();
        
        // 會先輸出 "interceptor"，然後才是 "Creating user: John"
        service.CreateUser("John"); 
    }
    
}

public class UserService
{
    public void CreateUser(string username)
    {
        Console.WriteLine($"Creating user: {username}");
    }

    public string GetUserInfo(int userId)
    {
        return $"User info for ID: {userId}";
    }
}