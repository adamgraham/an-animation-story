using UnityEngine;
using UnityEngine.AI;

public static class NavMashExtensions
{
	public static bool HasReachedDestination(this NavMeshAgent agent)
	{
		if (!agent.hasPath) {
			return true;
		}

		if (!agent.isActiveAndEnabled || agent.pathPending) {
			return false;
		}

		if (agent.remainingDistance > agent.stoppingDistance) {
			return false;
		}

		return true;
	}

}
