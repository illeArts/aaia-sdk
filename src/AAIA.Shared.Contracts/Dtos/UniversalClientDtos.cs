// Universal Client DTOs — alle Typen die IAaiaServerClient benötigt
// Fehlende DTOs aus Windows Client + iOS Client Analyse (2026-06-03)

namespace Aaia.Shared.Contracts.Dtos;

// ── WorkOrders ────────────────────────────────────────────────────────────────

public sealed record WorkOrderDto(
    string Id,
    string Title,
    string Goal,
    string Status,
    string? AssignedOffice,
    string? WorkspaceRoot,
    string? Priority,
    IReadOnlyList<string>? RouteHistory,
    DateTimeOffset CreatedAtUtc,
    DateTimeOffset UpdatedAtUtc,
    int StepCount,
    int ArtifactCount);

// ── Memory (Legacy) ───────────────────────────────────────────────────────────

public sealed record MemorySearchResultDto(
    string Path,
    string ProjectName,
    string Title,
    string Snippet,
    int Score);

// ── Core Memory ───────────────────────────────────────────────────────────────

public sealed record CoreStatusDto(
    bool Available,
    string? DatabasePath,
    CoreStatsDto? Stats);

public sealed record CoreStatsDto(
    int Projects,
    int KnowledgeItems,
    int Decisions,
    int Solutions,
    int Errors,
    int Conversations,
    int InboxEvents,
    int AuditLogEntries);

public sealed record CoreProjectDto(
    string Id,
    string Slug,
    string Title,
    string Status,
    string Domain,
    string Genre,
    string? Purpose,
    string? ProjectPath);

public sealed record CoreKnowledgeItemDto(
    string Id,
    string? ProjectId,
    string Type,
    string Title,
    string Body,
    string Status,
    double Confidence,
    int Importance,
    DateTimeOffset CreatedAt);

public sealed record CoreDecisionDto(
    string Id,
    string? ProjectId,
    string Title,
    string Decision,
    string? Reason,
    string Status,
    DateTimeOffset DecidedAt);

public sealed record CoreSolutionDto(
    string Id,
    string? ProjectId,
    string Problem,
    string Solution,
    string? Environment,
    string Status,
    DateTimeOffset CreatedAt);

public sealed record CoreErrorDto(
    string Id,
    string? ProjectId,
    string Title,
    string Symptom,
    string? Cause,
    string? FixSummary,
    string Status,
    DateTimeOffset CreatedAt);

public sealed record CoreInboxEventDto(
    string Id,
    string? AgentId,
    string? ProjectId,
    string EventType,
    string PayloadJson,
    string Status,
    DateTimeOffset CreatedAt);

public sealed record CoreSearchResultDto(
    string ObjectType,
    string ObjectId,
    string Title,
    string Snippet,
    string ProjectSlug,
    double Rank);

public sealed record CoreContextDto(string Context);

public sealed record CoreAddedDto(string Id);

// ── Agent ─────────────────────────────────────────────────────────────────────

public sealed record AgentRunRequestDto(
    string Goal,
    string? ProjectSlug = null,
    IReadOnlyDictionary<string, string>? Parameters = null);

public sealed record AgentRunResultDto(
    string RunId,
    string Status,
    string? Output,
    string? Error);

public sealed record AgentToolDto(
    string Name,
    string Description,
    bool RequiresApproval);

// ── DUKI ──────────────────────────────────────────────────────────────────────

public sealed record DukiActionDto(
    string Id,
    string Title,
    string Description,
    string Risk,
    string Status,
    string RequestedBy,
    DateTimeOffset CreatedAt,
    DateTimeOffset? ExpiresAt);

public sealed record DukiPolicyModeDto(string Name, string Description, bool Active);

public sealed record DukiAuditEntryDto(
    string Id,
    string ActionId,
    string Event,
    string Actor,
    DateTimeOffset OccurredAt);

public sealed record DukiAuditVerifyDto(bool Valid, string? Reason);

// ── Users ─────────────────────────────────────────────────────────────────────

public sealed record UserDto(
    string Id,
    string DisplayName,
    string Email,
    string Role,
    bool IsActive);

// ── Logs ──────────────────────────────────────────────────────────────────────

public sealed record LogEntryDto(
    string Id,
    string Level,
    string Message,
    string? Source,
    DateTimeOffset Timestamp);

// ── Backups ───────────────────────────────────────────────────────────────────

public sealed record BackupInfoDto(
    string Name,
    long SizeBytes,
    DateTimeOffset CreatedAt);

// ── Plugins ───────────────────────────────────────────────────────────────────

