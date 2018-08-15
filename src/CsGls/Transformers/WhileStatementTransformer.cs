using System.Collections.Generic;
using CsGls.GlsInternals;
using CsGls.Results;
using CsGls.Routing;
using CsGls.Transformers;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CsGls.Transformers
{
    public class WhileStatementTransformer : INodeTransformer<WhileStatementSyntax>
    {
        private readonly SemanticModel Model;
        private readonly TransformerRouter Router;

        public WhileStatementTransformer(SemanticModel model, TransformerRouter router)
        {
            this.Model = model;
            this.Router = router;
        }

        public ITransformation VisitNode(WhileStatementSyntax node)
        {
            return new ChildTransformations(
                new ITransformation[]
                {
                    new CommandTransformation(
                        CommandNames.WhileStart,
                        Range.ForNode(node.Condition),
                        this.Router.RouteNode(node.Condition)
                    ),
                    this.Router.RouteNode(node.Statement),
                    new CommandTransformation(
                        CommandNames.WhileEnd,
                        Range.AfterNode(node)
                    )
                },
                Range.ForNode(node)
            );
        }
    }
}