namespace SWE.Time.Enum
{
    public enum TimeGranularity
    {
        Second = 1,
        Minute = 60,
        Quarter = 900,
        Hour = 3_600,
        Day = 86_400,
        Week = 604_800,
        //Month = 2_592_000,
        Month = 2_629_800,
        //QuarterYear = 7.776.000,
        QuarterYear = 7_889_400,
        Year = 31_557_600,
    }
}