public sealed record PluginInfoDto(
    string Id,
    string Name,
    string Version,
    bool Enabled,
    bool Signed);

// ── Providers ─────────────────────────────────────────────────────────────────

public sealed record ProviderStatusDto(
    string KeyName,
    string DisplayName,
    bool HasKey,
    bool IsLocal);

public sealed record OllamaPingDto(bool Reachable, string Url, string? ErrorMessage);

// ── Settings ──────────────────────────────────────────────────────────────────

public sealed record ServerSettingsDto(
    DatabaseSettingsDto Database,
    SecuritySettingsDto Security,
    UserDirectorySettingsDto Users,
    AiRuntimeSettingsDto Ai,
    EmailSettingsDto Email,
    ModulePluginSettingsDto Modules);

public sealed record DatabaseSettingsDto(
    string Provider,
    string ConnectionString,
    string BackupPath,
    bool AutomaticBackupsEnabled);

public sealed record SecuritySettingsDto(
    bool RequireApiKey,
    bool AllowRemoteAccess,
    bool RequireVpnOrHttps,
    bool AuditLogEnabled,
    string AdminRoleName);

public sealed record UserDirectorySettingsDto(
    bool LocalUsersEnabled,
    bool SelfRegistrationEnabled,
    string DefaultRole);

public sealed record AiRuntimeSettingsDto(
    string PrimaryProvider,
    bool OllamaEnabled,
    string OllamaBaseUrl,
    bool OpenAiEnabled,
    bool ClaudeEnabled,
    bool GeminiEnabled,
    string DefaultLocalModel);

public sealed record EmailSettingsDto(
    bool EmailModuleEnabled,
    bool GmailEnabled,
    bool MicrosoftMailEnabled,
    bool ImapSmtpEnabled,
    bool LearningFromMailAllowed);

public sealed record ModulePluginSettingsDto(
    string PluginPath,
    bool AutoScanEnabled,
    bool AllowUnsignedPlugins,
    bool RequirePluginPermissions);

// ── Voice / Lector ────────────────────────────────────────────────────────────

public sealed record VoiceTranscribeResultDto(
    string Transcript,
    string ResponseText,
    string? Intent,
    string? TargetModule,
    bool RequiresConfirmation,
    bool IsBlocked,
    string? Error);

public sealed record VoiceStatusDto(
    bool Active,
    string Mode,
    string SttEngine,
    string TtsEngine);

// ── Security — Devices ────────────────────────────────────────────────────────

public sealed record DeviceRegistrationRequestDto(
    string DeviceId,
    string DeviceName,
    string DeviceType,
    string PlatformVersion,
    string AppVersion,
    string? PublicKey);

public sealed record DeviceRegistrationResponseDto(
    string DeviceId,
    string TrustStatus,
    string? ServerKey);

public sealed record DeviceDto(
    string Id,
    string Name,
    string Type,
    string TrustStatus,
    DateTimeOffset RegisteredAt);

// ── Security — Agent Whitelist ────────────────────────────────────────────────

public sealed record SignedWhitelistDto(
    string Signature,
    IReadOnlyList<WhitelistRuleDto> Rules,
    DateTimeOffset SignedAt);

public sealed record WhitelistRuleDto(
    string AgentId,
    string Permission,
    string Scope,
    bool Allowed);

// ── Emergency ─────────────────────────────────────────────────────────────────

public sealed record EmergencyStatusDto(
    bool DukiPaused,
    bool AgentsStopped,
    bool ReadOnlyMode,
    DateTimeOffset CheckedAt);

// ── Messaging (Communication Hub) ────────────────────────────────────────────

public sealed record MessagingProviderStatusDto(
    string ProviderId,
    string DisplayName,
    bool   IsConnected,
    string? AccountName,
    DateTimeOffset? ConnectedAt);

public sealed record MessagingContactDto(
    string Id,
    string DisplayName,
    Dictionary<string, string> Channels,
    string? PreferredProvider,
    bool DirectAllowed);

public sealed record MessageSendResultDto(
    bool   Success,
    string? JobId,
    string? ProviderMessageId,
    string? ErrorMessage,
    bool   RequiresConfirm);

public sealed record MessageJobDto(
    string   Id,
    string   ProviderId,
    string   RecipientId,
    string   RecipientName,
    string   Text,
    string   Status,
    bool     RequiresConfirmation,
    string?  CreatedByUserId,
    DateTimeOffset CreatedAt,
    DateTimeOffset? SentAt,
    string?  ErrorMessage,
    string?  ProviderMessageId);
