using System.CommandLine;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

var rootCommand = new RootCommand("Halstead Analyzer (per-method mode)");

var pathOption = new Option<DirectoryInfo>(
    name: "--path",
    description: "Path to the source directory",
    getDefaultValue: () => new DirectoryInfo(Directory.GetCurrentDirectory())
);

var limitOption = new Option<int>(
    name: "--limit",
    description: "Maximum allowed Halstead Volume before failing",
    getDefaultValue: () => 1000
);

rootCommand.AddOption(pathOption);
rootCommand.AddOption(limitOption);

rootCommand.SetHandler((DirectoryInfo path, int limit) =>
{
    bool anyFailed = AnalyzeProject(path.FullName, limit);

    if (anyFailed)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.Error.WriteLine($"\n❌ Halstead Volume exceeded limit ({limit}).");
        Console.ResetColor();
        Environment.Exit(1);
    }
    else
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"\n✅ All methods are within Halstead Volume limit ({limit}).");
        Console.ResetColor();
    }
}, pathOption, limitOption);

return await rootCommand.InvokeAsync(args);

static bool AnalyzeProject(string rootPath, int limit)
{
    Console.WriteLine($"Analyzing project in: {rootPath}");
    var csFiles = Directory.GetFiles(rootPath, "*.cs", SearchOption.AllDirectories)
        .Where(f => !f.Contains(@"\obj\") && !f.Contains(@"\bin\"))
        .ToList();

    bool failed = false;
    foreach (var file in csFiles)
    {
        if (AnalyzeFile(file, limit))
            failed = true;
    }

    return failed;
}

static bool AnalyzeFile(string filePath, int limit, bool verbose = false)
{
    var code = File.ReadAllText(filePath);
    var syntaxTree = CSharpSyntaxTree.ParseText(code);
    var root = syntaxTree.GetRoot();

    var methods = root.DescendantNodes().OfType<MethodDeclarationSyntax>().ToList();
    bool fileFailed = false;

    if (!methods.Any())
        return false;
    if(verbose) Console.WriteLine($"\n📄 File: {Path.GetFileName(filePath)}");

    foreach (var method in methods)
    {
        if (AnalyzeMethod(method, limit, verbose))
        {
            Console.WriteLine($"in file: {filePath}\n");
            fileFailed = true;
        }
    }

    return fileFailed;
}

static bool AnalyzeMethod(MethodDeclarationSyntax method, int limit, bool verbose)
{
    var methodName = method.Identifier.Text;

    var tokens = method.DescendantTokens().ToList();
    var operatorTokens = tokens.Where(t => IsOperator(t.Text)).ToList();
    var operandTokens = tokens.Where(t => IsOperand(t.Text)).ToList();

    var n1 = operatorTokens.Distinct().Count();
    var n2 = operandTokens.Distinct().Count();
    var N1 = operatorTokens.Count;
    var N2 = operandTokens.Count;

    if (n1 + n2 == 0)
        return false;

    double programVocabulary = n1 + n2;
    double programLength = N1 + N2;
    double volume = programLength * Math.Log2(programVocabulary);
    double difficulty = (n1 / 2.0) * (N2 / (double)Math.Max(1, n2));
    double effort = difficulty * volume;

    bool overLimit = volume > limit;
    if (verbose)
    {
        Console.WriteLine($"  • {methodName}(): Volume={volume:F2} {(overLimit ? "❌" : "✅")}, Difficulty={difficulty:F2}, Effort={effort:F2}");
    } 
    else if(overLimit)
    {
        Console.WriteLine($"❌ {methodName}(): Volume={volume:F2}, Difficulty={difficulty:F2}, Effort={effort:F2}");
    }
    return overLimit;
}

static bool IsOperator(string token)
{
    var operators = new[]
    {
        "+", "-", "*", "/", "%", "=", "==", "!=", "<", ">", "<=", ">=",
        "&&", "||", "!", "&", "|", "^", "++", "--", "+=", "-=", "*=", "/=",
        "=>", ".", "?", ":", "??", "??=", "->", "new", "return", "if", "else",
        "for", "while", "switch", "case", "break", "continue", "throw", "try", "catch"
    };
    return operators.Contains(token);
}

static bool IsOperand(string token)
{
    return !string.IsNullOrWhiteSpace(token)
           && token.All(c => char.IsLetterOrDigit(c) || c == '_')
           && !IsOperator(token);
}
