namespace App.Common.Enums.Extenders
{
    using ComplexTypes;

    public static partial class EnumTextExtender
    {
        public static string ToText(this ResultStatus operation)
        {
            return operation switch
            {
                ResultStatus.Success => "Success",
                ResultStatus.Error => "Error",
                ResultStatus.Info => "Information",
                ResultStatus.Warning => "Warning",
                _ => ""
            };
        }
    }
}
