namespace GGroupp.Yandex.Migration;

public interface ITrackerApi :
    IOrganizationListGetSupplier,
    IQueueListGetSupplier,
    IQueueGetSupplier,
    IQueueCreateSupplier,
    IUserGetSupplier;