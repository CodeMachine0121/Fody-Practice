using System;
using System.Collections.Generic;
using Fody;
using Mono.Cecil;
using Mono.Cecil.Cil;
using Mono.Cecil.Rocks;

public class ModuleWeaver : BaseModuleWeaver
{
    public override void Execute()
    {
        var handlerType = ModuleDefinition.ImportReference(typeof(InterceptionHandler));
        var onMethodEnteredMethod = ModuleDefinition.ImportReference(
            typeof(InterceptionHandler).GetMethod("OnMethodEntered"));
        Console.WriteLine("Execute method");

        foreach (TypeDefinition type in ModuleDefinition.Types)
        {
            if (ShouldProcessType(type) || true)
            {
                foreach (MethodDefinition method in type.Methods)
                {
                    if (ShouldProcessMethod(method) || true)
                    {
                        try 
                        {
                            InjectInterceptor(method, onMethodEnteredMethod);
                        }
                        catch (Exception ex)
                        {
                            WriteWarning($"Failed to inject into {method.FullName}: {ex.Message}");
                        }
                    }
                }
            }
        }
    }

    private void InjectInterceptor(MethodDefinition method, MethodReference handlerMethod)
    {
        if (!method.HasBody)
            return;

        var body = method.Body;
        var il = body.GetILProcessor();

        var instructions = body.Instructions;
        if (instructions.Count == 0)
            return;

        // 保存原始指令
        var originalInstructions = new List<Instruction>(instructions);
        
        // 清空方法體
        instructions.Clear();

        // 注入對 InterceptionHandler.OnMethodEntered 的調用
        il.Append(il.Create(OpCodes.Ldstr, method.Name));
        il.Append(il.Create(OpCodes.Call, handlerMethod));

        // 重新添加原始指令
        foreach (var instruction in originalInstructions)
        {
            il.Append(instruction);
        }

        // 確保方法體仍然有效
        body.OptimizeMacros();
    }

    private bool ShouldProcessMethod(MethodDefinition method)
    {
        return method.IsPublic &&
               method.HasBody &&
               !method.IsConstructor &&
               !method.IsVirtual &&
               !method.IsGetter &&
               !method.IsSetter;
    }

    private bool ShouldProcessType(TypeDefinition type)
    {
        return type.IsPublic &&
               !type.IsInterface &&
               !type.IsEnum;
    }

    public override IEnumerable<string> GetAssembliesForScanning()
    {
        yield return "netstandard";
        yield return "mscorlib";
    }
}

public class InterceptionHandler
{
    public static void OnMethodEntered(string methodName)
    {
        Console.WriteLine($"[Intercepte111d] Method: {methodName}");
        // 這裡可以加入其他你想要的邏輯
        // 例如：記錄時間、寫入日誌等
    }
}
