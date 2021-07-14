namespace Jaxofy.Models.Enums
{
    /// <summary>
    /// The various states a <see cref="Proposal"/> can be in.
    /// </summary>
    public enum ProposalStateType : byte
    {
        Created = 0,
        Checked = 1,
        Preselected = 2,
        Rejected = 3,
        Accepted = 4,
        Expired = 5
    }
}