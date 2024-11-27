using System.Reflection;
using MethodDecorator.Fody.Interfaces;

// 3. 定義攔截器特性
namespace FodyConsole;

public class LogAttribute : Attribute, IMethodDecorator
{

    public void Init(object instance, MethodBase method, object[] args)
    {
        Console.WriteLine("init");
    }

    public void OnEntry()
    {
        Console.WriteLine("on entry");
    }

    public void OnExit()
    {
        Console.WriteLine("on exit");
    }

    public void OnException(Exception exception)
    {
        Console.WriteLine("on exception");
    }
}