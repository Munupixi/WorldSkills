using System;
using System.Collections.Generic;

namespace WorldSkills.Models;

public partial class Request
{
    public Request(int requestId, int? statusId, int? executorId, string? clientCompleteName, string? clientPhone, string? title, string? level, string? additional)
    {
        RequestId = requestId;
        StatusId = statusId;
        ExecutorId = executorId;
        ClientCompleteName = clientCompleteName;
        ClientPhone = clientPhone;
        Title = title;
        Level = level;
        Additional = additional;
    }

    public int RequestId { get; set; }

    public int? StatusId { get; set; }

    public int? ExecutorId { get; set; }

    public string? ClientCompleteName { get; set; }

    public string? ClientPhone { get; set; }

    public string? Title { get; set; }

    public string? Level { get; set; }

    public string? Additional { get; set; }

    public virtual User? Executor { get; set; }

    public virtual Status? Status { get; set; }
}
