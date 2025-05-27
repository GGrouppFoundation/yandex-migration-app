using System;
using System.Threading;
using System.Threading.Tasks;
using GarageGroup.Infra;

namespace GGroupp.Yandex.Migration;

partial class TrackerApi
{
    public ValueTask<Result<TrackerQueueCreateOut, Failure<TrackerQueueCreateFailureCode>>> CreateQueueAsync(
        TrackerQueueCreateIn input, CancellationToken cancellationToken)
        =>
        AsyncPipeline.Pipe(
            input, cancellationToken)
        .Pipe(
            static @in => new HttpSendIn(HttpVerb.Post, $"/v3/queues")
            {
                Headers =
                [
                    new("X-Cloud-Org-ID", @in.OrganizationId)
                ],
                Body = HttpBody.SerializeAsJson(@in.Queue)
            })
        .PipeValue(
            httpApi.SendAsync)
        .Map(
            static success => success.Body.DeserializeFromJson<QueueCreateJson>(),
            static failure => failure.ToStandardFailure("Yandex Tracker API call to create queue failed:"))
        .Map(
            MapQueue,
            static failure => failure.MapFailureCode(MapQueueCreateFailureCode));

    private static TrackerQueueCreateOut MapQueue(QueueCreateJson queue)
        =>
        new()
        {
            Id = queue.Id.OrEmpty(),
            Key = queue.Key.OrEmpty(),
            Name = queue.Name.OrEmpty(),
            Lead = MapLead(queue.Lead)
        };

    private static TrackerQueueCreateOut.QueueLead MapLead(QueueCreateJson.QueueLead lead)
        =>
        new()
        {
            Id = lead.Id ?? string.Empty,
            Display = lead.Display ?? string.Empty
        };

    private static TrackerQueueCreateFailureCode MapQueueCreateFailureCode(HttpFailureCode failureCode)
        =>
        failureCode switch
        {
            HttpFailureCode.BadRequest => TrackerQueueCreateFailureCode.BadRequest,
            HttpFailureCode.Forbidden => TrackerQueueCreateFailureCode.Forbidden,
            HttpFailureCode.NotFound => TrackerQueueCreateFailureCode.ReferenceNotFound,
            HttpFailureCode.Conflict => TrackerQueueCreateFailureCode.Conflict,
            HttpFailureCode.UnprocessableContent => TrackerQueueCreateFailureCode.BadRequest,
            _ => TrackerQueueCreateFailureCode.Unknown
        };
}