using Microsoft.AspNetCore.Mvc;

namespace Fujitsu.CvQc.Business
{
    public class ServerResponse<T>
    {
        public bool Success { get; set; } = true;

        public List<ValidationMessage> Messages { get; set; } = new();

        public T? Result { get; set; }

        public void AddValidationMessage(string title, string message = "", string severity = SeverityFlags.Danger) 
        {
            AddValidationMessage(true, title, message, severity);
        }

        public void AddValidationMessage(bool unsuccessful, string title, string message = "", string severity = SeverityFlags.Danger) 
        {
            var validationMessage = new ValidationMessage();
            validationMessage.Title = title;
            validationMessage.Message = message;
            validationMessage.Severity = severity;
            Messages.Add(validationMessage);

            if (unsuccessful)
                Success = false;
        }
    }
}
