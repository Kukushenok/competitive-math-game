using System;
using System.IO;
using System.Reflection;

namespace AssemblyAnalyzer
{
    class Program
    {
        static void Main(string[] args)
        {
            //if (args.Length == 0)
            //{
            //    Console.WriteLine("Please provide a directory path");
            //    return;
            //}

            string directoryPath = "D:\\STUDWORK\\sixth_semester\\ppo\\src\\TechnologicalUI\\bin\\Debug\\net8.0";//args[0];

            if (!Directory.Exists(directoryPath))
            {
                Console.WriteLine("Directory does not exist");
                return;
            }

            string[] dllFiles = Directory.GetFiles(directoryPath, "*.dll");

            foreach (string dllPath in dllFiles)
            {
                //try
                //{
                    var info = System.Diagnostics.FileVersionInfo.GetVersionInfo(dllPath);
                    if (info.FileVersion == "1.0.0.0")
                    {
                        Assembly assembly = Assembly.LoadFrom(dllPath);
                        AnalyzeAssembly(assembly);
                    }
                //}
                //catch (Exception ex)
                //{
                //    Console.WriteLine($"Could not load {dllPath}: {ex.Message}");
                //}
            }
        }

        static void AnalyzeAssembly(Assembly assembly)
        {
            Console.WriteLine($"\nAnalyzing assembly: {assembly.GetName().Name}\n");

            foreach (Type type in assembly.GetTypes())
            {
                if (GetTypeName(type).Contains("d__")) continue;
                if (type.IsClass || type.IsInterface)
                {

                    PrintTypeInfo(type);
                }
            }
        }

        static void PrintTypeInfo(Type type)
        {
            Console.WriteLine(type.Namespace);
            Console.WriteLine($"{(type.IsInterface ? "Interface" : "Class")}: {GetTypeName(type)}");

            // Base types and interfaces
            if (type.BaseType != null && type.BaseType != typeof(object))
            {
                Console.WriteLine($"    {GetTypeName(type.BaseType)}");
            }

            Type[] interfaces = type.GetInterfaces();
            if (interfaces.Length > 0)
            {

                foreach (Type i in interfaces)
                {
                    Console.WriteLine($"    {GetTypeName(i)}");
                }
            }

            PrintMembers(type);
            Console.WriteLine();
            Console.ReadKey();
        }

        static void PrintMembers(Type type)
        {
            BindingFlags flags = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;

            // Fields
            FieldInfo[] fields = type.GetFields(flags);
            if (fields.Length > 0)
            {
                Console.WriteLine("==== Fields:");
                foreach (FieldInfo field in fields)
                {
                    Console.WriteLine($"{GetFieldSignature(field)}");
                }
            }
            // Properties
            PropertyInfo[] properties = type.GetProperties(flags);
            if (properties.Length > 0)
            {
                Console.WriteLine("==== Properties:");
                foreach (PropertyInfo property in properties)
                {
                    Console.WriteLine($"{GetPropertySignature(property)}");
                }
            }


            // Methods
            MethodInfo[] methods = type.GetMethods(flags);
            if (methods.Length > 0)
            {
                Console.WriteLine("==== Methods:");
                foreach (MethodInfo method in methods)
                {
                    if (!method.IsSpecialName) // Exclude property methods
                    {
                        Console.WriteLine($"{GetMethodSignature(method)}");
                    }
                }
            }


        }

        static string GetTypeName(Type type)
        {
            if (!type.IsGenericType)
                return type.Name;

            string genericArgs = string.Join(", ", type.GetGenericArguments().Select(GetTypeName));
            int index = type.Name.IndexOf('`');
            string name = index < 0 ? type.Name : type.Name.Remove(index);
            if (name == "Nullable") return $"{genericArgs}?";
            return $"{name}<{genericArgs}>";
        }

        static string GetMethodSignature(MethodInfo method)
        {
            string returnType = GetTypeName(method.ReturnType);
            string name = method.Name;

            // Handle generic methods
            if (method.IsGenericMethod)
            {
                Type[] typeArgs = method.GetGenericArguments();
                string genericArgs = string.Join(", ", typeArgs.Select(GetTypeName));
                name += $"<{genericArgs}>";
            }

            string parameters = string.Join(", ", method.GetParameters()
                .Select(p => $"{GetTypeName(p.ParameterType)} {p.Name}"));

            return $"{returnType} {name}({parameters})";
        }

        static string GetPropertySignature(PropertyInfo property)
        {
            string accessors = "";
            if (property.CanRead) accessors += "get; ";
            if (property.CanWrite) accessors += "set; ";
            return $"{GetTypeName(property.PropertyType)} {property.Name} {{ {accessors}}}";
        }

        static string GetFieldSignature(FieldInfo field)
        {
            return $"{GetTypeName(field.FieldType)} {field.Name}";
        }
    }
}