namespace AAIA.Shared.Contracts.V3;

/// <summary>
/// Canonical, stable permission identifiers shared by AAIAS, SDK consumers,
/// extension manifests and registry validators.
///
/// Authorization decisions remain server-owned. Consumers must treat unknown
/// permission identifiers as unsupported and fail closed.
/// </summary>
public static class AaiaPermissions
{
    public const string ContractVersion = "0.7.2";

    public static class Duki
    {
        public const string Observe = "duki.observe";
        public const string Analyze = "duki.analyze";
        public const string Work = "duki.work";
        public const string Communicate = "duki.communicate";
        public const string Admin = "duki.admin";
        public const string Approve = "duki.approve";
    }

    public static class Email
    {
        public const string Read = "email.read";
        public const string ReplyDraft = "email.reply-draft";
        public const string Send = "email.send";
    }

    public static IReadOnlySet<string> All { get; } = new HashSet<string>(StringComparer.Ordinal)
    {
        Duki.Observe,
        Duki.Analyze,
        Duki.Work,
        Duki.Communicate,
        Duki.Admin,
        Duki.Approve,
        Email.Read,
        Email.ReplyDraft,
        Email.Send
    };

    public static bool IsKnown(string? permission) =>
        permission is not null && All.Contains(permission);
}
