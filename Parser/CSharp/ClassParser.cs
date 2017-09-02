using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Parser.CSharp.Modules;
using System.Collections.Generic;
using System.Linq;

namespace Parser.CSharp
{
    public class ClassParser
    {
        public static SemanticModel Model { get; set; }

        public ClassParser(SemanticModel model)
        {
            Model = model;
        }

        public IEnumerable<ClassModel> Parse()
        {
            return Model.SyntaxTree.GetRoot().DescendantNodes().OfType<ClassDeclarationSyntax>()
                .Select(cls => Model.GetDeclaredSymbol(cls))
                .Select(ConvertClass);
        }

        private ClassModel ConvertClass(INamedTypeSymbol symbol)
        {
            return new ClassModel
            {
                Name = symbol.Name,
                Description = CommentHelper.GetSummary(symbol.GetDocumentationCommentXml()),
                Methods = symbol.GetMembers().OfType<IMethodSymbol>()
                    .Where(method => !symbol.Constructors.Contains(method) && method.DeclaredAccessibility == Accessibility.Public)
                    .Select(method =>
                    {
                        var methodModel = new MethodModel
                        {
                            Name = method.Name,
                        };
                        var commentXml = CommentHelper.GetXmlDocument(method.GetDocumentationCommentXml());
                        methodModel.Description = CommentHelper.GetSummary(commentXml);

                        var httpMethodAttr = method.GetAttributes()
                           .FirstOrDefault(attr => attr.AttributeClass.Name.Equals("HttpGet") || attr.AttributeClass.Name.Equals("HttpPost"));
                        if (httpMethodAttr != null)
                        {
                            methodModel.HttpMethod = httpMethodAttr.AttributeClass.Name.Replace("Http", string.Empty).ToUpper();
                        }
                        methodModel.Parameters = method.Parameters.Select(para =>
                        new ParameterModel
                        {
                            Name = para.Name,
                            Type = para.Type.Name,
                            DefaultValue = para.HasExplicitDefaultValue ? (para.ExplicitDefaultValue ?? string.Empty).ToString() : null,
                            Description = CommentHelper.GetParamByName(commentXml, para.Name),
                        });
                        return methodModel;
                    }),
            };
        }
    }
}
