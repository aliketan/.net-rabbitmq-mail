namespace App.Common.Utility.Results.Contracts
{
    public interface IDialogResult
    {
        public string Status { get; }
        public string Response { get; }
    }
}
