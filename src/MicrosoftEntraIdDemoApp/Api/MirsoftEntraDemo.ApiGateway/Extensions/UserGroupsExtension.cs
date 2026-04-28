using System.Security.Claims;

namespace MirsoftEntraDemo.ApiGateway.Extensions
{
    public static class UserGroupsExtension
    {
        extension(Claim claim)
        {
            public string GetGoupName() =>
                claim.Value switch
                {
                    "7d8c1230-8e85-4421-8f07-4eb9dcae7812" => "App-Testers",
                    "b3ed6fe5-0ea5-4be4-8d5f-91c4bd247cfd" => "SuperDruperAdminZDuper",
                    "9b7d25da-24a0-41fc-9fb7-c58cf60a167a" => "Testowa",
                    _ => claim.Value
                };
        }
    }
}
