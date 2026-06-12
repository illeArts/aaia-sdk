@echo off
chcp 65001 >nul
cd /d "H:\AAIAGitHub\aaia-sdk"

echo.
echo === aaia-sdk - Commit + Push ===
echo.

git add .

git commit -m "feat: Publisher + Security contracts; Marketplace DTOs erweitert

- Publisher/ Namespace hinzugefuegt (RegisterPublisherKeyRequest/Response, PublisherKeyDto etc.)
- Security/ Namespace hinzugefuegt
- MarketplaceDtos: DeveloperLoginRequest/Response, DeveloperRegisterRequest/Response, DeveloperAccountDto
- ExtensionManifestDtos: aktualisiert
- AaiaApiRoutes: aktualisiert
- release-contracts scripts hinzugefuegt"

echo.
echo Pushing to GitHub...
git push origin main

echo.
echo === Done! ===
pause
