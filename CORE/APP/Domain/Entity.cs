namespace CORE.APP.Domain
{
    public abstract class Entity
    {
		public int Id { get; set; }

		protected Entity(int id)
		{
			Id = id;
		}

        protected Entity()
        {
        }
    }
}
