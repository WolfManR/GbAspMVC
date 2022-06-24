namespace CalculatorApp
{
    class OperationMetadata
    {
        public Operation<double> Operation { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public override string ToString() => $"{Name} : {Description}";
    }
}