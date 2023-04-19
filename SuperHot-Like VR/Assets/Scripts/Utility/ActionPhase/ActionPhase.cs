namespace Utility
{
	namespace ActionPhase
	{
		public interface IActionPhase
		{
			public UnityEngine.GameObject gameObject { get; }
			public bool isActive { get; }
		}

		public interface IInitialize : IActionPhase
		{
			public void Initialize();
		}

		public interface IFrameUpdate : IActionPhase
		{
			public void FrameUpdate();
		}

		public interface IPhysicsUpdate : IActionPhase
		{
			public void PhysicsUpdate();
		}

		public interface IPostUpdate : IActionPhase
		{
			public void PostUpdate();
		}
	}
}