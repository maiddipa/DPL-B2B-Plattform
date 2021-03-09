namespace Dpl.B2b.Dal.Models
{
    // TODO Is it enough to have the ability to set this on org level
    public class CustomerIpSecurityRule : OlmaAuditable
    {
        public int Id { get; set; }

        public OrganisationSecurityRuleType Type { get; set; }

        public OrganisationIpVersion IpVersion { get; set; }

        public string Value { get; set; }
    }

    public enum OrganisationIpVersion
    {
        V4 = 0,
        V6 = 1
    }

    public enum OrganisationSecurityRuleType
    {
        IP = 0,
        Subnet = 1
    }
}