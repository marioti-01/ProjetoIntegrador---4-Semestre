Write-Host "Subindo EduSecure 360..." -ForegroundColor Cyan
docker compose up -d --build
Write-Host "Portal: http://localhost:8080"
Write-Host "Swagger: http://localhost:5000/swagger"
Write-Host "Grafana: http://localhost:3000 (admin/admin)"
Write-Host "Prometheus: http://localhost:9090"
Write-Host "Alertmanager: http://localhost:9093"
Write-Host "n8n: http://localhost:5678"
