using CORE.APP.Domain;

namespace APP.Users.Domain
{
    public class UserSkill : Entity
    {
        public int UserId { get; set; }
        public int SkillId { get; set; }

        public User _User { get; set; }
        public Skill _Skill { get; set; }
    }
}
