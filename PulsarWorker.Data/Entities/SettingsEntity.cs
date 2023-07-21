namespace PulsarWorker.Data.Entities;

public sealed class SettingsEntity
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string Key { get; set; } //TODO reformat to enum
    public string? Value { get; set; }
}