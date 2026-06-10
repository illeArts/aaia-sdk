namespace AAIA.Shared.Contracts.Client;

[Flags]
public enum LibraryAccessMode
{
    None = 0,
    ServerOnly = 1,
    ReadThroughServer = 2,
    ReadCache = 4,
    WriteRequestsViaServer = 8
}
