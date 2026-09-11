Write-Host "Executando simulação CONTROLADA dentro do laboratório EduSecure..." -ForegroundColor Yellow
docker compose --profile demo run --rm attack-simulator
Write-Host "Agora confira Grafana, Alertmanager e o painel administrativo." -ForegroundColor Cyan
