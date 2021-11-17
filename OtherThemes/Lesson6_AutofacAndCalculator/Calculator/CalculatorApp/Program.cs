using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Autofac;
using CalculatorApp;
using Sharprompt;

//===================================================

var container = new ContainerBuilder().RegisterMany(builder =>
{
    builder.RegisterType<Calculator>();
    builder.AddOperation("Sum", "Summation of two operands", Operations.Sum);
    builder.AddOperation("Sub", "Subtraction of two operands", Operations.Subtract);
    builder.AddOperation("Mult", "Multiply two operands", Operations.Multiply);
    builder.AddOperation("Div", "Division of two operands", Operations.Division);
}).Build();

//===================================================

container.Resolve<Calculator>().Start();

//===================================================

class Calculator
{
    private readonly IEnumerable<OperationMetadata> _operations;

    public Calculator(IEnumerable<OperationMetadata> operations) => _operations = operations;

    public void Start()
    {
        double? previousResult = null;
        do
        {
            do
            {
                var operation = SelectOperation(previousResult);
                previousResult = operation.Calculate();
                Console.WriteLine($"Result of operation is {previousResult}");
            } while (Prompt.Confirm("Continue calculation with previous result?")); 
        } while (!Prompt.Confirm("You wanna exit?"));
    }

    private CalculationOperation SelectOperation(double? previousResult = null)
    {
        OperationFormModel<double> form;
        if (previousResult is null)
        {
            form = Prompt.AutoForms<OperationFormModel<double>>();
        }
        else
        {
            var rightOperand = Prompt.Input<double>("Input right operand");
            form = new OperationFormModel<double>() { LeftOperand = previousResult.Value, RightOperand = rightOperand };
        }
        
        var operation = Prompt.Select("Select operation", _operations);
        return new(form, operation.Operation);
    }
}


//===================================================

record CalculationOperation(OperationFormModel<double> Form, Operation<double> Operation)
{
    public double Calculate() => Operation.Invoke(Form);
};

delegate T Operation<T>(OperationFormModel<T> formModel);

class OperationFormModel<T>
{
    [Display(Prompt = "Input left operand")]
    public T LeftOperand { get; set; }

    [Display(Prompt = "Input right operand")]
    public T RightOperand { get; set; }
}

static class Operations
{
    public static double Sum(OperationFormModel<double> formModel)
    {
        return formModel.LeftOperand + formModel.RightOperand;
    }

    public static double Subtract(OperationFormModel<double> formModel)
    {
        return formModel.LeftOperand - formModel.RightOperand;
    }

    public static double Multiply(OperationFormModel<double> formModel)
    {
        return formModel.LeftOperand * formModel.RightOperand;
    }

    public static double Division(OperationFormModel<double> formModel)
    {
        return formModel.LeftOperand / formModel.RightOperand;
    }
}