namespace SWE.Issue.Abstractions.Enumerations
{
    public enum IssueClassification
    {
        Unknown = 1,

        Exception = 100,

        UnExpected = 120,

        UnDeliverable = 200,

        UnAcknowledged = 250,

        Missing = 300,

        OutOfRange = 1000,

        OutOfDate = 1100
    }
}