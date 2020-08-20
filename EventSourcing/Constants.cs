namespace LightestNight.System.EventSourcing
{
    public static class Constants
    {
        public const string SystemStreamPrefix = "@";
        public const string VersionKey = "version";
        public const string CheckpointPrefix = "checkpoint";
        public const string CheckpointMessageType = "checkpoint";
        public const string CheckpointIntervalKey = "EventStore:Checkpoints:Interval";
        public const string GlobalCheckpointId = "global";
        public const string TimestampKey = "timestamp";
    }
}