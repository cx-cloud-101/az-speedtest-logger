using System.ComponentModel.DataAnnotations;

namespace SpeedTestLogger.Models;

public record TestResult(
    [Required]
    Guid SessionId,

    [StringLength(500, MinimumLength = 2)]
    [Required]
    string User,

    [Range(1, int.MaxValue)]
    [Required]
    int Device,

    [Range(0, long.MaxValue)]
    [Required]
    long Timestamp,

    [Required]
    TestData Data);

public record TestData(
    [Required]
    TestSpeeds Speeds,

    [Required]
    TestClient Client,

    [Required]
    TestServer Server);

public record TestSpeeds(
    [Range(0, 3000)]
    [Required]
    double Download,

    [Range(0, 3000)]
    [Required]
    double Upload);

public record TestClient(
    [RegularExpression(@"^(?:(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.){3}(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)$")]
    [Required]
    string Ip,

    [Range(-90, 90)]
    double Latitude,

    [Range(-180, 180)]
    double Longitude,

    [StringLength(500, MinimumLength = 2)]
    [Required]
    string Isp,

    [RegularExpression(@"^([A-Z]){2}$")]
    [Required]
    string Country);

public record TestServer(
    [StringLength(500, MinimumLength = 2)]
    [Required]
    string Host,

    [Range(-90, 90)]
    [Required]
    double Latitude,

    [Range(-180, 180)]
    [Required]
    double Longitude,

    [RegularExpression(@"^([A-Z]){2}$")]
    [Required]
    string Country,

    [Range(0, 21000000)]
    [Required]
    double Distance,

    [Range(0, int.MaxValue)]
    [Required]
    int Ping,

    [Range(0, int.MaxValue)]
    [Required]
    int Id);
