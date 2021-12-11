using FoldersTranspiler.Data.Instructions.Commands;
using FoldersTranspiler.Data.Instructions.Expressions;
using FoldersTranspiler.Services;

namespace FoldersTranspiler.Converters;

public class CommandConverter
{
    private readonly TranspilerService _transpiler;

    public CommandConverter(TranspilerService transpiler)
    {
        _transpiler = transpiler;
    }


    public List<BaseCommand> ConvertStatements(SyntaxList<StatementSyntax> statements)
    {
        var programCommands = new List<BaseCommand>();
        foreach (var statement in statements)
        {
            List<BaseCommand> commands = new();
            commands = !commands.Any() ? ConvertDeclaration(statement as LocalDeclarationStatementSyntax) : commands;
            commands = !commands.Any() ? ConvertExpression(statement as ExpressionStatementSyntax) : commands;
            if (!commands.Any())
            {
                var ifCommand = ConvertIf(statement as IfStatementSyntax);
                if(ifCommand != null) commands.Add(ifCommand);
                var whileCommand = ConvertWhile(statement as WhileStatementSyntax);
                if(whileCommand != null) commands.Add(whileCommand);
            }
            programCommands.AddRange(commands);
        }

        return programCommands;
    }

    public List<BaseCommand> ConvertDeclaration(LocalDeclarationStatementSyntax? syntax)
    {
        var commands = new List<BaseCommand>();
        if (syntax == null) return commands;
        var type = _transpiler.TypeConverter.Convert(syntax.Declaration.Type);

        foreach (var variable in syntax.Declaration.Variables)
        {
            var variableCommands = ConvertDeclarator(variable);
            commands.AddRange(variableCommands);
        }


        return commands;
    }

    public List<BaseCommand> ConvertDeclarator(VariableDeclaratorSyntax? syntax)
    {
        var commands = new List<BaseCommand>();
        if (syntax == null) return commands;
        var type = _transpiler.TypeConverter.Convert(syntax.Parent as VariableDeclarationSyntax);
        var varName = syntax.Identifier.ValueText;
        var varNumber = _transpiler.DeclareVariable(varName);
        if (type != null && varNumber > 0)
        {
            var declarationCommand = new DeclareCommand(varNumber, type.Value);
            commands.Add(declarationCommand);
        }

        var valueExpression = _transpiler.ExpressionConverter.ConvertExpression(syntax.Initializer?.Value);
        if (valueExpression != null)
        {
            var letCommand = new LetCommand(varNumber, valueExpression);
            commands.Add(letCommand);
        }

        return commands;
    }

    public List<BaseCommand> ConvertExpression(ExpressionStatementSyntax? syntax)
    {
        var commands = new List<BaseCommand>();
        if (syntax == null) return commands;

        var expression = syntax.Expression;
        if (expression is InvocationExpressionSyntax invocation)
        {
            var method = string.Join('.', invocation.Expression.ChildNodes());
            if (method == "Console.Write" || method == "Console.WriteLine")
            {
                var arguments = invocation.ArgumentList.Arguments;
                if (arguments.Count != 1)
                    throw new NotSupportedException($"Print with more than 1 argument is not supported.");
                var arg = _transpiler.ExpressionConverter.ConvertExpression(arguments[0].Expression);
                arg = arg != null && method == "Console.WriteLine"
                    ? new AddExpression(arg, new LiteralValueExpression(Environment.NewLine))
                    : arg;
                if (arg == null) throw new NotSupportedException($"Unable to find expression from print {syntax}");
                var printCommand = new PrintCommand(arg);
                commands.Add(printCommand);
            }
        }
        else if (expression is AssignmentExpressionSyntax assignment)
        {
            var varName = (assignment.Left as IdentifierNameSyntax)?.Identifier.ValueText;
            var varNumber = _transpiler.GetVariableNumber(varName);
            if (varNumber != null && !string.IsNullOrEmpty(varName))
            {
                if (assignment.Right is InvocationExpressionSyntax assignInvocation)
                {
                    var method = string.Join('.', assignInvocation.Expression.ChildNodes());
                    if (method == "Console.ReadLine")
                    {
                        commands.Add(new InputCommand(varNumber.Value));
                    }
                }
                else
                {
                    var assignExpression = _transpiler.ExpressionConverter.ConvertExpression(assignment.Right);
                    if (assignExpression != null)
                    {
                        commands.Add(new LetCommand(varNumber.Value, assignExpression));
                    }
                }
            }
        }

        return commands;
    }

    public BaseCommand? ConvertIf(IfStatementSyntax? syntax)
    {
        if (syntax == null) return null;

        var conditionExpression = _transpiler.ExpressionConverter.ConvertExpression(syntax.Condition);
        if (conditionExpression != null)
        {
            var commands = ConvertBlock(syntax.Statement as BlockSyntax);
            if (commands.Any())
            {
                var command = new IfCommand(conditionExpression, commands);
                return command;
            }
        }
        return null;
    }

    public BaseCommand? ConvertWhile(WhileStatementSyntax? syntax)
    {
        if (syntax == null) return null;

        var conditionExpression = _transpiler.ExpressionConverter.ConvertExpression(syntax.Condition);
        if (conditionExpression != null)
        {
            var commands = ConvertBlock(syntax.Statement as BlockSyntax);

            if (commands.Any())
            {
                var command = new WhileCommand(conditionExpression, commands);
                return command;
            }
        }
        return null;
    }

    private List<BaseCommand> ConvertBlock(BlockSyntax? syntax)
    {
        var commands = new List<BaseCommand>();
        if(syntax == null) return commands;

        var statementsCommands = ConvertStatements(syntax.Statements);
        if (statementsCommands.Any())
        {
            commands.AddRange(statementsCommands);
        }

        return commands;
    }
}