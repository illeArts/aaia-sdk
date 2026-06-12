namespace AAIA.Shared.Contracts.Security;

/// <summary>
/// Vertrauensstufe eines installierten Moduls/Plugins.
/// Die Stufe bestimmt welche Rechte AAIAS gewährt — nicht die Lizenz.
/// Lizenz und Trust sind bewusst getrennte Ebenen.
/// </summary>
public enum TrustLevel
{
    /// <summary>
    /// Paket wurde empfangen, noch keine Prüfung durchgeführt.
    /// Kein Code läuft. Dateien liegen im Quarantäne-Verzeichnis.
    /// </summary>
    Quarantined = 0,

    /// <summary>
    /// Hash und Manifest sind gültig, aber Signatur fehlt oder ist unbekannt.
    /// Sideload / lokale Entwicklung. Eingeschränkte Rechte, kein Netzwerkzugriff.
    /// </summary>
    LocalUnverified = 10,

    /// <summary>
    /// Signatur mathematisch gültig, Zertifikat lokal gecacht und noch nicht abgelaufen.
    /// Online-Prüfung war nicht möglich (Offline-Betrieb).
    /// Volles Rechtemodell aktiv, aber keine Live-Revocation-Prüfung.
    /// </summary>
    OfflineVerified = 20,

    /// <summary>
    /// Signatur gültig, ETW-ID bekannt, Inspector bestanden, Revocation-Status online geprüft.
    /// Volles Rechtemodell aktiv.
    /// </summary>
    Verified = 30,

    /// <summary>
    /// Modul wurde nach erfolgreicher Verifikation nachträglich gesperrt.
    /// ETW-ID, Key oder Modul-ID steht auf der Revocation-Liste.
    /// Modul wird nicht mehr ausgeführt. Benutzer wird informiert.
    /// </summary>
    Revoked = -1,

    /// <summary>
    /// Modul hat beim Inspector oder bei der Signaturprüfung eine kritische Prüfung nicht bestanden.
    /// Dauerhaft blockiert bis manuell entsperrt (Admin-only).
    /// </summary>
    Blocked = -2
}

/// <summary>
/// Quelle der Installation — bestimmt initiales Trust-Level und erlaubte Upgrade-Pfade.
/// </summary>
public enum InstallSource
{
    /// <summary>Vom AAIA Marketplace heruntergeladen (signiert, lizenziert).</summary>
    Marketplace,

    /// <summary>Offline-Paket mit gültiger AAIA-Signatur (ohne Online-Verifikation).</summary>
    OfflinePackage,

    /// <summary>Manuell in Modul-Ordner kopiert oder per Pfad installiert.</summary>
    Sideload,

    /// <summary>Lokal entwickelt, Developer Mode aktiv.</summary>
    DeveloperLocal
}

/// <summary>
/// Abgestufte Rechte eines Moduls basierend auf TrustLevel + InstallSource.
/// AAIAS wertet diese beim Aktivieren aus — das Modul selbst kann sie nicht ändern.
/// </summary>
public sealed record ModuleTrustProfile(
    TrustLevel    Trust,
    InstallSource Source,
    bool          NetworkAllowed,
    bool          DatabaseWriteAllowed,
    bool          FileSystemSandboxOnly,
    bool          DukiDirectAllowed,
    bool          ShellExecuteAllowed,
    bool          AutoUpdateAllowed)
{
    /// <summary>
    /// Standard-Rechte für eine gegebene Trust/Source-Kombination.
    /// AAIAS kann einzelne Rechte per Admin-Override erweitern oder einschränken.
    /// </summary>
    public static ModuleTrustProfile From(TrustLevel trust, InstallSource source) => trust switch
    {
        TrustLevel.Verified => new(trust, source,
            NetworkAllowed:        true,
            DatabaseWriteAllowed:  true,
            FileSystemSandboxOnly: true,
            DukiDirectAllowed:     false,   // Duki.Direct ist nie automatisch
            ShellExecuteAllowed:   false,
            AutoUpdateAllowed:     source == InstallSource.Marketplace),

        TrustLevel.OfflineVerified => new(trust, source,
            NetworkAllowed:        true,
            DatabaseWriteAllowed:  true,
            FileSystemSandboxOnly: true,
            DukiDirectAllowed:     false,
            ShellExecuteAllowed:   false,
            AutoUpdateAllowed:     false),  // kein Auto-Update ohne Online-Verifikation

        TrustLevel.LocalUnverified => new(trust, source,
            NetworkAllowed:        false,
            DatabaseWriteAllowed:  false,
            FileSystemSandboxOnly: true,
            DukiDirectAllowed:     false,
            ShellExecuteAllowed:   false,
            AutoUpdateAllowed:     false),

        TrustLevel.Quarantined => new(trust, source,
            NetworkAllowed:        false,
            DatabaseWriteAllowed:  false,
            FileSystemSandboxOnly: true,
            DukiDirectAllowed:     false,
            ShellExecuteAllowed:   false,
            AutoUpdateAllowed:     false),

        _ => new(trust, source,           // Revoked / Blocked → alles verboten
            NetworkAllowed:        false,
            DatabaseWriteAllowed:  false,
            FileSystemSandboxOnly: true,
            DukiDirectAllowed:     false,
            ShellExecuteAllowed:   false,
            AutoUpdateAllowed:     false)
    };
}
