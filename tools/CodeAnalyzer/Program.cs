using System.CommandLine;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Text.RegularExpressions;

var rootCommand = new RootCommand("Pure Roslyn Cyclomatic Complexity Analyzer");

var pathOption = new Option<DirectoryInfo>(
    name: "--path",
    description: "Path to the directory with C# source files",
    getDefaultValue: () => new DirectoryInfo(Directory.GetCurrentDirectory())
);

var limitOption = new Option<int>(
    name: "--limit",
    description: "Maximum allowed cyclomatic complexity per method",
    getDefaultValue: () => 10
);

rootCommand.AddOption(pathOption);
rootCommand.AddOption(limitOption);

rootCommand.SetHandler((DirectoryInfo path, int limit) =>
{
    bool failed = AnalyzeDirectory(path.FullName, limit);

    Console.ForegroundColor = failed ? ConsoleColor.Red : ConsoleColor.Green;
    Console.WriteLine(
        failed
            ? $"\n❌ Cyclomatic Complexity exceeded limit ({limit})."
            : $"\n✅ All methods are within complexity limit ({limit})."
    );
    Console.ResetColor();

    Environment.Exit(failed ? 1 : 0);
}, pathOption, limitOption);

return await rootCommand.InvokeAsync(args);


/// <summary>
/// Analyze all .cs files in a directory recursively.
/// </summary>
static bool AnalyzeDirectory(string rootPath, int limit)
{
    Console.WriteLine($"Analyzing directory: {rootPath}");
    var files = Directory.GetFiles(rootPath, "*.cs", SearchOption.AllDirectories)
        .Where(f => !f.Contains(@"\obj\") && !f.Contains(@"\bin\"))
        .ToList();

    bool anyFailed = false;
    foreach (var file in files)
    {
        var code = File.ReadAllText(file);
        var syntaxTree = CSharpSyntaxTree.ParseText(code);
        var root = syntaxTree.GetRoot();

        var methods = root.DescendantNodes().OfType<BaseMethodDeclarationSyntax>();
        foreach (var method in methods)
        {
            int cc = CalculateCyclomaticComplexity(method);
            if (cc > limit)
            {
                anyFailed = true;
                Console.WriteLine(
                    $"❌ {IdentifierOrName(method)}(): CC={cc} (>{limit})  File: {file}");
            }
        }
    }

    return anyFailed;
}


/// <summary>
/// Calculate Cyclomatic Complexity based on control flow constructs.
/// </summary>
static int CalculateCyclomaticComplexity(SyntaxNode method)
{
    // Base complexity = 1
    int complexity = 1;

    // Each control-flow construct adds one path
    var keywords = new[]
    {
        SyntaxKind.IfStatement,
        SyntaxKind.ForStatement,
        SyntaxKind.ForEachStatement,
        SyntaxKind.ForEachVariableStatement,
        SyntaxKind.WhileStatement,
        SyntaxKind.DoStatement,
        SyntaxKind.CaseSwitchLabel,
        SyntaxKind.ConditionalExpression,
        SyntaxKind.CatchClause,
        SyntaxKind.LogicalAndExpression,
        SyntaxKind.LogicalOrExpression,
        SyntaxKind.CoalesceExpression,
        SyntaxKind.WhenClause
    };

    foreach (var kind in keywords)
        complexity += method.DescendantNodes().Count(n => n.IsKind(kind));

    return complexity;
}

/// <summary>
/// Helper: Extract a readable name from method declaration.
/// </summary>
static string IdentifierOrName(BaseMethodDeclarationSyntax method)
{
    return method switch
    {
        MethodDeclarationSyntax m => m.Identifier.Text,
        ConstructorDeclarationSyntax c => c.Identifier.Text,
        DestructorDeclarationSyntax d => "~" + d.Identifier.Text,
        _ => "(anonymous)"
    };
}
