using System;
using System.Threading;
using System.Threading.Tasks;
using GarageGroup.Infra;

namespace GGroupp.Yandex.Migration;

partial class TrackerApi
{
    public ValueTask<Result<TrackerQueueGetOut, Failure<TrackerQueueGetFailureCode>>> GetQueueAsync(
        TrackerQueueGetIn input, CancellationToken cancellationToken)
        =>
        AsyncPipeline.Pipe(
            input, cancellationToken)
        .Pipe(
            static @in => new HttpSendIn(HttpVerb.Get, $"/v3/queues/{@in.QueueId}?expand=issueTypesConfig")
            {
                Headers = BuildHeader(@in.OrganizationId)
            })
        .PipeValue(
            httpApi.SendAsync)
        .Map(
            static success => success.Body.DeserializeFromJson<QueueDetailJson>(),
            ReadTrackerFailure)
        .Map(
            MapQueue,
            static failure => failure.MapFailureCode(MapQueueGetFailureCode));

    private static TrackerQueueGetOut MapQueue(QueueDetailJson queue)
        =>
        new()
        {
            Key = queue.Key.OrEmpty(),
            Name = queue.Name.OrEmpty(),
            Lead = new TrackerQueueGetOut.QueueLead
            {
                Id = queue.Lead?.Id ?? string.Empty
            },
            DefaultType = new TrackerQueueGetOut.QueueDefaultType
            {
                Key = queue.DefaultType?.Key ?? string.Empty
            },
            DefaultPriority = new TrackerQueueGetOut.QueueDefaultPriority
            {
                Key = queue.DefaultPriority?.Key ?? string.Empty
            },
            IssueTypesConfig = queue.IssueTypesConfig.Map(MapIssueTypeConfig)
        };

    private static TrackerQueueGetOut.QueueIssueTypeConfig MapIssueTypeConfig(QueueDetailJson.IssueTypeConfigJson issueTypeConfig)
        =>
        new()
        {
            IssueType = new TrackerQueueGetOut.QueueIssueType
            {
                Key = issueTypeConfig.IssueType?.Key ?? string.Empty
            },
            Workflow = new TrackerQueueGetOut.QueueWorkflow
            {
                Id = issueTypeConfig.Workflow?.Id ?? string.Empty
            },
            Resolutions = issueTypeConfig.Resolutions.Map(MapResolution)
        };

    private static TrackerQueueGetOut.QueueResolution MapResolution(QueueDetailJson.ResolutionJson resolution)
        =>
        new()
        {
            Key = resolution.Key.OrEmpty()
        };

    private static TrackerQueueGetFailureCode MapQueueGetFailureCode(HttpFailureCode httpFailureCode)
        =>
        httpFailureCode switch
        {
            HttpFailureCode.NotFound => TrackerQueueGetFailureCode.NotFound,
            HttpFailureCode.Unauthorized => TrackerQueueGetFailureCode.Unauthorized,
            HttpFailureCode.Forbidden => TrackerQueueGetFailureCode.Forbidden,
            _ => TrackerQueueGetFailureCode.Unknown
        };
}