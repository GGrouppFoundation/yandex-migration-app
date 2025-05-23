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
                Headers =
                [
                    new("X-Cloud-Org-ID", @in.OrganizationId)
                ]
            })
        .PipeValue(
            httpApi.SendAsync)
        .Map(
            static success => success.Body.DeserializeFromJson<QueueDetailJson>(),
            static failure => failure.ToStandardFailure("Yandex Tracker API call to get queue detail failed.")
        )
        .Map(
            static queue => MapQueue(queue),
            static failure => failure.MapFailureCode(MapQueueGetFailureCode));

    private static TrackerQueueGetOut MapQueue(QueueDetailJson queue)
        =>
        new()
        {
            Key = queue.Key.OrEmpty(),
            Name = queue.Name.OrEmpty(),
            Lead = new TrackerQueueLead
            {
                Id = queue.Lead?.Id.OrEmpty() ?? string.Empty
            },
            DefaultType = new TrackerQueueDefaultType
            {
                Key = queue.DefaultType?.Key.OrEmpty() ?? string.Empty
            },
            DefaultPriority = new TrackerQueueDefaultPriority
            {
                Key = queue.DefaultPriority?.Key.OrEmpty() ?? string.Empty
            },
            IssueTypesConfig = queue.IssueTypesConfig.Map(issueTypeConfig => new TrackerQueueIssueTypeConfig
            {
                IssueType = new TrackerQueueIssueType
                {
                    Key = issueTypeConfig.IssueType?.Key.OrEmpty() ?? string.Empty
                },
                Workflow = new TrackerQueueWorkflow
                {
                    Id = issueTypeConfig.Workflow?.Id.OrEmpty() ?? string.Empty
                },
                Resolutions = issueTypeConfig.Resolutions.Map(resolution => new TrackerQueueResolution
                {
                    Key = resolution.Key.OrEmpty()
                })
            })
        };

    private static TrackerQueueGetFailureCode MapQueueGetFailureCode(HttpFailureCode httpFailureCode)
        =>
        httpFailureCode switch
        {
            HttpFailureCode.NotFound => TrackerQueueGetFailureCode.NotFound,
            HttpFailureCode.Forbidden => TrackerQueueGetFailureCode.Forbidden,
            _ => TrackerQueueGetFailureCode.Unknown
        };
}