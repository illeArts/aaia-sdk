@echo off
chcp 65001 >nul
cd /d "H:\AAIAGitHub\aaia-sdk"
git add .
git commit -m "fix: suppress CS1587 + add .gitattributes"
git push origin main
echo === Done! ===
pause
