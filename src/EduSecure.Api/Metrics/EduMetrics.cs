using Prometheus;
namespace EduSecure.Api.Metrics;
public static class EduMetrics
{
    public static readonly Counter LoginAttempts = Prometheus.Metrics.CreateCounter(
        "edusecure_login_attempts_total", "Tentativas de login.", new CounterConfiguration { LabelNames = new[] { "result" } });
    public static readonly Counter SecurityEvents = Prometheus.Metrics.CreateCounter(
        "edusecure_security_events_total", "Eventos de segurança.", new CounterConfiguration { LabelNames = new[] { "event" } });
    public static readonly Counter Incidents = Prometheus.Metrics.CreateCounter(
        "edusecure_incidents_total", "Incidentes criados.", new CounterConfiguration { LabelNames = new[] { "type", "severity" } });
    public static readonly Gauge ActiveIncidents = Prometheus.Metrics.CreateGauge(
        "edusecure_active_incidents", "Incidentes ainda não resolvidos.");
    public static readonly Counter HttpRequests = Prometheus.Metrics.CreateCounter(
        "edusecure_http_requests_total", "Requisições HTTP da aplicação.", new CounterConfiguration { LabelNames = new[] { "method", "status" } });
    public static readonly Histogram HttpDuration = Prometheus.Metrics.CreateHistogram(
        "edusecure_http_request_duration_seconds", "Duração de requisições HTTP em segundos.");
}
