using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using TestHelper;
using Commands;

namespace Commands.Test
{
    [TestClass]
    public class UnitTest : CodeFixVerifier
    {

        //No diagnostics expected to show up
        [TestMethod]
        public void TestMethod1()
        {
            var test = @"";

            VerifyCSharpDiagnostic(test);
        }

        //Diagnostic and CodeFix both triggered and checked for
        [TestMethod]
        public void TestMethod2()
        {
            var test = @"
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Diagnostics;

    namespace ConsoleApplication1
    {
        [Command(""Test"")]
        class TypeName
        {   
        }

        [Command(""Test"")]
        class TypeName2
        {   
        }

        [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = false)]
        class CommandAttribute : Attribute
        {
            public string CommandName;

            public CommandAttribute(string commandName)
            {
                CommandName = commandName;
            }
        }
    }";
            var expected = new DiagnosticResult
            {
                Id = "CommandDublicate",
                Message = "Есть команда с таким же названием",
                Severity = DiagnosticSeverity.Error,
                Locations =
                    new[] {
                            new DiagnosticResultLocation("Test0.cs", 12, 15)
                        }
            };

            var expected2 = new DiagnosticResult
            {
                Id = "CommandDublicate",
                Message = "Есть команда с таким же названием",
                Severity = DiagnosticSeverity.Error,
                Locations =
                         new[] {
                               new DiagnosticResultLocation("Test0.cs", 17, 15)
                            }
            };

            VerifyCSharpDiagnostic(test, expected, expected2);

        }

        protected override DiagnosticAnalyzer GetCSharpDiagnosticAnalyzer()
        {
            return new CommandsAnalyzer();
        }
    }
}
