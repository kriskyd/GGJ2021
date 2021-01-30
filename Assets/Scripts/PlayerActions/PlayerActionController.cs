using PlayerActions;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerActionController : MonoBehaviour
{
	[SerializeField]
	private List<PlayerActionProvider> playerActionsProviders = new List<PlayerActionProvider>();

	private List<PlayerAction> playerActions = new List<PlayerAction>();

	private void Awake()
	{
		foreach(var playerActionsProvider in playerActionsProviders)
		{
			playerActions.Add(playerActionsProvider.Provide());
		}
	}

	private void Update()
	{
		if(Input.GetButtonDown("Action"))
		{
			PlayerAction possibleAction = SelectPossibleAction();

			if(possibleAction == null)
			{
				return;
			}

			possibleAction.Perform();
		}
	}

	private PlayerAction SelectPossibleAction()
	{
		return playerActions.FirstOrDefault(action => action.CanPerformAction);
	}
}