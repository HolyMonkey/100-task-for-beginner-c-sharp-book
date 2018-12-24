using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace Commands
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class CommandsAnalyzer : DiagnosticAnalyzer
    {
        public const string DiagnosticId = "CommandDublicate";

        private static readonly LocalizableString Title = "Command Dublicate";
        private static readonly LocalizableString MessageFormat = "≈сть команда с таким же названием";
        private static readonly LocalizableString Description = "¬ проекте есть команда с таким же названием";
        private const string Category = "Error";

        private static DiagnosticDescriptor Rule = new DiagnosticDescriptor(DiagnosticId, Title, MessageFormat, Category, DiagnosticSeverity.Error, isEnabledByDefault: true, description: Description);

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get { return ImmutableArray.Create(Rule); } }
        public static readonly string AttributeName = "CommandAttribute";

        public override void Initialize(AnalysisContext context)
        {
            context.RegisterSymbolAction(AnalyzeSymbol, SymbolKind.NamedType);
        }

        private static void AnalyzeSymbol(SymbolAnalysisContext context)
        {
            var namedTypeSymbol = (INamedTypeSymbol)context.Symbol;
            var commands = new GetAllCommandsVisitor(namedTypeSymbol);
            commands.Visit(context.Compilation.GlobalNamespace);

            var declaredAttributes = namedTypeSymbol.GetAttributes();
            foreach (var attribute in declaredAttributes)
            {
                if (attribute.AttributeClass.Name == AttributeName)
                {
                    if (commands.Commands.Contains(attribute.ConstructorArguments[0].ToCSharpString()))
                    {
                        var diagnostic = Diagnostic.Create(Rule, namedTypeSymbol.Locations[0], namedTypeSymbol.Name);

                        context.ReportDiagnostic(diagnostic);
                    }
                }
            }
        }
    }

    public class GetAllCommandsVisitor : SymbolVisitor
    {
        public List<string> Commands = new List<string>();
        public INamedTypeSymbol Ignored;

        public GetAllCommandsVisitor(INamedTypeSymbol ignored)
        {
            Ignored = ignored;
        }

        public override void VisitNamespace(INamespaceSymbol symbol)
        {
            Parallel.ForEach(symbol.GetMembers(), s => s.Accept(this));
        }

        public override void VisitNamedType(INamedTypeSymbol symbol)
        {
            if (symbol != Ignored)
            {
                foreach (var attribute in symbol.GetAttributes())
                {
                    if (attribute.AttributeClass.Name == CommandsAnalyzer.AttributeName)
                    {
                        Commands.Add(attribute.ConstructorArguments[0].ToCSharpString());
                    }
                }
            }
        }
    }
}
