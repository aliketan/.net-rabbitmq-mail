namespace App.Common.Utility.Results.Concrete
{
    using Enums.ComplexTypes;
    using Contracts;
    using Enums.Extenders;

    public class DialogResult:IDialogResult
    {
        public DialogResult(ResultStatus status)
        {
            Status = status.ToText();
        }

        public DialogResult(ResultStatus status, string response)
        {
            Status = status.ToText();
            Response = response;
        }

        public string Status { get; }

        public string Response { get; }
    }
}
