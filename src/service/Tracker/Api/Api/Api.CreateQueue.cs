using System;
using System.Threading;
using System.Threading.Tasks;
using GarageGroup.Infra;
using PrimeFuncPack.Core;

namespace GGroupp.Yandex.Migration;

partial class TrackerApi
{
    public ValueTask<Result<TrackerQueueCreateOut, Failure<TrackerQueueCreateFailureCode>>> CreateQueueAsync(
        TrackerQueueCreateIn input, CancellationToken cancellationToken)
        =>
        AsyncPipeline.Pipe(
            input, cancellationToken)
        .Pipe(
            static @in => new QueueCreateJsonIn
            {
                Key = @in.Queue.Key,
                Name = @in.Queue.Name,
                Lead = @in.Queue.Lead,
                DefaultType = @in.Queue.DefaultType,
                DefaultPriority = @in.Queue.DefaultPriority,
                IssueTypesConfig = @in.Queue.IssueTypesConfig.Map(MapIssueTypeConfig)
            })
        .Pipe(
            @in => new HttpSendIn(HttpVerb.Post, "/v3/queues")
            {
                Headers = BuildHeader(input.OrganizationId),
                Body = HttpBody.SerializeAsJson(@in)
            })
        .PipeValue(
            httpApi.SendAsync)
        .Map(
            static success => success.Body.DeserializeFromJson<QueueCreateJsonOut>(),
            ReadTrackerFailure)
        .Map(
            MapQueue,
            static failure => failure.MapFailureCode(MapQueueCreateFailureCode));

    private static QueueCreateJsonIn.IssueTypeConfig MapIssueTypeConfig(
        TrackerQueueCreateIn.IssueTypeConfig issueTypeConfig)
        =>
        new()
        {
            IssueType = issueTypeConfig.IssueType,
            Workflow = issueTypeConfig.Workflow,
            Resolutions = issueTypeConfig.Resolutions
        };

    private static TrackerQueueCreateOut MapQueue(QueueCreateJsonOut queue)
        =>
        new()
        {
            Id = queue.Id,
            Key = queue.Key.OrEmpty(),
            Name = queue.Name.OrEmpty(),
            Lead = MapLead(queue.Lead)
        };

    private static TrackerQueueCreateOut.QueueLead MapLead(QueueCreateJsonOut.QueueLead lead)
        =>
        new()
        {
            Id = lead.Id.OrEmpty(),
            Display = lead.Display.OrEmpty()
        };

    private static TrackerQueueCreateFailureCode MapQueueCreateFailureCode(HttpFailureCode failureCode)
        =>
        failureCode switch
        {
            HttpFailureCode.BadRequest => TrackerQueueCreateFailureCode.BadRequest,
            HttpFailureCode.Forbidden => TrackerQueueCreateFailureCode.Forbidden,
            HttpFailureCode.Unauthorized => TrackerQueueCreateFailureCode.Unauthorized,
            HttpFailureCode.NotFound => TrackerQueueCreateFailureCode.ReferenceNotFound,
            HttpFailureCode.Conflict => TrackerQueueCreateFailureCode.Conflict,
            HttpFailureCode.UnprocessableContent => TrackerQueueCreateFailureCode.BadRequest,
            _ => TrackerQueueCreateFailureCode.Unknown
        };
}