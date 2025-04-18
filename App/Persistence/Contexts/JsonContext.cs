using System.Text.Json.Serialization;
using App.Common.Utility.Results.Contracts;
using App.Domain.Dto;
using App.Domain.Models;

namespace App.Persistence.Contexts
{
    [JsonSerializable(typeof(string))]
    [JsonSerializable(typeof(IDialogResult))]
    [JsonSerializable(typeof(SmtpModel))]
    [JsonSerializable(typeof(WelcomeMessageDto))]
    public partial class AppJsonSerializerContext : JsonSerializerContext
    {
    }
}